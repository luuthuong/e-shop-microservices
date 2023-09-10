FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "Domain/Domain.csproj"
COPY . .
WORKDIR "/src/Domain"
RUN dotnet build "Domain.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Domain.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Domain.dll"]
