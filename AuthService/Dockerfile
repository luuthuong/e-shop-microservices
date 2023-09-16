FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["AppIdentity/AppIdentity.csproj", "AppIdentity/"]
RUN dotnet restore "AppIdentity/AppIdentity.csproj"
COPY . .
WORKDIR "/src/AppIdentity"
RUN dotnet build "AppIdentity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppIdentity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppIdentity.dll"]
