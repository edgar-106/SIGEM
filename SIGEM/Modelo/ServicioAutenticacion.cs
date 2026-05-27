namespace SIGEM.Modelo;

public class ServicioAutenticacion
{
    private readonly List<Usuario> usuarios =
    [
        new("admin", "admin123", "Dr. Admin", RolUsuario.Doctor),
        new("enfermera", "enfermera123", "Enf. María López", RolUsuario.Enfermera),
    ];

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
        return usuarios.Find(u =>
            string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase)
            && u.Contrasena == contrasena);
    }
}
