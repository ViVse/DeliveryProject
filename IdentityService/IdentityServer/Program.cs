using AutoMapper;
using IdentityServer;
using IdentityServer.Data;
using IdentityServer.Mapper;
using IdentityServer.Services;
using IdentityServer4.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var seed = args.Contains("/seed");
if(seed)
{
    args = args.Except(new[] { "/seed" }).ToArray();
}

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly.GetName().Name;
var defaultConnString = builder.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection");

seed = true;
if (seed)
{
    SeedData.EnsureSeedData(defaultConnString);
}

builder.Services.AddDbContext<AspNetIdentityDbContext>(options =>
    options.UseSqlServer(defaultConnString, b => b.MigrationsAssembly("IdentityServer")));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddIdentity<UserModel, IdentityRole>()
    .AddUserManager<UserManager<UserModel>>()
    .AddSignInManager<SignInManager<UserModel>>()
    .AddEntityFrameworkStores<AspNetIdentityDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<UserModel>()
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b =>
            b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly("IdentityServer"));
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b =>
           b.UseSqlServer(defaultConnString, opt => opt.MigrationsAssembly("IdentityServer"));
    })
    .AddDeveloperSigningCredential()
    .AddProfileService<ProfileService<UserModel>>();

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


app.UseIdentityServer();

app.MapControllers();

app.Run();
