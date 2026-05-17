namespace SIGEM.Modelo;

public class ServicioAutenticacion
{
    private readonly Usuario usuarioAdmin = new("admin", "admin123", "Dr. Admin");

    public Usuario? Autenticar(string nombreUsuario, string contrasena)
    {
        bool credencialesCorrectas =
            string.Equals(nombreUsuario, usuarioAdmin.NombreUsuario, StringComparison.OrdinalIgnoreCase)
            && contrasena == usuarioAdmin.Contrasena;

        return credencialesCorrectas ? usuarioAdmin : null;
    }
}
