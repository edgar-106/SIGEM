namespace SIGEM.Vista;

partial class MenuPrincipalVista
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlMenu;
    private Panel pnlMarca;
    private Label lblMarca;
    private Label lblSistema;
    private Button btnPanelPrincipal;
    private Button btnPacientes;
    private Button btnConsulta;
    private Button btnAdministracion;
    private Panel pnlUsuario;
    private Label lblUsuario;
    private Button btnCerrarSesion;
    private Panel pnlContenido;
    private Label lblTituloContenido;
    private Label lblSubtituloContenido;
    private Panel pnlTarjetaResumen;
    private Label lblResumen;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlMenu = new Panel();
        pnlUsuario = new Panel();
        btnCerrarSesion = new Button();
        lblUsuario = new Label();
        btnAdministracion = new Button();
        btnConsulta = new Button();
        btnPacientes = new Button();
        btnPanelPrincipal = new Button();
        pnlMarca = new Panel();
        lblSistema = new Label();
        lblMarca = new Label();
        pnlContenido = new Panel();
        pnlTarjetaResumen = new Panel();
        lblResumen = new Label();
        lblSubtituloContenido = new Label();
        lblTituloContenido = new Label();
        pnlMenu.SuspendLayout();
        pnlUsuario.SuspendLayout();
        pnlMarca.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlTarjetaResumen.SuspendLayout();
        SuspendLayout();
        // 
        // pnlMenu
        // 
        pnlMenu.BackColor = Color.White;
        pnlMenu.Controls.Add(pnlUsuario);
        pnlMenu.Controls.Add(btnAdministracion);
        pnlMenu.Controls.Add(btnConsulta);
        pnlMenu.Controls.Add(btnPacientes);
        pnlMenu.Controls.Add(btnPanelPrincipal);
        pnlMenu.Controls.Add(pnlMarca);
        pnlMenu.Dock = DockStyle.Left;
        pnlMenu.Location = new Point(0, 0);
        pnlMenu.Name = "pnlMenu";
        pnlMenu.Size = new Size(260, 681);
        pnlMenu.TabIndex = 0;
        // 
        // pnlUsuario
        // 
        pnlUsuario.BorderStyle = BorderStyle.FixedSingle;
        pnlUsuario.Controls.Add(btnCerrarSesion);
        pnlUsuario.Controls.Add(lblUsuario);
        pnlUsuario.Dock = DockStyle.Bottom;
        pnlUsuario.Location = new Point(0, 591);
        pnlUsuario.Name = "pnlUsuario";
        pnlUsuario.Size = new Size(260, 90);
        pnlUsuario.TabIndex = 5;
        // 
        // btnCerrarSesion
        // 
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCerrarSesion.ForeColor = Color.FromArgb(37, 99, 235);
        btnCerrarSesion.Location = new Point(32, 45);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Size = new Size(196, 30);
        btnCerrarSesion.TabIndex = 1;
        btnCerrarSesion.Text = "Cerrar Sesión";
        btnCerrarSesion.UseVisualStyleBackColor = true;
        btnCerrarSesion.Click += BtnCerrarSesion_Click;
        // 
        // lblUsuario
        // 
        lblUsuario.Location = new Point(10, 15);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(238, 20);
        lblUsuario.TabIndex = 0;
        lblUsuario.Text = "Usuario:";
        lblUsuario.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnAdministracion
        // 
        btnAdministracion.FlatAppearance.BorderSize = 0;
        btnAdministracion.FlatStyle = FlatStyle.Flat;
        btnAdministracion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnAdministracion.Location = new Point(20, 312);
        btnAdministracion.Name = "btnAdministracion";
        btnAdministracion.Size = new Size(220, 52);
        btnAdministracion.TabIndex = 4;
        btnAdministracion.Text = "Administración del Sistema";
        btnAdministracion.UseVisualStyleBackColor = true;
        btnAdministracion.Click += BtnAdministracion_Click;
        // 
        // btnConsulta
        // 
        btnConsulta.FlatAppearance.BorderSize = 0;
        btnConsulta.FlatStyle = FlatStyle.Flat;
        btnConsulta.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnConsulta.Location = new Point(20, 244);
        btnConsulta.Name = "btnConsulta";
        btnConsulta.Size = new Size(220, 52);
        btnConsulta.TabIndex = 3;
        btnConsulta.Text = "Consulta Médica";
        btnConsulta.UseVisualStyleBackColor = true;
        btnConsulta.Click += BtnConsulta_Click;
        // 
        // btnPacientes
        // 
        btnPacientes.FlatAppearance.BorderSize = 0;
        btnPacientes.FlatStyle = FlatStyle.Flat;
        btnPacientes.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnPacientes.Location = new Point(20, 176);
        btnPacientes.Name = "btnPacientes";
        btnPacientes.Size = new Size(220, 52);
        btnPacientes.TabIndex = 2;
        btnPacientes.Text = "Gestión de Pacientes";
        btnPacientes.UseVisualStyleBackColor = true;
        btnPacientes.Click += BtnPacientes_Click;
        // 
        // btnPanelPrincipal
        // 
        btnPanelPrincipal.FlatAppearance.BorderSize = 0;
        btnPanelPrincipal.FlatStyle = FlatStyle.Flat;
        btnPanelPrincipal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnPanelPrincipal.Location = new Point(20, 108);
        btnPanelPrincipal.Name = "btnPanelPrincipal";
        btnPanelPrincipal.Size = new Size(220, 52);
        btnPanelPrincipal.TabIndex = 1;
        btnPanelPrincipal.Text = "Panel Principal";
        btnPanelPrincipal.UseVisualStyleBackColor = true;
        btnPanelPrincipal.Click += BtnPanelPrincipal_Click;
        // 
        // pnlMarca
        // 
        pnlMarca.BackColor = Color.FromArgb(37, 99, 235);
        pnlMarca.Controls.Add(lblSistema);
        pnlMarca.Controls.Add(lblMarca);
        pnlMarca.Dock = DockStyle.Top;
        pnlMarca.Location = new Point(0, 0);
        pnlMarca.Name = "pnlMarca";
        pnlMarca.Size = new Size(260, 90);
        pnlMarca.TabIndex = 0;
        // 
        // lblSistema
        // 
        lblSistema.AutoSize = true;
        lblSistema.ForeColor = Color.White;
        lblSistema.Location = new Point(32, 54);
        lblSistema.Name = "lblSistema";
        lblSistema.Size = new Size(159, 15);
        lblSistema.TabIndex = 1;
        lblSistema.Text = "Sistema de Gestión Médica";
        // 
        // lblMarca
        // 
        lblMarca.AutoSize = true;
        lblMarca.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(32, 16);
        lblMarca.Name = "lblMarca";
        lblMarca.Size = new Size(98, 37);
        lblMarca.TabIndex = 0;
        lblMarca.Text = "SIGEM";
        // 
        // pnlContenido
        // 
        pnlContenido.BackColor = Color.FromArgb(239, 246, 255);
        pnlContenido.Controls.Add(pnlTarjetaResumen);
        pnlContenido.Controls.Add(lblSubtituloContenido);
        pnlContenido.Controls.Add(lblTituloContenido);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(260, 0);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Padding = new Padding(42);
        pnlContenido.Size = new Size(824, 681);
        pnlContenido.TabIndex = 1;
        // 
        // pnlTarjetaResumen
        // 
        pnlTarjetaResumen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlTarjetaResumen.BackColor = Color.White;
        pnlTarjetaResumen.BorderStyle = BorderStyle.FixedSingle;
        pnlTarjetaResumen.Controls.Add(lblResumen);
        pnlTarjetaResumen.Location = new Point(42, 145);
        pnlTarjetaResumen.Name = "pnlTarjetaResumen";
        pnlTarjetaResumen.Size = new Size(740, 210);
        pnlTarjetaResumen.TabIndex = 2;
        // 
        // lblResumen
        // 
        lblResumen.Font = new Font("Segoe UI", 12F);
        lblResumen.Location = new Point(28, 30);
        lblResumen.Name = "lblResumen";
        lblResumen.Size = new Size(670, 145);
        lblResumen.TabIndex = 0;
        lblResumen.Text = "Esta pantalla es la base del menú. Desde aquí puedes agregar paneles, formularios o controles de usuario para cada módulo.";
        // 
        // lblSubtituloContenido
        // 
        lblSubtituloContenido.AutoSize = true;
        lblSubtituloContenido.Font = new Font("Segoe UI", 12F);
        lblSubtituloContenido.ForeColor = Color.FromArgb(55, 65, 81);
        lblSubtituloContenido.Location = new Point(42, 92);
        lblSubtituloContenido.Name = "lblSubtituloContenido";
        lblSubtituloContenido.Size = new Size(74, 21);
        lblSubtituloContenido.TabIndex = 1;
        lblSubtituloContenido.Text = "Subtitulo";
        // 
        // lblTituloContenido
        // 
        lblTituloContenido.AutoSize = true;
        lblTituloContenido.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblTituloContenido.Location = new Point(42, 42);
        lblTituloContenido.Name = "lblTituloContenido";
        lblTituloContenido.Size = new Size(103, 45);
        lblTituloContenido.TabIndex = 0;
        lblTituloContenido.Text = "Titulo";
        // 
        // MenuPrincipalVista
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1084, 681);
        Controls.Add(pnlContenido);
        Controls.Add(pnlMenu);
        MinimumSize = new Size(1000, 650);
        Name = "MenuPrincipalVista";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIGEM - Panel Principal";
        pnlMenu.ResumeLayout(false);
        pnlUsuario.ResumeLayout(false);
        pnlMarca.ResumeLayout(false);
        pnlMarca.PerformLayout();
        pnlContenido.ResumeLayout(false);
        pnlContenido.PerformLayout();
        pnlTarjetaResumen.ResumeLayout(false);
        ResumeLayout(false);
    }
}
