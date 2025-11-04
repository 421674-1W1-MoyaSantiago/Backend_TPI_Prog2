-- PostgreSQL script para Auth-API
-- Este script se ejecutará automáticamente en Railway

-- Crear tabla Users
CREATE TABLE IF NOT EXISTS Users (
    Id SERIAL PRIMARY KEY,
    Username VARCHAR(50) NOT NULL,
    PasswordHash VARCHAR(255) NOT NULL,
    Email VARCHAR(100),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UpdatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    IsActive BOOLEAN DEFAULT true,
    DeletedAt TIMESTAMP NULL
);

-- Crear índice único para Username
CREATE UNIQUE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username);

-- Crear tabla para logs de autenticación
CREATE TABLE IF NOT EXISTS AuthLogs (
    Id BIGSERIAL PRIMARY KEY,
    UserId INTEGER REFERENCES Users(Id),
    Action VARCHAR(50) NOT NULL,
    IpAddress VARCHAR(45),
    UserAgent VARCHAR(500),
    Success BOOLEAN NOT NULL DEFAULT false,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Crear función para actualizar UpdatedAt automáticamente
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.UpdatedAt = CURRENT_TIMESTAMP;
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Crear trigger para actualizar UpdatedAt
DROP TRIGGER IF EXISTS update_users_updated_at ON Users;
CREATE TRIGGER update_users_updated_at
    BEFORE UPDATE ON Users
    FOR EACH ROW
    EXECUTE FUNCTION update_updated_at_column();

-- Insertar usuario de prueba (opcional)
INSERT INTO Users (Username, PasswordHash, Email)
VALUES ('admin', '$2a$11$example.hash.for.testing', 'admin@example.com')
ON CONFLICT (Username) DO NOTHING;

-- Comentarios informativos
COMMENT ON TABLE Users IS 'Tabla de usuarios para autenticación';
COMMENT ON TABLE AuthLogs IS 'Logs de eventos de autenticación';