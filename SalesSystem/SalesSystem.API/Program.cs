using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SalesSystem.Database;
using SalesSystem.Entities;
using SalesSystem.Shared.Database.Entities;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SalesSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .UseLazyLoadingProxies());


builder.Services.AddAuthorization();

builder.Services.AddControllers().AddNewtonsoftJson();

//builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
//    .AddRoles<IdentityRole>()
//    .AddEntityFrameworkStores<SalesSystemContext>();

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<SalesSystemContext>()
    .AddDefaultTokenProviders();

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
}, typeof(Program).Assembly);

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentitySeed.SeedAsync(services);
}

app.Run();
