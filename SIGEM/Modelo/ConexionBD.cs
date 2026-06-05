using Npgsql;

namespace SIGEM.Modelo;

public static class ConexionBD
{
    private static string? cadenaConexion;

    public static string CadenaConexion
    {
        get => cadenaConexion ??= $"Host=localhost;Database=IMSS;Username=postgres;Password=postgres";
        set => cadenaConexion = value;
    }

    public static NpgsqlConnection CrearConexion()
    {
        return new NpgsqlConnection(CadenaConexion);
    }

    public static bool ProbarConexion()
    {
        try
        {
            using var conexion = CrearConexion();
            conexion.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static ISigemRepositorio CrearRepositorio()
    {
        if (ProbarConexion())
        {
            var repo = new SigemRepositorioPostgres();
            if (repo.InicializarBaseDeDatos())
                return repo;
        }

        return new SigemRepositorioJson();
    }
}
