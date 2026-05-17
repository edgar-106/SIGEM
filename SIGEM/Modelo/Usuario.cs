namespace SIGEM.Modelo;

public class Usuario
{
    public Usuario(string nombreUsuario, string contrasena, string nombreCompleto)
    {
        NombreUsuario = nombreUsuario;
        Contrasena = contrasena;
        NombreCompleto = nombreCompleto;
    }

    public string NombreUsuario { get; }

    public string Contrasena { get; }

    public string NombreCompleto { get; }
}
