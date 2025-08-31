using SalesSystem.API.Extensions;
using SalesSystem.Database;
using SalesSystem.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SalesSystemContext>();
builder.Services.AddTransient<DAL<Employee>>();
builder.Services.AddTransient<DAL<Product>>();
builder.Services.AddTransient<DAL<ProductCategory>>();
builder.Services.AddTransient<DAL<SalesLog>>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddEndPointsEmployee();
app.AddEndPointsProduct();
app.AddEndPointsProductCategory();
app.AddEndPointsSalesLog();

app.Run();
