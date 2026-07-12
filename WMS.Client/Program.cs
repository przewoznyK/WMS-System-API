using WMS.Client.Components;
using MudBlazor.Services;
using WMS.Client.Services;
using WMS.Client.Services.Interfaces;
using WMS.Client.Services.Implementations;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient{ BaseAddress = new Uri("https://localhost:7052")} );
builder.Services.AddScoped<ApiClientService>();
builder.Services.AddScoped<UiUtilityService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IStockMovementService, StockMovementService>();
builder.Services.AddScoped<IStockService, StockService>();
builder.Services.AddScoped<IWarehouseLocationService, WarehouseLocationService>();
builder.Services.AddMudServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
