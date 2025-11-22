using ByCodersTecApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//builder.Services.AddApiVersioning(options =>
//{
//    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
//    options.AssumeDefaultVersionWhenUnspecified = true;
//    options.ReportApiVersions = true;
//})
//.AddApiExplorer(options =>
//{
//    options.GroupNameFormat = "'v'V";
//    options.SubstituteApiVersionInUrl = true;
//});

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// Database: SQL Server via EF Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(connectionString, sql =>
    {
        sql.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    });
});

// Repositories & UoW
builder.Services.AddScoped<ByCodersTecApi.Infrastructure.Repositories.Interfaces.IStoreRepository, ByCodersTecApi.Infrastructure.Repositories.Implementations.StoreRepository>();
builder.Services.AddScoped<ByCodersTecApi.Infrastructure.Repositories.Interfaces.ITransactionRepository, ByCodersTecApi.Infrastructure.Repositories.Implementations.TransactionRepository>();
builder.Services.AddScoped<ByCodersTecApi.Infrastructure.Repositories.Interfaces.IUnitOfWork, ByCodersTecApi.Infrastructure.Repositories.Implementations.UnitOfWork>();
builder.Services.AddScoped<ByCodersTecApi.Services.Interfaces.ICnabImportService, ByCodersTecApi.Services.Implementations.CnabImportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "ByCoders API v1");
        c.RoutePrefix = "swagger";
    });
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
