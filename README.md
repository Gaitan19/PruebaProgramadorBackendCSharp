# PruebaProgramadorBackendCSharp

API REST desarrollada en .NET 8 para gestión de marcas de automóviles.

## Características

- **API REST** con endpoints CRUD para marcas de automóviles
- **Entity Framework Core** con PostgreSQL
- **Pruebas unitarias** con XUnit (>70% cobertura)
- **Docker Compose** para fácil despliegue


## Estructura del Proyecto

```
PruebaProgramadorBackendCSharp/
├── Controllers/           # Controladores de la API
├── Models/               # Entidades del dominio
├── DTOs/                 # Data Transfer Objects
├── Services/             # Lógica de negocio
├── Repositories/         # Acceso a datos
├── Data/                 # Contexto de Entity Framework
├── Migrations/           # Migraciones de base de datos
└── Tests/                # Pruebas unitarias e integración
```

## Endpoints de la API

| Método | Endpoint | Descripción |
|--------|----------|-------------|
| GET | `/api/MarcasAutos` | Obtener todas las marcas |
| GET | `/api/MarcasAutos/{id}` | Obtener marca por ID |
| POST | `/api/MarcasAutos` | Crear nueva marca |
| PUT | `/api/MarcasAutos/{id}` | Actualizar marca existente |
| DELETE | `/api/MarcasAutos/{id}` | Eliminar marca |

## Ejecución con Docker Compose

### Prerequisitos
- Docker
- Docker Compose

### Instrucciones

1. **Clonar el repositorio:**
   ```bash
   git clone <repository-url>
   cd PruebaProgramadorBackendCSharp
   ```

2. **Ejecutar con Docker Compose:**
   ```bash
   docker-compose up -d
   ```

3. **Verificar que los servicios estén ejecutándose:**
   ```bash
   docker-compose ps
   ```

4. **Acceder a la API:**
   - URL: `http://localhost:8080`
   - Swagger UI: `http://localhost:8080/swagger`

5. **Verificar la base de datos:**
   - Host: `localhost`
   - Puerto: `5432`
   - Base de datos: `prueba_db`
   - Usuario: `postgres`
   - Contraseña: `19062003kk`

### Servicios Docker

El archivo `docker-compose.yml` configura dos servicios:

1. **postgres-db**: Base de datos PostgreSQL
   - Puerto: 5432
   - Volumen persistente para datos
   - Healthcheck configurado

2. **api-rest**: API REST de .NET 8
   - Puerto: 8080
   - Conecta automáticamente a PostgreSQL
   - Healthcheck configurado

## Desarrollo Local

### Prerequisitos
- .NET 8 SDK
- PostgreSQL (local o Docker)

### Configuración

1. **Actualizar connection string en `appsettings.Development.json`:**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=prueba_db;Username=postgres;Password=your_password"
     }
   }
   ```

2. **Aplicar migraciones:**
   ```bash
   cd PruebaProgramadorBackendCSharp
   dotnet ef database update
   ```

3. **Ejecutar la aplicación:**
   ```bash
   dotnet run
   ```

## Pruebas

### Ejecutar todas las pruebas:
```bash
cd PruebaProgramadorBackendCSharp.Tests
dotnet test
```

### Ejecutar pruebas con cobertura:
```bash
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=lcov
```

```bash
dotnet test ../PruebaProgramadorBackendCSharp.Tests /p:CollectCoverage=true /p:CoverletOutputFormat=lcov '/p:Exclude="[*]Program,[*]*.Migrations.*"'
```

### Tipos de pruebas incluidas:

1. **Pruebas unitarias del Repositorio** - Verifican operaciones CRUD en memoria
2. **Pruebas unitarias del Servicio** - Verifican lógica de negocio con mocks
3. **Pruebas unitarias del Controlador** - Verifican comportamiento de endpoints


## Comandos Útiles Docker

```bash
# Iniciar servicios
docker-compose up -d

# Ver logs
docker-compose logs -f

# Ver logs de un servicio específico
docker-compose logs -f api-rest
docker-compose logs -f postgres-db

# Detener servicios
docker-compose down

# Detener servicios y eliminar volúmenes
docker-compose down -v

# Reconstruir imágenes
docker-compose build --no-cache

# Ejecutar comando en contenedor
docker-compose exec api-rest bash
docker-compose exec postgres-db psql -U postgres -d prueba_db
```

## Monitoreo y Health Checks

Ambos servicios tienen configurados health checks:

- **PostgreSQL**: Verifica conectividad con `pg_isready`
- **API**: Verifica endpoint `/api/MarcasAutos`

## Arquitectura

La aplicación sigue el patrón de arquitectura en capas:

1. **Presentación** (Controllers) - Manejo de requests HTTP
2. **Aplicación** (Services) - Lógica de negocio
3. **Dominio** (Models, DTOs) - Entidades del negocio
4. **Infraestructura** (Repositories, Data) - Acceso a datos

