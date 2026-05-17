namespace SIGEM.Modelo;

public enum RolUsuario
{
    Doctor,
    Enfermera


}

public class Usuario
{
    public Usuario(string nombreUsuario, string contrasena, string nombreCompleto, RolUsuario rol)
    {
        NombreUsuario = nombreUsuario;
        Contrasena = contrasena;
        NombreCompleto = nombreCompleto;
        Rol = rol;
    }

    public string NombreUsuario { get; }
    public string Contrasena { get; }
    public string NombreCompleto { get; }
    public RolUsuario Rol { get; }
}
