using Microsoft.EntityFrameworkCore;
using WMS.Api.Middleware;
using WMS.Application.Products.Commands;
using WMS.Application.WarehouseLocations.Commands;
using WMS.Domain.Repositories;
using WMS.Domain.Services;
using WMS.Infrastructure;
using WMS.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
builder.Services.AddScoped<IWarehouseLocationRepository, SqlWarehouseLocationRepository>();
builder.Services.AddScoped<IStockRepository, SqlStockRepository>();
builder.Services.AddScoped<IStockMovementRepository, SqlStockMovementRepository>();
builder.Services.AddScoped<StockService>();
builder.Services.AddDbContext<WmsDbContext>(options => options.UseSqlite("Data Source=wms.db"));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateWarehouseLocationCommand).Assembly));

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();