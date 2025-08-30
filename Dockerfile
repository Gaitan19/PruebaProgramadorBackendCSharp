# Dockerfile para la API REST de PruebaProgramadorBackendCSharp
# Utiliza una imagen base de .NET 8.0 ASP.NET Core

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivo de proyecto y restaurar dependencias
COPY ["PruebaProgramadorBackendCSharp/PruebaProgramadorBackendCSharp.csproj", "PruebaProgramadorBackendCSharp/"]
RUN dotnet restore "PruebaProgramadorBackendCSharp/PruebaProgramadorBackendCSharp.csproj"

# Copiar el código fuente y compilar la aplicación
COPY . .
WORKDIR "/src/PruebaProgramadorBackendCSharp"
RUN dotnet build "PruebaProgramadorBackendCSharp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PruebaProgramadorBackendCSharp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Instalar curl para health checks
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Configurar variables de entorno
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Punto de entrada de la aplicación
ENTRYPOINT ["dotnet", "PruebaProgramadorBackendCSharp.dll"]