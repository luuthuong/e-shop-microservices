FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Core/Core/Core.csproj", "Core/Core/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "Core/Core.Infrastructure/"]

COPY ["src/Services/CustomerService/CustomerService.API/CustomerService.API.csproj", "src/Services/CustomerService/CustomerService.API/"]
COPY ["src/Services/CustomerService/CustomerService.Application/CustomerService.Application.csproj", "src/Services/CustomerService/CustomerService.Application/"]
COPY ["src/Services/CustomerService/CustomerService.DTO/CustomerService.DTO.csproj", "src/Services/CustomerService/CustomerService.DTO/"]
COPY ["src/Services/CustomerService/CustomerService.Domain/CustomerService.Domain.csproj", "src/Services/CustomerService/CustomerService.Domain/"]
COPY ["src/Services/CustomerService/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj", "src/Services/CustomerService/CustomerService.Infrastructure/"]

RUN dotnet restore "src/Services/CustomerService/CustomerService.API/CustomerService.API.csproj"
COPY . .
WORKDIR "src/Services/CustomerService/CustomerService.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.API.dll"]
