using Common.Logging;
using Discount.Grpc.Extensions;
using Discount.Grpc.Repositories;
using Discount.Grpc.Repositories.Interfaces;
using Discount.Grpc.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddGrpc();

var app = builder.Build();
app.MigrateDatabase<Program>();

// Configure the HTTP request pipeline.
app.MapGrpcService<CouponService>();
app.MapGrpcService<ProductDiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
