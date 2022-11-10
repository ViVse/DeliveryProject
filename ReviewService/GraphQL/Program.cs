using GraphQL.Data;
using GraphQL.GraphQL;
using GraphQL.GraphQL.Reviews;
using GraphQL.GraphQL.Shops;
using GraphQL.GraphQL.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPooledDbContextFactory<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));

builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSubscriptionType<Subscription>()
    .AddType<UserType>()
    .AddType<ShopType>()
    .AddType<ReviewType>()
    .AddFiltering()
    .AddSorting()
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseWebSockets();

app.MapGraphQL();
app.UseGraphQLVoyager("/graphql-voyager");

app.Run();