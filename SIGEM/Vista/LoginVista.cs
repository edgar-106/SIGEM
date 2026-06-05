using SIGEM.Modelo;
using SIGEM.Presentador;

namespace SIGEM.Vista;

public partial class LoginVista : Form, ILoginVista
{
    private readonly LoginPresentador presentador;
    private bool mostrarContrasena;

    public LoginVista()
    {
        InitializeComponent();
        presentador = new LoginPresentador(this, new ServicioAutenticacion());
    }

    public event EventHandler? IniciarSesionSolicitado;

    public string NombreUsuario => txtUsuario.Text;

    public string Contrasena => txtContrasena.Text;

    public void MostrarError(string mensaje)
    {
        lblError.Text = mensaje;
        lblError.Visible = true;
    }

    public void AbrirMenuPrincipal(Usuario usuario)
    {
        MenuPrincipalVista menuPrincipal = new(usuario);
        menuPrincipal.FormClosed += (_, _) => Close();
        Hide();
        menuPrincipal.Show();
    }

    private void BtnIniciarSesion_Click(object sender, EventArgs e)
    {
        IniciarSesionSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void TxtContrasena_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            IniciarSesionSolicitado?.Invoke(this, EventArgs.Empty);
            e.SuppressKeyPress = true;
        }
    }

    private void BtnTogglePassword_Click(object? sender, EventArgs e)
    {
        mostrarContrasena = !mostrarContrasena;
        txtContrasena.UseSystemPasswordChar = !mostrarContrasena;
        btnTogglePassword.Text = mostrarContrasena ? "Ocultar" : "Mostrar";
    }

    private void TxtUsuario_Enter(object sender, EventArgs e)
    {
        pnlUsuarioBorder.BackColor = Color.FromArgb(37, 99, 235);
    }

    private void TxtUsuario_Leave(object sender, EventArgs e)
    {
        pnlUsuarioBorder.BackColor = Color.FromArgb(209, 213, 219);
    }

    private void TxtContrasena_Enter(object sender, EventArgs e)
    {
        pnlContrasenaBorder.BackColor = Color.FromArgb(37, 99, 235);
    }

    private void TxtContrasena_Leave(object sender, EventArgs e)
    {
        pnlContrasenaBorder.BackColor = Color.FromArgb(209, 213, 219);
    }

    private void txtUsuario_TextChanged(object sender, EventArgs e)
    {

    }
}
