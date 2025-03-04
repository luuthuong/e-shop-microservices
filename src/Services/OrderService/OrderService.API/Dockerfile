ENV DOTNET_VERSION=8.0

FROM mcr.microsoft.com/dotnet/aspnet:$DOTNET_VERSION AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:$DOTNET_VERSION AS build

WORKDIR /src

COPY ["src/Core/Core/Core.csproj", "src/Core/Core/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "src/Core/Core.Infrastructure/"]
COPY ["src/OrderService/OrderService.API/OrderService.API.csproj", "src/OrderService/OrderService.API/"]
COPY ["src/OrderService/OrderService.Application/OrderService.Application.csproj", "src/OrderService/OrderService.Application/"]
COPY ["src/OrderService/OrderService.Domain/OrderService.Domain.csproj", "src/OrderService/OrderService.Domain/"]
COPY ["src/OrderService/OrderService.Infrastructure/OrderService.Infrastructure.csproj", "src/OrderService/OrderService.Infrastructure/"]

RUN dotnet restore "src/OrderService/OrderService.API/OrderService.API.csproj"
COPY . .
WORKDIR src/OrderService/OrderService.API
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderService.API.dll"]
