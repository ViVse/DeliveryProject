using Common.Logging;
using Polly;
using Polly.Extensions.Http;
using Serilog;
using Shopping.Aggregator.Services;
using Shopping.Aggregator.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<LoggingDelegatingHandler>();

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.
builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderApi"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>()
    .AddPolicyHandler(GetRetryPolicy())
    .AddPolicyHandler(GetCircuitBreakerPolicy());

builder.Services.AddHttpClient<IUserService, UserService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:UserApi"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddHttpClient<IShopService, ShopService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogApi"]))
    .AddHttpMessageHandler<LoggingDelegatingHandler>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (exception, retryCount, context) =>
            {
                Log.Error($"Retry {retryCount} of {context.PolicyKey} at {context.OperationKey}, due to: {exception}.");
            });
}

static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
{
    return HttpPolicyExtensions
        .HandleTransientHttpError()
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        );
}

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
