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
        pnlContenido.Controls.Clear();
        pnlContenido.Controls.Add(pnlTarjetaResumen);
        pnlContenido.Controls.Add(lblSubtituloContenido);
        pnlContenido.Controls.Add(lblTituloContenido);
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
        // Cambiar título y botón activo
        lblTituloContenido.Text = "";
        lblSubtituloContenido.Text = "";
        SeleccionarBoton(btnAdministracion);

        // Limpiar contenido anterior y cargar el control
        pnlContenido.Controls.Clear();
        var adminControl = new AdministracionControl();
        pnlContenido.Controls.Add(adminControl);
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

    private void pnlMarca_Paint(object sender, PaintEventArgs e)
    {

    }
}
