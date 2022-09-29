using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Orders.Commands.CreateOrder;
using AutoMapper;
using Infrastructure.Persistence;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(CreateOrderCommand).GetTypeInfo().Assembly);
builder.Services.AddApplicationServices();

builder.Services.AddScoped<IMongoDbContext>(s =>
{
    var mediator = s.GetRequiredService<IMediator>();
    var connectionString = builder.Configuration.GetConnectionString("mongoDb");
    return new MongoDbContext(mediator, connectionString, builder.Configuration["MongoDbName"]);
});

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
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
