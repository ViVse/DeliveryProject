#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["OrderService/CleanController/Ordering.API.csproj", "OrderService/CleanController/"]
COPY ["BuildingBlocks/EventBus.Messages/EventBus.Messages.csproj", "BuildingBlocks/EventBus.Messages/"]
COPY ["OrderService/Infrastructure/Infrastructure.csproj", "OrderService/Infrastructure/"]
COPY ["OrderService/Application/Application.csproj", "OrderService/Application/"]
COPY ["OrderService/Domain/Domain.csproj", "OrderService/Domain/"]
RUN dotnet restore "OrderService/CleanController/Ordering.API.csproj"
COPY . .
WORKDIR "/src/OrderService/CleanController"
RUN dotnet build "Ordering.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.API.dll"]