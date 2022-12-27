using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Orders.Commands.CreateOrder;
using AutoMapper;
using Common.Logging;
using EventBus.Messages.Common;
using Infrastructure.Persistence;
using MassTransit;
using MediatR;
using Ordering.API.EventBusConsumer;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

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
builder.Services.AddScoped<BasketCheckoutConsumer>();

//MassTransit & RabbitMq
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);

        cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});

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
