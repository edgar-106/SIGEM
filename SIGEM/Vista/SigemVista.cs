using System.ComponentModel;
using SIGEM.Modelo;
using SIGEM.Presentador;

namespace SIGEM.Vista;

public partial class SigemVista : Form, ISigemVista
{
    private readonly SigemPresentador presentador;

    public SigemVista(Usuario usuario)
    {
        InitializeComponent();
        presentador = new SigemPresentador(this, new SigemRepositorioJson(), usuario);

        txtPeso.TextChanged += (_, _) => Recalcular();
        txtEstatura.TextChanged += (_, _) => Recalcular();
        txtPresionSistolica.TextChanged += (_, _) => Recalcular();
        txtPresionDiastolica.TextChanged += (_, _) => Recalcular();
    }

    public event EventHandler? BuscarSolicitado;
    public event EventHandler? GuardarSolicitado;
    public event EventHandler? LimpiarSolicitado;
    public event EventHandler? NuevoPacienteSolicitado;
    public event EventHandler? ValidarSolicitado;
    public event EventHandler? CalcularSolicitado;

    public string Expediente => txtExpediente.Text.Trim();

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PacienteNombre { get => txtPacienteNombre.Text.Trim(); set => txtPacienteNombre.Text = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PacienteApellido { get => txtPacienteApellido.Text.Trim(); set => txtPacienteApellido.Text = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public DateTime PacienteFechaNacimiento { get => dtpFechaNacimiento.Value; set => dtpFechaNacimiento.Value = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PacienteSexo { get => cmbSexo.Text; set => cmbSexo.Text = value; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string PacienteTelefono { get => txtTelefono.Text.Trim(); set => txtTelefono.Text = value; }

    public double Peso => double.TryParse(txtPeso.Text, out var v) ? v : 0;
    public double Estatura => double.TryParse(txtEstatura.Text, out var v) ? v : 0;
    public double Temperatura => double.TryParse(txtTemperatura.Text, out var v) ? v : 0;
    public int Pulso => int.TryParse(txtPulso.Text, out var v) ? v : 0;
    public int FrecuenciaRespiratoria => int.TryParse(txtFrecResp.Text, out var v) ? v : 0;
    public int PresionSistolica => int.TryParse(txtPresionSistolica.Text, out var v) ? v : 0;
    public int PresionDiastolica => int.TryParse(txtPresionDiastolica.Text, out var v) ? v : 0;
    public double CC => double.TryParse(txtCC.Text, out var v) ? v : 0;
    public double SaturacionO2 => double.TryParse(txtSaturacion.Text, out var v) ? v : 0;

    public string UsuarioActual { get; private set; } = string.Empty;
    public RolUsuario RolActual { get; private set; }

    public void SetUsuario(Usuario usuario)
    {
        UsuarioActual = usuario.NombreCompleto;
        RolActual = usuario.Rol;
        lblRolUsuario.Text = $"{usuario.NombreCompleto} ({usuario.Rol})";

        // CAMBIO: Control dinámico de colores y textos según el rol
        if (usuario.Rol == RolUsuario.Doctor)
        {
            MostrarBotonValidar(true);
            btnGuardar.Text = "Guardar y Validar";

            // Colores originales en azul para el Doctor
            pnlEncabezado.BackColor = Color.FromArgb(47, 124, 246);
            btnBuscar.BackColor = Color.FromArgb(47, 124, 246);
        }
        else
        {
            MostrarBotonValidar(false);
            btnGuardar.Text = "Guardar (Borrador)";

            // CAMBIO NUEVO: Cambia la barra de título y el botón Buscar a verde esmeralda para la Enfermera
            pnlEncabezado.BackColor = Color.FromArgb(16, 185, 129);
            btnBuscar.BackColor = Color.FromArgb(16, 185, 129);
        }
    }

    public void MostrarPaciente(Paciente paciente)
    {
        txtPacienteNombre.Text = paciente.Nombre;
        txtPacienteApellido.Text = paciente.Apellido;
        dtpFechaNacimiento.Value = paciente.FechaNacimiento == default ? DateTime.Now.AddYears(-30) : paciente.FechaNacimiento;
        cmbSexo.Text = paciente.Sexo;
        txtTelefono.Text = paciente.Telefono;

        if (paciente.SignosVitales.Count > 0)
        {
            var ultimo = paciente.SignosVitales[^1];
            txtPeso.Text = ultimo.Peso.ToString("F1");
            txtEstatura.Text = ultimo.Estatura.ToString("F2");
            txtTemperatura.Text = ultimo.Temperatura.ToString("F1");
            txtPulso.Text = ultimo.Pulso.ToString();
            txtFrecResp.Text = ultimo.FrecuenciaRespiratoria.ToString();
            txtPresionSistolica.Text = ultimo.PresionSistolica.ToString();
            txtPresionDiastolica.Text = ultimo.PresionDiastolica.ToString();
            txtCC.Text = ultimo.CC.ToString("F1");
            txtSaturacion.Text = ultimo.SaturacionO2.ToString("F0");
            Recalcular();
        }

        HabilitarCamposPaciente(false);
        HabilitarCamposSignosVitales(true);
    }

    public void MostrarCalculos(double imc, double pam)
    {
        lblValorIMC.Text = imc > 0 ? imc.ToString("F2") : "--";
        lblValorPAM.Text = pam > 0 ? pam.ToString("F0") : "--";
    }

    public void MostrarHistorial(IReadOnlyList<string> registros)
    {
        lstHistorial.DataSource = null;
        lstHistorial.DataSource = registros;
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
        txtExpediente.Clear();
        txtPacienteNombre.Clear();
        txtPacienteApellido.Clear();
        dtpFechaNacimiento.Value = DateTime.Now.AddYears(-30);
        cmbSexo.SelectedIndex = -1;
        txtTelefono.Clear();
        txtPeso.Clear();
        txtEstatura.Clear();
        txtTemperatura.Clear();
        txtPulso.Clear();
        txtFrecResp.Clear();
        txtPresionSistolica.Clear();
        txtPresionDiastolica.Clear();
        txtCC.Clear();
        txtSaturacion.Clear();
        lblValorIMC.Text = "--";
        lblValorPAM.Text = "--";
        lblEstado.Text = string.Empty;
        lstHistorial.DataSource = null;

        HabilitarCamposPaciente(false);
        HabilitarCamposSignosVitales(false);
        MostrarBotonValidar(RolActual == RolUsuario.Doctor);
        txtExpediente.Focus();
    }

    public void HabilitarCamposPaciente(bool habilitar)
    {
        txtPacienteNombre.Enabled = habilitar;
        txtPacienteApellido.Enabled = habilitar;
        dtpFechaNacimiento.Enabled = habilitar;
        cmbSexo.Enabled = habilitar;
        txtTelefono.Enabled = habilitar;
    }

    public void HabilitarCamposSignosVitales(bool habilitar)
    {
        txtPeso.Enabled = habilitar;
        txtEstatura.Enabled = habilitar;
        txtTemperatura.Enabled = habilitar;
        txtPulso.Enabled = habilitar;
        txtFrecResp.Enabled = habilitar;
        txtPresionSistolica.Enabled = habilitar;
        txtPresionDiastolica.Enabled = habilitar;
        txtCC.Enabled = habilitar;
        txtSaturacion.Enabled = habilitar;
        btnGuardar.Enabled = habilitar;
    }

    public void MostrarBotonValidar(bool visible)
    {
        btnValidar.Visible = visible;
    }

    public void EstablecerModoNuevoPaciente()
    {
        LimpiarFormulario();
        HabilitarCamposPaciente(true);
        HabilitarCamposSignosVitales(true);
        txtPacienteNombre.Focus();
    }

    private void Recalcular()
    {
        CalcularSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void BtnBuscar_Click(object sender, EventArgs e)
    {
        BuscarSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void BtnNuevoPaciente_Click(object sender, EventArgs e)
    {
        NuevoPacienteSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void BtnGuardar_Click(object sender, EventArgs e)
    {
        GuardarSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void BtnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void BtnValidar_Click(object sender, EventArgs e)
    {
        ValidarSolicitado?.Invoke(this, EventArgs.Empty);
    }

    private void TxtExpediente_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            BuscarSolicitado?.Invoke(this, EventArgs.Empty);
            e.SuppressKeyPress = true;
        }
    }
}