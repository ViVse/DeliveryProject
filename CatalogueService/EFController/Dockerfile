#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CatalogueService/EFController/EFController.csproj", "CatalogueService/EFController/"]
COPY ["CatalogueService/BLL/BLL.csproj", "CatalogueService/BLL/"]
COPY ["CatalogueService/DAL/DAL.csproj", "CatalogueService/DAL/"]
RUN dotnet restore "CatalogueService/EFController/EFController.csproj"
COPY . .
WORKDIR "/src/CatalogueService/EFController"
RUN dotnet build "EFController.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EFController.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EFController.dll"]