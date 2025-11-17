using KocurmeApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using KocurmeApp.Application;
using MediatR;
using Microsoft.OpenApi.Models;
using KocurmeApp.Infrastructure.Services.FileImport;
using KocurmeApp.Application.Application;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Kocurme APP API",
        Version = "v1",
        Description = "Kocurme APP",
        Contact = new OpenApiContact
        {
            Name = "Kocurme APP",
            Email = "support@kocurmeapp.local"
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly("KocurmeApp.Api")));

builder.Services.AddControllers();
builder.Services.AddApplicationServices();

// Infrastructure Services (Excel Export)
builder.Services.AddInfrastructureServices();

// CQRS / MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(KocurmeApp.Application.AssemblyMarker).Assembly));
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()); 
});
// DBF servisini kaydet
builder.Services.AddTransient<DbfImportService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kocurme APP API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
