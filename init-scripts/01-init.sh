#!/bin/bash
# Script de inicialización para PostgreSQL
# Configura la base de datos para la aplicación PruebaProgramadorBackendCSharp

set -e

# Crear la base de datos si no existe
psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "$POSTGRES_DB" <<-EOSQL
    -- La base de datos ya se crea automáticamente por la variable POSTGRES_DB
    -- Este script puede extenderse para configuraciones adicionales
    
    -- Crear extensiones si son necesarias
    -- CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
    
    -- Otorgar permisos adicionales si son necesarios
    GRANT ALL PRIVILEGES ON DATABASE prueba_db TO postgres;
    
    -- Mostrar mensaje de confirmación
    SELECT 'Base de datos prueba_db inicializada correctamente' as status;
EOSQL

echo "Inicialización de la base de datos completada"