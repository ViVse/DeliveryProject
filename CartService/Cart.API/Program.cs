using Cart.API.GrpcServices;
using Cart.API.Repositories;
using Cart.API.Repositories.Interfaces;
using Discount.Grpc.Protos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddStackExchangeRedisCache(options => { 
    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
    options.InstanceName = "Cart_";
});

// General Configuration
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddGrpcClient<ProductDiscountProtoService.ProductDiscountProtoServiceClient>
    (o => o.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]));
builder.Services.AddScoped<ProductDiscountGrpcService>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
