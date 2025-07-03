FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

RUN mkdir -p /app/https
COPY TelegramBot.Api/Certificates/aspnetapp.pfx /app/https/aspnetapp.pfx

EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["TelegramBot.Api/TelegramBot.Api.csproj", "TelegramBot.Api/"]
COPY ["TelegramBot.Application/TelegramBot.Application.csproj", "TelegramBot.Application/"]
COPY ["TelegramBot.Domain/TelegramBot.Domain.csproj", "TelegramBot.Domain/"]
COPY ["TelegramBot.Infrastructure/TelegramBot.Infrastructure.csproj", "TelegramBot.Infrastructure/"]
COPY ["TelegramBot.SharedKernel/TelegramBot.SharedKernel.csproj", "TelegramBot.SharedKernel/"]

RUN dotnet restore "TelegramBot.Api/TelegramBot.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TelegramBot.Api/TelegramBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TelegramBot.Api/TelegramBot.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development \
    ASPNETCORE_URLS=https://+:8081 \
    ASPNETCORE_Kestrel__Certificates__Default__Path=/app/https/aspnetapp.pfx

ENTRYPOINT ["dotnet", "TelegramBot.Api.dll"]