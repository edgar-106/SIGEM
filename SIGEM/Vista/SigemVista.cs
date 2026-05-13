using SIGEM.Modelo;
using SIGEM.Presentador;

namespace SIGEM.Vista;

public partial class SigemVista : Form, ISigemVista
{
    private readonly SigemPresentador presentador;

    public SigemVista()
    {
        InitializeComponent();
        presentador = new SigemPresentador(this, new SigemRepositorioTxt());
    }

    public event EventHandler? GuardarSigemSolicitado;

    public string NombreSigem => txtNombre.Text;

    public void MostrarRegistros(IReadOnlyList<string> nombres)
    {
        lstRegistros.DataSource = null;
        lstRegistros.DataSource = nombres;
    }

    public void MostrarMensaje(string mensaje)
    {
        lblEstado.ForeColor = Color.FromArgb(38, 120, 79);
        lblEstado.Text = mensaje;
    }

    public void MostrarError(string mensaje)
    {
        lblEstado.ForeColor = Color.FromArgb(184, 54, 54);
        lblEstado.Text = mensaje;
    }

    public void LimpiarFormulario()
    {
        txtNombre.Clear();
        txtNombre.Focus();
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        GuardarSigemSolicitado?.Invoke(this, EventArgs.Empty);
    }
}
