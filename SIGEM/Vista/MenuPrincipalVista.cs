using SIGEM.Modelo;

namespace SIGEM.Vista;

public partial class MenuPrincipalVista : Form
{
    private readonly Usuario usuario;

    public MenuPrincipalVista(Usuario usuario)
    {
        this.usuario = usuario;
        InitializeComponent();
        lblUsuario.Text = $"Usuario: {usuario.NombreCompleto}";
        MostrarPanelPrincipal();
    }

    private void MostrarContenido(string titulo, string subtitulo)
    {
        lblTituloContenido.Text = titulo;
        lblSubtituloContenido.Text = subtitulo;
    }

    private void MostrarPanelPrincipal()
    {
        MostrarContenido("Panel Principal", "Bienvenido al Sistema de Gestión Médica - SIGEM");
        SeleccionarBoton(btnPanelPrincipal);
    }

    private void MostrarGestionPacientes()
    {
        MostrarContenido("Gestión de Pacientes", "Aquí irá el registro, búsqueda, historial y evolución de pacientes.");
        SeleccionarBoton(btnPacientes);
    }

    private void MostrarConsultaMedica()
    {
        MostrarContenido("Consulta Médica", "Aquí irá la captura de consulta, diagnóstico, receta y tratamiento.");
        SeleccionarBoton(btnConsulta);
    }

    private void MostrarAdministracion()
    {
        MostrarContenido("Administración del Sistema", "Aquí irá la configuración del administrador, seguridad y respaldos.");
        SeleccionarBoton(btnAdministracion);
    }

    private void SeleccionarBoton(Button botonActivo)
    {
        Button[] botones = [btnPanelPrincipal, btnPacientes, btnConsulta, btnAdministracion];

        foreach (Button boton in botones)
        {
            boton.BackColor = Color.White;
            boton.ForeColor = Color.FromArgb(31, 41, 55);
        }

        botonActivo.BackColor = Color.FromArgb(47, 124, 246);
        botonActivo.ForeColor = Color.White;
    }

    private void BtnPanelPrincipal_Click(object sender, EventArgs e)
    {
        MostrarPanelPrincipal();
    }

    private void BtnPacientes_Click(object sender, EventArgs e)
    {
        MostrarGestionPacientes();
    }

    private void BtnConsulta_Click(object sender, EventArgs e)
    {
        MostrarConsultaMedica();
    }

    private void BtnAdministracion_Click(object sender, EventArgs e)
    {
        MostrarAdministracion();
    }

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
    {
        LoginVista login = new();
        login.Show();
        Close();
    }
}
