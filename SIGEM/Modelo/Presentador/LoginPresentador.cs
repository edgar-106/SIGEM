using SIGEM.Modelo;
using SIGEM.Vista;

namespace SIGEM.Presentador;

public class LoginPresentador
{
    private readonly ILoginVista vista;
    private readonly ServicioAutenticacion servicioAutenticacion;

    public LoginPresentador(ILoginVista vista, ServicioAutenticacion servicioAutenticacion)
    {
        this.vista = vista;
        this.servicioAutenticacion = servicioAutenticacion;

        this.vista.IniciarSesionSolicitado += IniciarSesion;
    }

    private void IniciarSesion(object? sender, EventArgs e)
    {
        string nombreUsuario = vista.NombreUsuario.Trim();
        string contrasena = vista.Contrasena;

        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
        {
            vista.MostrarError("Ingresa usuario y contraseña.");
            return;
        }

        Usuario? usuario = servicioAutenticacion.Autenticar(nombreUsuario, contrasena);

        if (usuario is null)
        {
            vista.MostrarError("Usuario o contraseña incorrectos.");
            return;
        }

        vista.AbrirMenuPrincipal(usuario);
    }
}
