using Application.Common.Interfaces;
using Application.Orders.Commands.CreateOrder;
using AutoMapper;
using BLL.Configurations;
using BLL.Factories;
using BLL.Interfaces;
using BLL.Interfaces.Services;
using BLL.Services;
using DAL;
using DAL.Data;
using DAL.Data.Repositories;
using DAL.Entities;
using DAL.Interfaces;
using DAL.Interfaces.Repositories;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
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
builder.Services.AddTransient<IDeliveryManRepository, DeliveryManRepository>();
builder.Services.AddTransient<IReviewRepository, ReviewRepository>();

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IShopService, ShopService>();
builder.Services.AddTransient<IReviewService, ReviewService>(); 
builder.Services.AddTransient<IDeliveryManService, DeliveryManService>();
builder.Services.AddTransient<IIdentityService, IdentityService>();
builder.Services.AddTransient<IUsersService, UsersService>();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<JwtTokenConfiguration>();
builder.Services.AddTransient<IJwtTokenFactory, JwtSecurityTokenFactory>();

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddSignInManager<SignInManager<User>>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<Context>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey"])),
            ClockSkew = TimeSpan.Zero,
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine(context.Exception.Message);
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

//Clean
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(CreateOrderCommand).GetTypeInfo().Assembly);
//builder.Services.AddApplicationServices();

builder.Services.AddScoped<IMongoDbContext>(s =>
{
    var mediator = s.GetRequiredService<IMediator>();
    var connectionString = builder.Configuration.GetConnectionString("mongoDb");
    return new MongoDbContext(mediator, connectionString, builder.Configuration["MongoDbName"]);
});

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

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.
                                    Enter 'Bearer' [space] and then your token in the
                                    text input below. Example: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
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

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
