using Npgsql;
using NpgsqlTypes;
using System.Diagnostics;

namespace SIGEM.Modelo;

public class DatosAdministrador
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}

public class ConfiguracionSistema
{
    public bool Notificaciones { get; set; } = true;
    public bool RespaldoAutomatico { get; set; } = true;
    public int TiempoSesionMinutos { get; set; } = 30;
}

public class ServicioAdministracionSistema
{
    public ServicioAdministracionSistema()
    {
        InicializarEstructura();
    }

    private void InicializarEstructura()
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using (var cmd = new NpgsqlCommand(
            "ALTER TABLE usuarios ADD COLUMN IF NOT EXISTS correo VARCHAR(150)", conexion))
        {
            cmd.ExecuteNonQuery();
        }

        using (var cmd = new NpgsqlCommand(
            "ALTER TABLE usuarios ADD COLUMN IF NOT EXISTS telefono VARCHAR(30)", conexion))
        {
            cmd.ExecuteNonQuery();
        }

        using (var cmd = new NpgsqlCommand(
            "CREATE TABLE IF NOT EXISTS configuracion_sistema (" +
            "clave VARCHAR(100) PRIMARY KEY, " +
            "valor TEXT NOT NULL)", conexion))
        {
            cmd.ExecuteNonQuery();
        }
    }

    public DatosAdministrador ObtenerDatosAdministrador(Usuario usuarioActual)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(
            "SELECT nombre_completo, correo, telefono " +
            "FROM usuarios " +
            "WHERE nombre_usuario = @usuario " +
            "LIMIT 1", conexion);

        cmd.Parameters.AddWithValue("@usuario", usuarioActual.NombreUsuario);

        using var reader = cmd.ExecuteReader();

        if (!reader.Read())
        {
            return new DatosAdministrador
            {
                NombreCompleto = usuarioActual.NombreCompleto,
                Correo = "admin@sigem.com",
                Telefono = "+52 55 0000 0000"
            };
        }

        return new DatosAdministrador
        {
            NombreCompleto = LeerTexto(reader, "nombre_completo"),
            Correo = LeerTexto(reader, "correo"),
            Telefono = LeerTexto(reader, "telefono")
        };
    }

    public bool GuardarDatosAdministrador(Usuario usuarioActual, string nombreCompleto, string correo, string telefono)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(
            "UPDATE usuarios " +
            "SET nombre_completo = @nombre, correo = @correo, telefono = @telefono " +
            "WHERE nombre_usuario = @usuario", conexion);

        cmd.Parameters.AddWithValue("@nombre", nombreCompleto.Trim());
        cmd.Parameters.AddWithValue("@correo", correo.Trim());
        cmd.Parameters.AddWithValue("@telefono", telefono.Trim());
        cmd.Parameters.AddWithValue("@usuario", usuarioActual.NombreUsuario);

        return cmd.ExecuteNonQuery() > 0;
    }

    public bool CambiarContrasena(Usuario usuarioActual, string contrasenaActual, string contrasenaNueva)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(
            "UPDATE usuarios " +
            "SET contrasena = @nueva " +
            "WHERE nombre_usuario = @usuario " +
            "AND contrasena = @actual " +
            "AND activo = true", conexion);

        cmd.Parameters.AddWithValue("@nueva", contrasenaNueva);
        cmd.Parameters.AddWithValue("@usuario", usuarioActual.NombreUsuario);
        cmd.Parameters.AddWithValue("@actual", contrasenaActual);

        return cmd.ExecuteNonQuery() > 0;
    }

    public ConfiguracionSistema ObtenerConfiguracion()
    {
        var config = new ConfiguracionSistema();

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(
            "SELECT clave, valor FROM configuracion_sistema", conexion);

        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            string clave = LeerTexto(reader, "clave");
            string valor = LeerTexto(reader, "valor");

            switch (clave)
            {
                case "notificaciones":
                    config.Notificaciones = valor == "true";
                    break;

                case "respaldo_automatico":
                    config.RespaldoAutomatico = valor == "true";
                    break;

                case "tiempo_sesion_minutos":
                    if (int.TryParse(valor, out int minutos))
                        config.TiempoSesionMinutos = minutos;
                    break;
            }
        }

        return config;
    }

    public void GuardarConfiguracion(ConfiguracionSistema config)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        GuardarValorConfiguracion(conexion, "notificaciones", config.Notificaciones ? "true" : "false");
        GuardarValorConfiguracion(conexion, "respaldo_automatico", config.RespaldoAutomatico ? "true" : "false");
        GuardarValorConfiguracion(conexion, "tiempo_sesion_minutos", config.TiempoSesionMinutos.ToString());
    }

    private static void GuardarValorConfiguracion(NpgsqlConnection conexion, string clave, string valor)
    {
        using var cmd = new NpgsqlCommand(
            "INSERT INTO configuracion_sistema (clave, valor) " +
            "VALUES (@clave, @valor) " +
            "ON CONFLICT (clave) DO UPDATE SET valor = EXCLUDED.valor", conexion);

        cmd.Parameters.AddWithValue("@clave", clave);
        cmd.Parameters.AddWithValue("@valor", valor);
        cmd.ExecuteNonQuery();
    }

    public string ObtenerTamanoBaseDatos()
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(
            "SELECT pg_database_size(current_database())", conexion);

        long bytes = Convert.ToInt64(cmd.ExecuteScalar());

        double kb = bytes / 1024.0;
        double mb = kb / 1024.0;

        return mb >= 1 ? $"{mb:F2} MB" : $"{kb:F1} KB";
    }

    public int LimpiarDatosAntiguos(int diasAntiguedad = 365)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(
            "DELETE FROM notas_de_evolucion " +
            "WHERE fecha < CURRENT_DATE - @dias", conexion);

        cmd.Parameters.Add("@dias", NpgsqlDbType.Integer).Value = diasAntiguedad;

        return cmd.ExecuteNonQuery();
    }

    public string CrearRespaldoPostgres()
    {
        string carpetaRespaldos = Path.Combine(AppContext.BaseDirectory, "Datos", "ims", "respaldos");
        Directory.CreateDirectory(carpetaRespaldos);

        string archivo = Path.Combine(
            carpetaRespaldos,
            $"respaldo_IMSS_{DateTime.Now:yyyyMMdd_HHmmss}.sql"
        );

        var builder = new NpgsqlConnectionStringBuilder(ConexionBD.CadenaConexion);

        string host = builder.Host ?? "localhost";
        string database = builder.Database ?? "IMSS";
        string username = builder.Username ?? "postgres";
        string password = builder.Password ?? "";

        var proceso = new ProcessStartInfo
        {
            FileName = "pg_dump",
            Arguments = $"-h {host} -U {username} -d {database} -f \"{archivo}\"",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        proceso.EnvironmentVariables["PGPASSWORD"] = password;

        using var process = Process.Start(proceso);

        if (process == null)
            throw new Exception("No se pudo iniciar pg_dump.");

        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new Exception("No se pudo crear el respaldo. Verifica que pg_dump esté instalado y agregado al PATH.");

        return archivo;
    }

    public void RestaurarRespaldoPostgres(string archivoSql)
    {
        if (!File.Exists(archivoSql))
            throw new FileNotFoundException("No se encontró el archivo de respaldo.");

        var builder = new NpgsqlConnectionStringBuilder(ConexionBD.CadenaConexion);

        string host = builder.Host ?? "localhost";
        string database = builder.Database ?? "IMSS";
        string username = builder.Username ?? "postgres";
        string password = builder.Password ?? "";

        var proceso = new ProcessStartInfo
        {
            FileName = "psql",
            Arguments = $"-h {host} -U {username} -d {database} -f \"{archivoSql}\"",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        proceso.EnvironmentVariables["PGPASSWORD"] = password;

        using var process = Process.Start(proceso);

        if (process == null)
            throw new Exception("No se pudo iniciar psql.");

        process.WaitForExit();

        if (process.ExitCode != 0)
            throw new Exception("No se pudo restaurar el respaldo. Verifica que psql esté instalado y agregado al PATH.");
    }

    private static string LeerTexto(NpgsqlDataReader reader, string columna)
    {
        object valor = reader[columna];
        return valor is null || valor == DBNull.Value ? string.Empty : Convert.ToString(valor)?.Trim() ?? string.Empty;
    }
}