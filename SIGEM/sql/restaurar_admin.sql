-- Restaura el usuario administrador del sistema SIGEM
-- Ejecutar en la base de datos IMSS (PostgreSQL)

UPDATE usuarios
SET
    rol = 'Administrador',
    contrasena = '123456',
    activo = true,
    id_doctor = NULL
WHERE lower(nombre_usuario) = 'admin';

-- Verificar el resultado (sin mostrar la contraseña)
SELECT id_usuario, nombre_usuario, nombre_completo, rol, activo, id_doctor
FROM usuarios
WHERE lower(nombre_usuario) = 'admin';
