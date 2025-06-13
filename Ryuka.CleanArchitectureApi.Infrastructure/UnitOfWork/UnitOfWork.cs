using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Infrastructure.Data;
using Ryuka.NlayerApi.Infrastructure.Repositories;

namespace Ryuka.NlayerApi.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApiDbContext _context;
    private IDbContextTransaction _transaction;

    public IVehicleRepository Vehicles { get; }
    public ISlotRepository Slots { get; }
    public IParkingRecordRepository ParkingRecords { get; }

    public UnitOfWork(ApiDbContext context)
    {
        _context = context;
        Vehicles = new VehicleRepository(_context);
        Slots = new SlotRepository(_context);
        ParkingRecords = new ParkingRecordRepository(_context);
    }

    public async Task BeginTransaction()
    {
        if (_transaction == null)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }
    }

    public async Task CommitTransaction()
    {
        if (_transaction != null)
        {
            await _context.Database.CommitTransactionAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public async Task RollbackTransaction()
    {
        if (_transaction != null)
        {
            await _context.Database.RollbackTransactionAsync();
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    public void Dispose()
    {
        _context.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
}