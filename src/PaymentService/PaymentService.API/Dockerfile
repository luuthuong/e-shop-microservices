FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/PaymentService/PaymentService.API/PaymentService.API.csproj", "src/PaymentService/PaymentService.API/"]
COPY ["src/PaymentService/PaymentService.Application/PaymentService.Application.csproj", "src/PaymentService/PaymentService.Application/"]
COPY ["src/Core/Core.csproj", "src/Core/"]
COPY ["src/PaymentService/PaymentService.Domain/PaymentService.Domain.csproj", "src/PaymentService/PaymentService.Domain/"]
COPY ["src/PaymentService/PaymentService.Infrastructure/PaymentService.Infrastructure.csproj", "src/PaymentService/PaymentService.Infrastructure/"]

RUN dotnet restore "src/PaymentService/PaymentService.API/PaymentService.API.csproj"
COPY . .
WORKDIR "src/PaymentService/PaymentService.API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PaymentService.API.dll"]
