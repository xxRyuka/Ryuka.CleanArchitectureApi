using Microsoft.EntityFrameworkCore;
using Ryuka.NlayerApi.Application.Interfaces;
using Ryuka.NlayerApi.Application.Services;
using Ryuka.NlayerApi.Core.Abstractions;
using Ryuka.NlayerApi.Infrastructure.Data;
using Ryuka.NlayerApi.Infrastructure.Repositories;
using Ryuka.NlayerApi.Infrastructure.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApiDbContext>(options =>
{
    string key =
        "Data Source=localhost;Database = SmartParking;Integrated Security=True;Persist Security Info=False;Pooling=False;Multiple Active Result Sets=False;Encrypt=True;Trust Server Certificate=True;Application Name=\"SQL Server Management Studio\";Command Timeout=0";
    var connectionString = key;
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ISlotRepository, SlotRepository>();
builder.Services.AddScoped<IParkingRecordRepository, ParkingRecordRepository>();

builder.Services.AddScoped<IParkingRecordService, ParkingRecordService>();
builder.Services.AddScoped<ISlotService, SlotService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();