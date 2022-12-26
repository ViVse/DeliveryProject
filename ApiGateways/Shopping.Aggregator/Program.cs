using Shopping.Aggregator.Services;
using Shopping.Aggregator.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient<IOrderService, OrderService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderApi"]));

builder.Services.AddHttpClient<IUserService, UserService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:UserApi"]));

builder.Services.AddHttpClient<IShopService, ShopService>(c =>
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogApi"]));

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
