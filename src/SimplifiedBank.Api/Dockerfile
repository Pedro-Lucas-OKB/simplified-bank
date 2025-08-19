FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimplifiedBank.Api/SimplifiedBank.Api.csproj", "SimplifiedBank.Api/"]
COPY ["SimplifiedBank.Application/SimplifiedBank.Application.csproj", "SimplifiedBank.Application/"]
COPY ["SimplifiedBank.Domain/SimplifiedBank.Domain.csproj", "SimplifiedBank.Domain/"]
COPY ["SimplifiedBank.Infrastructure/SimplifiedBank.Infrastructure.csproj", "SimplifiedBank.Infrastructure/"]
RUN dotnet restore "SimplifiedBank.Api/SimplifiedBank.Api.csproj"
COPY . .

WORKDIR "/src/SimplifiedBank.Api"
RUN dotnet build "./SimplifiedBank.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SimplifiedBank.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimplifiedBank.Api.dll"]
