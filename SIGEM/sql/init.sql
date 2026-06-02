-- ============================================================
-- Script de inicialización — Base de Datos IMSS (PostgreSQL)
-- ============================================================

CREATE DATABASE IMSS;

\c IMSS;

-- Tabla de pacientes
CREATE TABLE IF NOT EXISTS pacientes (
    id_paciente SERIAL PRIMARY KEY,
    curp VARCHAR(20) UNIQUE,
    nombre VARCHAR(100),
    apellido_paterno VARCHAR(100),
    apellido_materno VARCHAR(100),
    sexo VARCHAR(10),
    genero VARCHAR(20)
);

-- Tabla de doctores
CREATE TABLE IF NOT EXISTS doctores (
    id_doctor SERIAL PRIMARY KEY,
    cedula_profesional VARCHAR(50) UNIQUE NOT NULL,
    nombre VARCHAR(100),
    apellido_paterno VARCHAR(100),
    apellido_materno VARCHAR(100),
    usuario VARCHAR(50),
    contrasena VARCHAR(100)
);

-- Tabla de historiales medicos
CREATE TABLE IF NOT EXISTS historiales_medicos (
    codigo_historial SERIAL PRIMARY KEY,
    id_paciente INTEGER NOT NULL REFERENCES pacientes(id_paciente) ON DELETE CASCADE,
    fecha DATE DEFAULT CURRENT_DATE NOT NULL
);

-- Tabla de notas de evolucion
CREATE TABLE IF NOT EXISTS notas_de_evolucion (
    numero_expediente SERIAL PRIMARY KEY,
    codigo_historial INTEGER NOT NULL REFERENCES historiales_medicos(codigo_historial) ON DELETE CASCADE,
    id_doctor INTEGER REFERENCES doctores(id_doctor),
    fecha DATE DEFAULT CURRENT_DATE NOT NULL,
    hora TIME DEFAULT CURRENT_TIME NOT NULL,
    presion_arterial VARCHAR(20),
    frecuencia_respiratoria VARCHAR(10),
    nota_medica TEXT,
    peso DECIMAL(5,2),
    temperatura DECIMAL(4,1),
    estatura DECIMAL(4,2),
    pulso VARCHAR(10),
    cc DECIMAL(5,1),
    saturacion_oxigeno DECIMAL(4,1)
);

-- Tabla de usuarios del sistema
CREATE TABLE IF NOT EXISTS usuarios (
    id_usuario SERIAL PRIMARY KEY,
    nombre_usuario VARCHAR(50) UNIQUE NOT NULL,
    contrasena VARCHAR(100) NOT NULL,
    nombre_completo VARCHAR(200) NOT NULL,
    rol VARCHAR(30) NOT NULL,
    activo BOOLEAN DEFAULT true NOT NULL,
    id_doctor INTEGER REFERENCES doctores(id_doctor)
);

-- Indices
CREATE INDEX IF NOT EXISTS idx_historiales_paciente ON historiales_medicos(id_paciente);
CREATE INDEX IF NOT EXISTS idx_notas_historial ON notas_de_evolucion(codigo_historial);
CREATE INDEX IF NOT EXISTS idx_usuarios_doctor ON usuarios(id_doctor);

-- Usuarios por defecto
INSERT INTO usuarios (nombre_usuario, contrasena, nombre_completo, rol, activo)
SELECT 'doctor', 'doctor123', 'Doctor SIGEM', 'Doctor', true
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre_usuario = 'doctor');

INSERT INTO usuarios (nombre_usuario, contrasena, nombre_completo, rol, activo)
SELECT 'enfermera', 'enfermera123', 'Enfermera SIGEM', 'Enfermera', true
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre_usuario = 'enfermera');

INSERT INTO usuarios (nombre_usuario, contrasena, nombre_completo, rol, activo)
SELECT 'recepcion', 'recepcion123', 'Recepcionista SIGEM', 'Recepcionista', true
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre_usuario = 'recepcion');

INSERT INTO usuarios (nombre_usuario, contrasena, nombre_completo, rol, activo)
SELECT 'admin', 'admin123', 'Administrador SIGEM', 'Administrador', true
WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre_usuario = 'admin');
