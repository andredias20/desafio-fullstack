FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["WeatherApp.API/WeatherApp.API.csproj", "WeatherApp.API/"]
COPY ["WeatherApp.Application/WeatherApp.Application.csproj", "WeatherApp.Application/"]
COPY ["WeatherApp.Domain/WeatherApp.Domain.csproj", "WeatherApp.Domain/"]
COPY ["WeatherApp.Infrastructure/WeatherApp.Infrastructure.csproj", "WeatherApp.Infrastructure/"]
RUN dotnet restore "WeatherApp.API/WeatherApp.API.csproj"

COPY . .
WORKDIR /src/WeatherApp.API
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "WeatherApp.API.dll"]