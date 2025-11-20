using ByCodersTecApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Database: SQL Server via EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
    //options.ConfigureWarnings(w => w.Log(RelationalEventId.PendingModelChangesWarning));
});

// Repositories & UoW
builder.Services.AddScoped<ByCodersTecApi.Infrastructure.Repositories.Interfaces.IStoreRepository, ByCodersTecApi.Infrastructure.Repositories.Implementations.StoreRepository>();
builder.Services.AddScoped<ByCodersTecApi.Infrastructure.Repositories.Interfaces.ITransactionRepository, ByCodersTecApi.Infrastructure.Repositories.Implementations.TransactionRepository>();
builder.Services.AddScoped<ByCodersTecApi.Infrastructure.Repositories.Interfaces.IUnitOfWork, ByCodersTecApi.Infrastructure.Repositories.Implementations.UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Apply pending EF Core migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
