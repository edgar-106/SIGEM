using System.Text.Json;

namespace SIGEM.Modelo;

public class ServicioAutenticacion
{
    private readonly string rutaUsuarios;
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };
    private readonly ISigemRepositorio? repositorio;

    public ServicioAutenticacion() : this(null)
    {
    }

    public ServicioAutenticacion(ISigemRepositorio? repositorio)
    {
        this.repositorio = repositorio;
        string carpetaIms = Path.Combine(AppContext.BaseDirectory, "Datos", "ims");
        Directory.CreateDirectory(carpetaIms);
        rutaUsuarios = Path.Combine(carpetaIms, "usuarios.json");
        InicializarUsuarios();
    }

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
        if (repositorio is not null)
        {
            try
            {
                var usuario = repositorio.AutenticarUsuario(nombreUsuario, contrasena);
                if (usuario is not null)
                    return usuario;
            }
            catch
            {
            }
        }

        List<Usuario> usuarios = CargarUsuarios();

        return usuarios.Find(u =>
            string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase)
            && u.Contrasena == contrasena);
    }

    private void InicializarUsuarios()
    {
        if (File.Exists(rutaUsuarios))
            return;

        List<Usuario> usuarios =
        [
            new("doctor", "doctor123", "Doctor SIGEM", RolUsuario.Doctor),
            new("enfermera", "enfermera123", "Enfermera SIGEM", RolUsuario.Enfermera),
            new("recepcion", "recepcion123", "Recepcionista SIGEM", RolUsuario.Recepcionista),
            new("admin", "admin123", "Administrador SIGEM", RolUsuario.Administrador),
        ];

        GuardarUsuarios(usuarios);
    }

    private List<Usuario> CargarUsuarios()
    {
        if (!File.Exists(rutaUsuarios))
        {
            InicializarUsuarios();
        }

        string json = File.ReadAllText(rutaUsuarios);
        return JsonSerializer.Deserialize<List<Usuario>>(json) ?? [];
    }

    private void GuardarUsuarios(List<Usuario> usuarios)
    {
        string json = JsonSerializer.Serialize(usuarios, jsonOptions);
        File.WriteAllText(rutaUsuarios, json);
    }
}