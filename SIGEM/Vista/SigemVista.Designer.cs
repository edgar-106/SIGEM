namespace SIGEM.Vista;

partial class SigemVista
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlEncabezado;
    private Label lblTitulo;
    private Label lblRolUsuario;

    // Sección búsqueda
    private Label lblExpediente;
    private TextBox txtExpediente;
    private Button btnBuscar;
    private Button btnNuevoPaciente;

    // Sección datos del paciente
    private GroupBox grpPaciente;
    private Label lblPacienteNombre;
    private TextBox txtPacienteNombre;
    private Label lblPacienteApellido;
    private TextBox txtPacienteApellido;
    private Label lblFechaNac;
    private DateTimePicker dtpFechaNacimiento;
    private Label lblSexo;
    private ComboBox cmbSexo;
    private Label lblTelefono;
    private TextBox txtTelefono;

    // Sección signos vitales
    private GroupBox grpSignosVitales;
    private Label lblPeso;
    private TextBox txtPeso;
    private Label lblEstatura;
    private TextBox txtEstatura;
    private Label lblTemperatura;
    private TextBox txtTemperatura;
    private Label lblPulso;
    private TextBox txtPulso;
    private Label lblFrecResp;
    private TextBox txtFrecResp;
    private Label lblCC;
    private TextBox txtCC;
    private Label lblSaturacion;
    private TextBox txtSaturacion;
    private Label lblPresion;
    private TextBox txtPresionSistolica;
    private Label lblPresionSep;
    private TextBox txtPresionDiastolica;
    private Label lblmmHg;

    // Sección cálculos
    private GroupBox grpCalculos;
    private Label lblIMC;
    private Label lblValorIMC;
    private Label lblPAM;
    private Label lblValorPAM;

    // Botones acción
    private Button btnGuardar;
    private Button btnLimpiar;
    private Button btnValidar;

    // Historial
    private Label lblHistorial;
    private ListBox lstHistorial;

    // Estado
    private Label lblEstado;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlEncabezado = new Panel();
        lblTitulo = new Label();
        lblRolUsuario = new Label();
        lblExpediente = new Label();
        txtExpediente = new TextBox();
        btnBuscar = new Button();
        btnNuevoPaciente = new Button();
        grpPaciente = new GroupBox();
        txtPacienteNombre = new TextBox();
        lblPacienteNombre = new Label();
        txtPacienteApellido = new TextBox();
        lblPacienteApellido = new Label();
        dtpFechaNacimiento = new DateTimePicker();
        lblFechaNac = new Label();
        cmbSexo = new ComboBox();
        lblSexo = new Label();
        txtTelefono = new TextBox();
        lblTelefono = new Label();
        grpSignosVitales = new GroupBox();
        txtPeso = new TextBox();
        lblPeso = new Label();
        txtEstatura = new TextBox();
        lblEstatura = new Label();
        txtTemperatura = new TextBox();
        lblTemperatura = new Label();
        txtPulso = new TextBox();
        lblPulso = new Label();
        txtFrecResp = new TextBox();
        lblFrecResp = new Label();
        txtCC = new TextBox();
        lblCC = new Label();
        txtSaturacion = new TextBox();
        lblSaturacion = new Label();
        txtPresionSistolica = new TextBox();
        lblPresion = new Label();
        lblPresionSep = new Label();
        txtPresionDiastolica = new TextBox();
        lblmmHg = new Label();
        grpCalculos = new GroupBox();
        lblIMC = new Label();
        lblValorIMC = new Label();
        lblPAM = new Label();
        lblValorPAM = new Label();
        btnGuardar = new Button();
        btnLimpiar = new Button();
        btnValidar = new Button();
        lblHistorial = new Label();
        lstHistorial = new ListBox();
        lblEstado = new Label();
        pnlEncabezado.SuspendLayout();
        grpPaciente.SuspendLayout();
        grpSignosVitales.SuspendLayout();
        grpCalculos.SuspendLayout();
        SuspendLayout();

        // pnlEncabezado
        pnlEncabezado.BackColor = Color.FromArgb(37, 99, 235);
        pnlEncabezado.Controls.Add(lblTitulo);
        pnlEncabezado.Controls.Add(lblRolUsuario);
        pnlEncabezado.Dock = DockStyle.Top;
        pnlEncabezado.Location = new Point(0, 0);
        pnlEncabezado.Name = "pnlEncabezado";
        pnlEncabezado.Size = new Size(880, 64);
        pnlEncabezado.TabIndex = 0;

        // lblTitulo
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(24, 14);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(374, 32);
        lblTitulo.Text = "Registro de Signos Vitales";

        // lblRolUsuario
        lblRolUsuario.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblRolUsuario.AutoSize = true;
        lblRolUsuario.Font = new Font("Segoe UI", 10F);
        lblRolUsuario.ForeColor = Color.White;
        lblRolUsuario.Location = new Point(680, 22);
        lblRolUsuario.Name = "lblRolUsuario";
        lblRolUsuario.Size = new Size(80, 19);
        lblRolUsuario.Text = "Rol: Usuario";

        // lblExpediente
        lblExpediente.AutoSize = true;
        lblExpediente.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblExpediente.Location = new Point(24, 90);
        lblExpediente.Name = "lblExpediente";
        lblExpediente.Size = new Size(113, 19);
        lblExpediente.Text = "No. Expediente:";

        // txtExpediente
        txtExpediente.Font = new Font("Segoe UI", 10F);
        txtExpediente.Location = new Point(143, 86);
        txtExpediente.Name = "txtExpediente";
        txtExpediente.PlaceholderText = "Ingrese número de expediente";
        txtExpediente.Size = new Size(200, 25);
        txtExpediente.TabIndex = 0;
        txtExpediente.KeyDown += TxtExpediente_KeyDown;

        // btnBuscar
        btnBuscar.BackColor = Color.FromArgb(47, 124, 246);
        btnBuscar.FlatAppearance.BorderSize = 0;
        btnBuscar.FlatStyle = FlatStyle.Flat;
        btnBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnBuscar.ForeColor = Color.White;
        btnBuscar.Location = new Point(352, 85);
        btnBuscar.Name = "btnBuscar";
        btnBuscar.Size = new Size(90, 28);
        btnBuscar.TabIndex = 1;
        btnBuscar.Text = "Buscar";
        btnBuscar.UseVisualStyleBackColor = false;
        btnBuscar.Click += BtnBuscar_Click;

        // btnNuevoPaciente
        btnNuevoPaciente.BackColor = Color.FromArgb(16, 185, 129);
        btnNuevoPaciente.FlatAppearance.BorderSize = 0;
        btnNuevoPaciente.FlatStyle = FlatStyle.Flat;
        btnNuevoPaciente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnNuevoPaciente.ForeColor = Color.White;
        btnNuevoPaciente.Location = new Point(450, 85);
        btnNuevoPaciente.Name = "btnNuevoPaciente";
        btnNuevoPaciente.Size = new Size(120, 28);
        btnNuevoPaciente.TabIndex = 2;
        btnNuevoPaciente.Text = "Nuevo Paciente";
        btnNuevoPaciente.UseVisualStyleBackColor = false;
        btnNuevoPaciente.Click += BtnNuevoPaciente_Click;

        // grpPaciente
        grpPaciente.BackColor = Color.White;
        grpPaciente.Controls.Add(txtPacienteNombre);
        grpPaciente.Controls.Add(lblPacienteNombre);
        grpPaciente.Controls.Add(txtPacienteApellido);
        grpPaciente.Controls.Add(lblPacienteApellido);
        grpPaciente.Controls.Add(dtpFechaNacimiento);
        grpPaciente.Controls.Add(lblFechaNac);
        grpPaciente.Controls.Add(cmbSexo);
        grpPaciente.Controls.Add(lblSexo);
        grpPaciente.Controls.Add(txtTelefono);
        grpPaciente.Controls.Add(lblTelefono);
        grpPaciente.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpPaciente.Location = new Point(24, 128);
        grpPaciente.Name = "grpPaciente";
        grpPaciente.Size = new Size(832, 112);
        grpPaciente.TabIndex = 3;
        grpPaciente.TabStop = false;
        grpPaciente.Text = "Datos del Paciente";

        // txtPacienteNombre
        txtPacienteNombre.Font = new Font("Segoe UI", 9F);
        txtPacienteNombre.Location = new Point(70, 30);
        txtPacienteNombre.Name = "txtPacienteNombre";
        txtPacienteNombre.PlaceholderText = "Nombre";
        txtPacienteNombre.Size = new Size(180, 23);
        txtPacienteNombre.TabIndex = 0;
        txtPacienteNombre.Enabled = false;

        // lblPacienteNombre
        lblPacienteNombre.AutoSize = true;
        lblPacienteNombre.Font = new Font("Segoe UI", 9F);
        lblPacienteNombre.Location = new Point(12, 33);
        lblPacienteNombre.Name = "lblPacienteNombre";
        lblPacienteNombre.Size = new Size(54, 15);
        lblPacienteNombre.Text = "Nombre:";

        // txtPacienteApellido
        txtPacienteApellido.Font = new Font("Segoe UI", 9F);
        txtPacienteApellido.Location = new Point(340, 30);
        txtPacienteApellido.Name = "txtPacienteApellido";
        txtPacienteApellido.PlaceholderText = "Apellido";
        txtPacienteApellido.Size = new Size(200, 23);
        txtPacienteApellido.TabIndex = 1;
        txtPacienteApellido.Enabled = false;

        // lblPacienteApellido
        lblPacienteApellido.AutoSize = true;
        lblPacienteApellido.Font = new Font("Segoe UI", 9F);
        lblPacienteApellido.Location = new Point(278, 33);
        lblPacienteApellido.Name = "lblPacienteApellido";
        lblPacienteApellido.Size = new Size(54, 15);
        lblPacienteApellido.Text = "Apellido:";

        // dtpFechaNacimiento
        dtpFechaNacimiento.Font = new Font("Segoe UI", 9F);
        dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
        dtpFechaNacimiento.Location = new Point(70, 68);
        dtpFechaNacimiento.Name = "dtpFechaNacimiento";
        dtpFechaNacimiento.Size = new Size(130, 23);
        dtpFechaNacimiento.TabIndex = 2;
        dtpFechaNacimiento.Enabled = false;

        // lblFechaNac
        lblFechaNac.AutoSize = true;
        lblFechaNac.Font = new Font("Segoe UI", 9F);
        lblFechaNac.Location = new Point(12, 72);
        lblFechaNac.Name = "lblFechaNac";
        lblFechaNac.Size = new Size(52, 15);
        lblFechaNac.Text = "Fec. Nac:";

        // cmbSexo
        cmbSexo.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSexo.Font = new Font("Segoe UI", 9F);
        cmbSexo.Items.AddRange(["Masculino", "Femenino"]);
        cmbSexo.Location = new Point(340, 68);
        cmbSexo.Name = "cmbSexo";
        cmbSexo.Size = new Size(130, 23);
        cmbSexo.TabIndex = 3;
        cmbSexo.Enabled = false;

        // lblSexo
        lblSexo.AutoSize = true;
        lblSexo.Font = new Font("Segoe UI", 9F);
        lblSexo.Location = new Point(278, 71);
        lblSexo.Name = "lblSexo";
        lblSexo.Size = new Size(35, 15);
        lblSexo.Text = "Sexo:";

        // txtTelefono
        txtTelefono.Font = new Font("Segoe UI", 9F);
        txtTelefono.Location = new Point(570, 68);
        txtTelefono.Name = "txtTelefono";
        txtTelefono.PlaceholderText = "Teléfono";
        txtTelefono.Size = new Size(180, 23);
        txtTelefono.TabIndex = 4;
        txtTelefono.Enabled = false;

        // lblTelefono
        lblTelefono.AutoSize = true;
        lblTelefono.Font = new Font("Segoe UI", 9F);
        lblTelefono.Location = new Point(508, 71);
        lblTelefono.Name = "lblTelefono";
        lblTelefono.Size = new Size(55, 15);
        lblTelefono.Text = "Teléfono:";

        // grpSignosVitales
        grpSignosVitales.BackColor = Color.White;
        grpSignosVitales.Controls.Add(txtPeso);
        grpSignosVitales.Controls.Add(lblPeso);
        grpSignosVitales.Controls.Add(txtEstatura);
        grpSignosVitales.Controls.Add(lblEstatura);
        grpSignosVitales.Controls.Add(txtTemperatura);
        grpSignosVitales.Controls.Add(lblTemperatura);
        grpSignosVitales.Controls.Add(txtPulso);
        grpSignosVitales.Controls.Add(lblPulso);
        grpSignosVitales.Controls.Add(txtFrecResp);
        grpSignosVitales.Controls.Add(lblFrecResp);
        grpSignosVitales.Controls.Add(txtCC);
        grpSignosVitales.Controls.Add(lblCC);
        grpSignosVitales.Controls.Add(txtSaturacion);
        grpSignosVitales.Controls.Add(lblSaturacion);
        grpSignosVitales.Controls.Add(txtPresionSistolica);
        grpSignosVitales.Controls.Add(lblPresion);
        grpSignosVitales.Controls.Add(lblPresionSep);
        grpSignosVitales.Controls.Add(txtPresionDiastolica);
        grpSignosVitales.Controls.Add(lblmmHg);
        grpSignosVitales.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpSignosVitales.Location = new Point(24, 252);
        grpSignosVitales.Name = "grpSignosVitales";
        grpSignosVitales.Size = new Size(832, 172);
        grpSignosVitales.TabIndex = 4;
        grpSignosVitales.TabStop = false;
        grpSignosVitales.Text = "Signos Vitales";

        // Fila 1: Peso | Estatura | Temperatura | Pulso
        lblPeso.AutoSize = true;
        lblPeso.Font = new Font("Segoe UI", 9F);
        lblPeso.Location = new Point(12, 30);
        lblPeso.Name = "lblPeso";
        lblPeso.Size = new Size(80, 15);
        lblPeso.Text = "Peso (kg):";

        txtPeso.Font = new Font("Segoe UI", 9F);
        txtPeso.Location = new Point(95, 27);
        txtPeso.Name = "txtPeso";
        txtPeso.PlaceholderText = "0.0";
        txtPeso.Size = new Size(100, 23);
        txtPeso.TabIndex = 0;
        txtPeso.Enabled = false;

        lblEstatura.AutoSize = true;
        lblEstatura.Font = new Font("Segoe UI", 9F);
        lblEstatura.Location = new Point(210, 30);
        lblEstatura.Name = "lblEstatura";
        lblEstatura.Size = new Size(87, 15);
        lblEstatura.Text = "Estatura (m):";

        txtEstatura.Font = new Font("Segoe UI", 9F);
        txtEstatura.Location = new Point(302, 27);
        txtEstatura.Name = "txtEstatura";
        txtEstatura.PlaceholderText = "0.00";
        txtEstatura.Size = new Size(100, 23);
        txtEstatura.TabIndex = 1;
        txtEstatura.Enabled = false;

        lblTemperatura.AutoSize = true;
        lblTemperatura.Font = new Font("Segoe UI", 9F);
        lblTemperatura.Location = new Point(420, 30);
        lblTemperatura.Name = "lblTemperatura";
        lblTemperatura.Size = new Size(109, 15);
        lblTemperatura.Text = "Temperatura (°C):";

        txtTemperatura.Font = new Font("Segoe UI", 9F);
        txtTemperatura.Location = new Point(535, 27);
        txtTemperatura.Name = "txtTemperatura";
        txtTemperatura.PlaceholderText = "36.5";
        txtTemperatura.Size = new Size(100, 23);
        txtTemperatura.TabIndex = 2;
        txtTemperatura.Enabled = false;

        lblPulso.AutoSize = true;
        lblPulso.Font = new Font("Segoe UI", 9F);
        lblPulso.Location = new Point(650, 30);
        lblPulso.Name = "lblPulso";
        lblPulso.Size = new Size(60, 15);
        lblPulso.Text = "Pulso/FC:";

        txtPulso.Font = new Font("Segoe UI", 9F);
        txtPulso.Location = new Point(716, 27);
        txtPulso.Name = "txtPulso";
        txtPulso.PlaceholderText = "lpm";
        txtPulso.Size = new Size(100, 23);
        txtPulso.TabIndex = 3;
        txtPulso.Enabled = false;

        // Fila 2: Frec.Resp | CC | Sat O2 | Presión Arterial
        lblFrecResp.AutoSize = true;
        lblFrecResp.Font = new Font("Segoe UI", 9F);
        lblFrecResp.Location = new Point(12, 66);
        lblFrecResp.Name = "lblFrecResp";
        lblFrecResp.Size = new Size(77, 15);
        lblFrecResp.Text = "Frec. Resp. (rpm):";

        txtFrecResp.Font = new Font("Segoe UI", 9F);
        txtFrecResp.Location = new Point(95, 63);
        txtFrecResp.Name = "txtFrecResp";
        txtFrecResp.PlaceholderText = "0";
        txtFrecResp.Size = new Size(100, 23);
        txtFrecResp.TabIndex = 4;
        txtFrecResp.Enabled = false;

        lblCC.AutoSize = true;
        lblCC.Font = new Font("Segoe UI", 9F);
        lblCC.Location = new Point(210, 66);
        lblCC.Name = "lblCC";
        lblCC.Size = new Size(52, 15);
        lblCC.Text = "CC (cm):";

        txtCC.Font = new Font("Segoe UI", 9F);
        txtCC.Location = new Point(268, 63);
        txtCC.Name = "txtCC";
        txtCC.PlaceholderText = "0.0";
        txtCC.Size = new Size(100, 23);
        txtCC.TabIndex = 5;
        txtCC.Enabled = false;

        lblSaturacion.AutoSize = true;
        lblSaturacion.Font = new Font("Segoe UI", 9F);
        lblSaturacion.Location = new Point(420, 66);
        lblSaturacion.Name = "lblSaturacion";
        lblSaturacion.Size = new Size(103, 15);
        lblSaturacion.Text = "Saturación O₂ (%):";

        txtSaturacion.Font = new Font("Segoe UI", 9F);
        txtSaturacion.Location = new Point(529, 63);
        txtSaturacion.Name = "txtSaturacion";
        txtSaturacion.PlaceholderText = "98";
        txtSaturacion.Size = new Size(100, 23);
        txtSaturacion.TabIndex = 6;
        txtSaturacion.Enabled = false;

        // Presión Arterial
        lblPresion.AutoSize = true;
        lblPresion.Font = new Font("Segoe UI", 9F);
        lblPresion.Location = new Point(650, 66);
        lblPresion.Name = "lblPresion";
        lblPresion.Size = new Size(92, 15);
        lblPresion.Text = "Presión Arterial:";

        txtPresionSistolica.Font = new Font("Segoe UI", 9F);
        txtPresionSistolica.Location = new Point(660, 87);
        txtPresionSistolica.Name = "txtPresionSistolica";
        txtPresionSistolica.PlaceholderText = "120";
        txtPresionSistolica.Size = new Size(55, 23);
        txtPresionSistolica.TabIndex = 7;
        txtPresionSistolica.Enabled = false;

        lblPresionSep.AutoSize = true;
        lblPresionSep.Font = new Font("Segoe UI", 12F);
        lblPresionSep.Location = new Point(718, 86);
        lblPresionSep.Name = "lblPresionSep";
        lblPresionSep.Size = new Size(15, 21);
        lblPresionSep.Text = "/";

        txtPresionDiastolica.Font = new Font("Segoe UI", 9F);
        txtPresionDiastolica.Location = new Point(736, 87);
        txtPresionDiastolica.Name = "txtPresionDiastolica";
        txtPresionDiastolica.PlaceholderText = "80";
        txtPresionDiastolica.Size = new Size(55, 23);
        txtPresionDiastolica.TabIndex = 8;
        txtPresionDiastolica.Enabled = false;

        lblmmHg.AutoSize = true;
        lblmmHg.Font = new Font("Segoe UI", 9F);
        lblmmHg.Location = new Point(795, 90);
        lblmmHg.Name = "lblmmHg";
        lblmmHg.Size = new Size(35, 15);
        lblmmHg.Text = "mmHg";

        // Fila 3: labels adicionales para presión arterial
        var lblPAS = new Label();
        lblPAS.AutoSize = true;
        lblPAS.Font = new Font("Segoe UI", 8F);
        lblPAS.Location = new Point(660, 112);
        lblPAS.Name = "lblPAS";
        lblPAS.Size = new Size(55, 13);
        lblPAS.Text = "Sistólica";
        grpSignosVitales.Controls.Add(lblPAS);

        var lblPAD = new Label();
        lblPAD.AutoSize = true;
        lblPAD.Font = new Font("Segoe UI", 8F);
        lblPAD.Location = new Point(736, 112);
        lblPAD.Name = "lblPAD";
        lblPAD.Size = new Size(55, 13);
        lblPAD.Text = "Diastólica";
        grpSignosVitales.Controls.Add(lblPAD);

        // Fila 3: espacio adicional para Presión en la fila de arriba
        // Movemos etiqueta Presión hacia arriba al lado de Sat O2
        lblPresion.Location = new Point(650, 66);

        // Ajustamos posiciones de los campos de presión
        txtPresionSistolica.Location = new Point(660, 87);
        lblPresionSep.Location = new Point(718, 86);
        txtPresionDiastolica.Location = new Point(736, 87);
        lblmmHg.Location = new Point(795, 90);

        // grpCalculos
        grpCalculos.BackColor = Color.White;
        grpCalculos.Controls.Add(lblIMC);
        grpCalculos.Controls.Add(lblValorIMC);
        grpCalculos.Controls.Add(lblPAM);
        grpCalculos.Controls.Add(lblValorPAM);
        grpCalculos.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        grpCalculos.Location = new Point(24, 436);
        grpCalculos.Name = "grpCalculos";
        grpCalculos.Size = new Size(832, 56);
        grpCalculos.TabIndex = 5;
        grpCalculos.TabStop = false;
        grpCalculos.Text = "Cálculos Automáticos";

        lblIMC.AutoSize = true;
        lblIMC.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblIMC.Location = new Point(120, 22);
        lblIMC.Name = "lblIMC";
        lblIMC.Size = new Size(40, 20);
        lblIMC.Text = "IMC:";

        lblValorIMC.AutoSize = true;
        lblValorIMC.Font = new Font("Segoe UI", 11F);
        lblValorIMC.ForeColor = Color.FromArgb(37, 99, 235);
        lblValorIMC.Location = new Point(166, 22);
        lblValorIMC.Name = "lblValorIMC";
        lblValorIMC.Size = new Size(20, 20);
        lblValorIMC.Text = "--";

        lblPAM.AutoSize = true;
        lblPAM.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblPAM.Location = new Point(400, 22);
        lblPAM.Name = "lblPAM";
        lblPAM.Size = new Size(41, 20);
        lblPAM.Text = "PAM:";

        lblValorPAM.AutoSize = true;
        lblValorPAM.Font = new Font("Segoe UI", 11F);
        lblValorPAM.ForeColor = Color.FromArgb(37, 99, 235);
        lblValorPAM.Location = new Point(447, 22);
        lblValorPAM.Name = "lblValorPAM";
        lblValorPAM.Size = new Size(20, 20);
        lblValorPAM.Text = "--";

        // btnGuardar
        btnGuardar.BackColor = Color.FromArgb(47, 124, 246);
        btnGuardar.FlatAppearance.BorderSize = 0;
        btnGuardar.FlatStyle = FlatStyle.Flat;
        btnGuardar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnGuardar.ForeColor = Color.White;
        btnGuardar.Location = new Point(24, 504);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(160, 40);
        btnGuardar.TabIndex = 6;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = false;
        btnGuardar.Enabled = false;
        btnGuardar.Click += BtnGuardar_Click;

        // btnLimpiar
        btnLimpiar.BackColor = Color.FromArgb(107, 114, 128);
        btnLimpiar.FlatAppearance.BorderSize = 0;
        btnLimpiar.FlatStyle = FlatStyle.Flat;
        btnLimpiar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnLimpiar.ForeColor = Color.White;
        btnLimpiar.Location = new Point(200, 504);
        btnLimpiar.Name = "btnLimpiar";
        btnLimpiar.Size = new Size(130, 40);
        btnLimpiar.TabIndex = 7;
        btnLimpiar.Text = "Limpiar";
        btnLimpiar.UseVisualStyleBackColor = false;
        btnLimpiar.Click += BtnLimpiar_Click;

        // btnValidar
        btnValidar.BackColor = Color.FromArgb(16, 185, 129);
        btnValidar.FlatAppearance.BorderSize = 0;
        btnValidar.FlatStyle = FlatStyle.Flat;
        btnValidar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnValidar.ForeColor = Color.White;
        btnValidar.Location = new Point(350, 504);
        btnValidar.Name = "btnValidar";
        btnValidar.Size = new Size(160, 40);
        btnValidar.TabIndex = 8;
        btnValidar.Text = "Validar Registro";
        btnValidar.UseVisualStyleBackColor = false;
        btnValidar.Visible = false;
        btnValidar.Click += BtnValidar_Click;

        // lblHistorial
        lblHistorial.AutoSize = true;
        lblHistorial.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblHistorial.Location = new Point(24, 560);
        lblHistorial.Name = "lblHistorial";
        lblHistorial.Size = new Size(166, 19);
        lblHistorial.Text = "Historial de Registros";

        // lstHistorial
        lstHistorial.Font = new Font("Segoe UI", 9F);
        lstHistorial.FormattingEnabled = true;
        lstHistorial.ItemHeight = 15;
        lstHistorial.Location = new Point(24, 585);
        lstHistorial.Name = "lstHistorial";
        lstHistorial.Size = new Size(832, 94);
        lstHistorial.TabIndex = 9;

        // lblEstado
        lblEstado.AutoSize = true;
        lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEstado.Location = new Point(24, 690);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(0, 15);
        lblEstado.Text = string.Empty;

        // SigemVista
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(239, 246, 255);
        ClientSize = new Size(880, 720);
        Controls.Add(lblEstado);
        Controls.Add(lstHistorial);
        Controls.Add(lblHistorial);
        Controls.Add(btnValidar);
        Controls.Add(btnLimpiar);
        Controls.Add(btnGuardar);
        Controls.Add(grpCalculos);
        Controls.Add(grpSignosVitales);
        Controls.Add(grpPaciente);
        Controls.Add(btnNuevoPaciente);
        Controls.Add(btnBuscar);
        Controls.Add(txtExpediente);
        Controls.Add(lblExpediente);
        Controls.Add(pnlEncabezado);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimumSize = new Size(896, 759);
        Name = "SigemVista";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIGEM - Signos Vitales";
        pnlEncabezado.ResumeLayout(false);
        pnlEncabezado.PerformLayout();
        grpPaciente.ResumeLayout(false);
        grpPaciente.PerformLayout();
        grpSignosVitales.ResumeLayout(false);
        grpSignosVitales.PerformLayout();
        grpCalculos.ResumeLayout(false);
        grpCalculos.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
