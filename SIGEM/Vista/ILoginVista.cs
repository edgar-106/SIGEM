using SIGEM.Modelo;

namespace SIGEM.Vista;

public interface ILoginVista
{
    event EventHandler? IniciarSesionSolicitado;

    string NombreUsuario { get; }

    string Contrasena { get; }

    void MostrarError(string mensaje);

    void AbrirMenuPrincipal(Usuario usuario);
}
