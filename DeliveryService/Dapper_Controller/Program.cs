using AutoMapper;
using Common.Logging;
using Dapper_BLL.Configurations;
using Dapper_BLL.Interfaces;
using Dapper_BLL.Services;
using Dapper_DAL.Interfaces;
using Dapper_DAL.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IReviewRepository>((o) => new ReviewRepository(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDeliveryManRepository>((o) => new DeliveryManRepository(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IDeliveryManService, DeliveryManService>();

var mapperConfig = new MapperConfiguration(mc => {
        mc.AddProfile(new MapperProfile());
    });
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
