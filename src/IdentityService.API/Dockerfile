FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/IdentityService.API/IdentityService.API.csproj", "src/IdentityService.API/"]
RUN dotnet restore "src/IdentityService.API/IdentityService.API.csproj"
COPY . .
WORKDIR "/src/src/IdentityService.API"
RUN dotnet build "IdentityService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IdentityService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityService.API.dll"]
