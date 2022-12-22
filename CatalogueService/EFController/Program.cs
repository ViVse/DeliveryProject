using AutoMapper;
using BLL.Configurations;
using BLL.Interfaces.Services;
using BLL.Services;
using DAL;
using DAL.Data;
using DAL.Data.Repositories;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>(options =>
{
    string connectionString = builder.Configuration.GetValue<string>("ConnectionStrings:defaultConnection");
    options.UseSqlServer(connectionString, b => { 
        b.MigrationsAssembly("EFController");
        b.EnableRetryOnFailure();
    });
});

builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<IShopRepository, ShopRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IShopService, ShopService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery.WebAPI",
        Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }