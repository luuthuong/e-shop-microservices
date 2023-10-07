FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/PaymentService/API/API.csproj", "src/PaymentService/API/"]
COPY ["src/PaymentService/Application/Application.csproj", "src/PaymentService/Application/"]
COPY ["src/Core/Core.csproj", "src/Core/"]
COPY ["src/PaymentService/Domain/Domain.csproj", "src/PaymentService/Domain/"]
COPY ["src/PaymentService/Infrastructure/Infrastructure.csproj", "src/PaymentService/Infrastructure/"]

RUN dotnet restore "src/PaymentService/API/API.csproj"
COPY . .
WORKDIR "src/PaymentService/API"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
