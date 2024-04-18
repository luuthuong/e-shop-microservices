FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Core/Core/Core.csproj", "src/Core/Core/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "src/Core/Core.Infrastructure/"]
COPY ["src/ProductSyncService/ProductSyncService.API/ProductSyncService.API.csproj", "src/ProductSyncService/ProductSyncService.API/"]
COPY ["src/ProductSyncService/ProductSyncService.Application/ProductSyncService.Application.csproj", "src/ProductSyncService/ProductSyncService.Application/"]
COPY ["src/ProductSyncService/ProductSyncService.DTO/ProductSyncService.DTO.csproj", "src/ProductSyncService/ProductSyncService.DTO/"]
COPY ["src/ProductSyncService/ProductSyncService.Domain/ProductSyncService.Domain.csproj", "src/ProductSyncService/ProductSyncService.Domain/"]
COPY ["src/ProductSyncService/ProductSyncService.Infrastructure/ProductSyncService.Infrastructure.csproj", "src/ProductSyncService/ProductSyncService.Infrastructure/"]

RUN dotnet restore "src/ProductSyncService/ProductSyncService.API/ProductSyncService.API.csproj"
COPY . .
WORKDIR "/src/src/ProductSyncService/ProductSyncService.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductSyncService.API.dll"] 