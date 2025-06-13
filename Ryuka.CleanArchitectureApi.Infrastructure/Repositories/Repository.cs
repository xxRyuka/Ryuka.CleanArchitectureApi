    using System.Linq.Expressions;
    using Microsoft.EntityFrameworkCore;
    using Ryuka.NlayerApi.Core.Abstractions;
    using Ryuka.NlayerApi.Infrastructure.Data;

    namespace Ryuka.NlayerApi.Infrastructure.Repositories;

    public class Repository <T>: IRepository<T> where T : class
    {
        
        protected readonly ApiDbContext  _context;

        public Repository(ApiDbContext context)
        {
            _context = context;
            
        }

        public IQueryable<T> Table => _context.Set<T>(); // Queryable olarak donduru veriyi 
        
        public async Task<T> FindAsync(int id) 
        {
           return await _context.Set<T>().FindAsync(id);
        } // Ok
        
        public async Task CreateAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
            
        }

        public T Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            return entity;
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            // opsiyonel as notrack eklicez 
           return _context.Set<T>().Where(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            // bool dondurur sarti saglayan veri var mi kontrol eder 
            return await _context.Set<T>().AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            // int donduru sartı saglayan kaç ver ivar onu kontrol eder 
            return await _context.Set<T>().CountAsync(predicate);
        }

        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            // sarti saglayan ilk veriyi <T> dondurur yoksa null dondurur
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }
    }