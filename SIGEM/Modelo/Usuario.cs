namespace SIGEM.Modelo;

public enum RolUsuario
{
    Doctor,
    Enfermera


}

public class Usuario
{
    public Usuario()
    {
    }

    public Usuario(string nombreUsuario, string contrasena, string nombreCompleto, RolUsuario rol)
    {
        NombreUsuario = nombreUsuario;
        Contrasena = contrasena;
        NombreCompleto = nombreCompleto;
        Rol = rol;
    }

    public string NombreUsuario { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
}
