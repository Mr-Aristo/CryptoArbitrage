# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/ArbitrageService/ArbitrageAPI/ArbitrageAPI.csproj", "src/ArbitrageService/ArbitrageAPI/"]
COPY ["src/Messaging/RabbitMQ/RabbitMQ.csproj", "src/Messaging/RabbitMQ/"]
COPY ["src/ArbitrageService/Arbitrage.Infrastructure/Arbitrage.Infrastructure.csproj", "src/ArbitrageService/Arbitrage.Infrastructure/"]
COPY ["src/ArbitrageService/Arbitrage.Application/Arbitrage.Application.csproj", "src/ArbitrageService/Arbitrage.Application/"]
COPY ["src/ArbitrageService/Arbitrage.Domain/Arbitrage.Domain.csproj", "src/ArbitrageService/Arbitrage.Domain/"]
RUN dotnet restore "./src/ArbitrageService/ArbitrageAPI/ArbitrageAPI.csproj"
COPY . .
WORKDIR "/src/src/ArbitrageService/ArbitrageAPI"
RUN dotnet build "./ArbitrageAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ArbitrageAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArbitrageAPI.dll"]