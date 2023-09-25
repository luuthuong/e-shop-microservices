FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HealthCheck/HealthCheck.csproj", "HealthCheck/"]
RUN dotnet restore "HealthCheck/HealthCheck.csproj"
COPY . .
WORKDIR "/src/HealthCheck"
RUN dotnet build "HealthCheck.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HealthCheck.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HealthCheck.dll"]
