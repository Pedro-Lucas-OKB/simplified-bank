# Estágio 1: Imagem base para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

# Estágio 2: Compilação do projeto
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# Define a configuração de build (Release por padrão)
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SimplifiedBank.Api/SimplifiedBank.Api.csproj", "SimplifiedBank.Api/"]
COPY ["SimplifiedBank.Application/SimplifiedBank.Application.csproj", "SimplifiedBank.Application/"]
COPY ["SimplifiedBank.Domain/SimplifiedBank.Domain.csproj", "SimplifiedBank.Domain/"]
COPY ["SimplifiedBank.Infrastructure/SimplifiedBank.Infrastructure.csproj", "SimplifiedBank.Infrastructure/"]

# Restaura os pacotes NuGet e copia o código fonte
RUN dotnet restore "SimplifiedBank.Api/SimplifiedBank.Api.csproj"
COPY . .

# Compila o projeto
WORKDIR "/src/SimplifiedBank.Api"
RUN dotnet build "./SimplifiedBank.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Estágio 3: Publicação
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SimplifiedBank.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Estágio 4: Imagem final
FROM base AS final
WORKDIR /app
# Copia os arquivos publicados do estágio de publish
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimplifiedBank.Api.dll"]
