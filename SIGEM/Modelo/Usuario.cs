namespace SIGEM.Modelo;

public enum RolUsuario
{
    Doctor,
    Enfermera,
<<<<<<< HEAD
=======
    Recepcionista,
>>>>>>> 72ebf2f66146c87d41c2f3c53b88dd92a5dfe847
    Administrador
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
    public int? IdUsuario { get; set; }
    public int? IdDoctor { get; set; }
}
