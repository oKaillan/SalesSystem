using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SalesSystem.API.Profiles;
using SalesSystem.Database;
using SalesSystem.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SalesSystemApi", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddDbContext<SalesSystemContext>();
builder.Services.AddTransient<DAL<Employee>>();
builder.Services.AddTransient<DAL<Product>>();
builder.Services.AddTransient<DAL<ProductCategory>>();
builder.Services.AddTransient<DAL<SalesLog>>();
builder.Services.AddAutoMapper(cfg =>
{
    // aqui você pode configurar globalmente se precisar
}, typeof(Program).Assembly);

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.Run();
