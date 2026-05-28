using System.Text.Json;

namespace SIGEM.Modelo;

public class ServicioAutenticacion
{
    private readonly string rutaUsuarios;
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

    public ServicioAutenticacion()
    {
        string carpetaIms = Path.Combine(AppContext.BaseDirectory, "Datos", "ims");
        Directory.CreateDirectory(carpetaIms);
        rutaUsuarios = Path.Combine(carpetaIms, "usuarios.json");
        InicializarUsuarios();
    }

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
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
            new("doctor", "doctor123", "Dr. Admin", RolUsuario.Doctor),
            new("enfermera", "enfermera123", "Enf. Maria Lopez", RolUsuario.Enfermera),
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
