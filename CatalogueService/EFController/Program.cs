using AutoMapper;
using BLL.Configurations;
using BLL.Interfaces.Services;
using BLL.Services;
using DAL;
using DAL.Data;
using DAL.Data.Repositories;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<Context>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://localhost:7177", "http://localhost:5177")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .WithExposedHeaders("*");
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }