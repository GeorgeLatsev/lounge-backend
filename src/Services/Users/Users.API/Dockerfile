FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src

COPY "BuildingBlocks/EventBus/EventBus/EventBus.csproj" "BuildingBlocks/EventBus/EventBus/EventBus.csproj"
COPY "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj" "BuildingBlocks/EventBus/EventBusRabbitMQ/EventBusRabbitMQ.csproj"
COPY "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj" "BuildingBlocks/EventBus/IntegrationEventLogEF/IntegrationEventLogEF.csproj"
COPY "BuildingBlocks/HostCustomization/Host.Customization/Host.Customization.csproj" "BuildingBlocks/HostCustomization/Host.Customization/Host.Customization.csproj"

COPY "Services/Notifications/Notifications.API/Notifications.API.csproj" "Services/Notifications/Notifications.API/Notifications.API.csproj"

COPY "Services/Users/Users.Models/Users.Models.csproj" "Services/Users/Users.Models/Users.Models.csproj"
COPY "Services/Users/Users.Infrastructure/Users.Infrastructure.csproj" "Services/Users/Users.Infrastructure/Users.Infrastructure.csproj"
COPY "Services/Users/Users.Services/Users.Services.csproj" "Services/Users/Users.Services/Users.Services.csproj"
COPY "Services/Users/Users.API/Users.API.csproj" "Services/Users/Users.API/Users.API.csproj"

COPY "/Lounge.sln" "Lounge.sln"

RUN dotnet restore "Lounge.sln"

COPY . .
WORKDIR /src/Services/Users/Users.API
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Users.API.dll"]