FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Core/Core/Core.csproj", "Core/Core/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "Core/Core.Infrastructure/"]
COPY ["src/Services/ProductSyncService/ProductSyncService.API/ProductSyncService.API.csproj", "src/Services/ProductSyncService/ProductSyncService.API/"]
COPY ["src/Services/ProductSyncService/ProductSyncService.Application/ProductSyncService.Application.csproj", "src/Services/ProductSyncService/ProductSyncService.Application/"]
COPY ["src/Services/ProductSyncService/ProductSyncService.DTO/ProductSyncService.DTO.csproj", "src/Services/ProductSyncService/ProductSyncService.DTO/"]
COPY ["src/Services/ProductSyncService/ProductSyncService.Domain/ProductSyncService.Domain.csproj", "src/Services/ProductSyncService/ProductSyncService.Domain/"]
COPY ["src/Services/ProductSyncService/ProductSyncService.Infrastructure/ProductSyncService.Infrastructure.csproj", "src/Services/ProductSyncService/ProductSyncService.Infrastructure/"]

RUN dotnet restore "src/Services/ProductSyncService/ProductSyncService.API/ProductSyncService.API.csproj"
COPY . .
WORKDIR "src/Services/ProductSyncService/ProductSyncService.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProductSyncService.API.dll"] 