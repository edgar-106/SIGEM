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
    private Panel cardPacientes;
    private Panel cardBorradores;
    private Panel cardSignos;
    private Panel cardHoy;
    private Label lblTotalPacientes;
    private Label lblBorradores;
    private Label lblRegistrosVitales;
    private Label lblRegistrosHoy;
    private Label lblCardPacientes;
    private Label lblCardBorradores;
    private Label lblCardSignos;
    private Label lblCardHoy;
    private Panel pnlPacientesRecientes;
    private Panel pnlActividadReciente;
    private Panel pnlBorradores;
    private Label lblPacientesRecientesTitulo;
    private Label lblActividadTitulo;
    private Label lblBorradoresTitulo;
    private ListBox lstPacientesRecientes;
    private ListBox lstActividadReciente;
    private ListBox lstBorradores;

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
        pnlBorradores = new Panel();
        lstBorradores = new ListBox();
        lblBorradoresTitulo = new Label();
        pnlActividadReciente = new Panel();
        lstActividadReciente = new ListBox();
        lblActividadTitulo = new Label();
        pnlPacientesRecientes = new Panel();
        lstPacientesRecientes = new ListBox();
        lblPacientesRecientesTitulo = new Label();
        cardHoy = new Panel();
        lblRegistrosHoy = new Label();
        lblCardHoy = new Label();
        cardSignos = new Panel();
        lblRegistrosVitales = new Label();
        lblCardSignos = new Label();
        cardBorradores = new Panel();
        lblBorradores = new Label();
        lblCardBorradores = new Label();
        cardPacientes = new Panel();
        lblTotalPacientes = new Label();
        lblCardPacientes = new Label();
        lblSubtituloContenido = new Label();
        lblTituloContenido = new Label();
        pnlMenu.SuspendLayout();
        pnlUsuario.SuspendLayout();
        pnlMarca.SuspendLayout();
        pnlContenido.SuspendLayout();
        pnlBorradores.SuspendLayout();
        pnlActividadReciente.SuspendLayout();
        pnlPacientesRecientes.SuspendLayout();
        cardHoy.SuspendLayout();
        cardSignos.SuspendLayout();
        cardBorradores.SuspendLayout();
        cardPacientes.SuspendLayout();
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
        pnlMenu.Margin = new Padding(3, 4, 3, 4);
        pnlMenu.Name = "pnlMenu";
        pnlMenu.Size = new Size(315, 961);
        pnlMenu.TabIndex = 0;
        // 
        // pnlUsuario
        // 
        pnlUsuario.BorderStyle = BorderStyle.FixedSingle;
        pnlUsuario.Controls.Add(btnCerrarSesion);
        pnlUsuario.Controls.Add(lblUsuario);
        pnlUsuario.Dock = DockStyle.Bottom;
        pnlUsuario.Location = new Point(0, 842);
        pnlUsuario.Margin = new Padding(3, 4, 3, 4);
        pnlUsuario.Name = "pnlUsuario";
        pnlUsuario.Size = new Size(315, 119);
        pnlUsuario.TabIndex = 5;
        // 
        // btnCerrarSesion
        // 
        btnCerrarSesion.FlatAppearance.BorderSize = 0;
        btnCerrarSesion.FlatStyle = FlatStyle.Flat;
        btnCerrarSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCerrarSesion.ForeColor = Color.FromArgb(37, 99, 235);
        btnCerrarSesion.Location = new Point(43, 63);
        btnCerrarSesion.Margin = new Padding(3, 4, 3, 4);
        btnCerrarSesion.Name = "btnCerrarSesion";
        btnCerrarSesion.Size = new Size(224, 40);
        btnCerrarSesion.TabIndex = 1;
        btnCerrarSesion.Text = "Cerrar Sesion";
        btnCerrarSesion.UseVisualStyleBackColor = true;
        btnCerrarSesion.Click += BtnCerrarSesion_Click;
        // 
        // lblUsuario
        // 
        lblUsuario.Location = new Point(11, 20);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(290, 27);
        lblUsuario.TabIndex = 0;
        lblUsuario.Text = "Usuario:";
        lblUsuario.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnAdministracion
        // 
        btnAdministracion.FlatAppearance.BorderSize = 0;
        btnAdministracion.FlatStyle = FlatStyle.Flat;
        btnAdministracion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnAdministracion.Location = new Point(23, 421);
        btnAdministracion.Margin = new Padding(3, 4, 3, 4);
        btnAdministracion.Name = "btnAdministracion";
        btnAdministracion.Size = new Size(270, 72);
        btnAdministracion.TabIndex = 4;
        btnAdministracion.Text = "Administracion del Sistema";
        btnAdministracion.UseVisualStyleBackColor = true;
        btnAdministracion.Click += BtnAdministracion_Click;
        // 
        // btnConsulta
        // 
        btnConsulta.FlatAppearance.BorderSize = 0;
        btnConsulta.FlatStyle = FlatStyle.Flat;
        btnConsulta.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnConsulta.Location = new Point(23, 328);
        btnConsulta.Margin = new Padding(3, 4, 3, 4);
        btnConsulta.Name = "btnConsulta";
        btnConsulta.Size = new Size(270, 72);
        btnConsulta.TabIndex = 3;
        btnConsulta.Text = "Consulta Medica";
        btnConsulta.UseVisualStyleBackColor = true;
        btnConsulta.Click += BtnConsulta_Click;
        // 
        // btnPacientes
        // 
        btnPacientes.FlatAppearance.BorderSize = 0;
        btnPacientes.FlatStyle = FlatStyle.Flat;
        btnPacientes.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnPacientes.Location = new Point(23, 235);
        btnPacientes.Margin = new Padding(3, 4, 3, 4);
        btnPacientes.Name = "btnPacientes";
        btnPacientes.Size = new Size(270, 72);
        btnPacientes.TabIndex = 2;
        btnPacientes.Text = "Gestion de Pacientes";
        btnPacientes.UseVisualStyleBackColor = true;
        btnPacientes.Click += BtnPacientes_Click;
        // 
        // btnPanelPrincipal
        // 
        btnPanelPrincipal.FlatAppearance.BorderSize = 0;
        btnPanelPrincipal.FlatStyle = FlatStyle.Flat;
        btnPanelPrincipal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnPanelPrincipal.Location = new Point(23, 141);
        btnPanelPrincipal.Margin = new Padding(3, 4, 3, 4);
        btnPanelPrincipal.Name = "btnPanelPrincipal";
        btnPanelPrincipal.Size = new Size(270, 72);
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
        pnlMarca.Margin = new Padding(3, 4, 3, 4);
        pnlMarca.Name = "pnlMarca";
        pnlMarca.Size = new Size(315, 120);
        pnlMarca.TabIndex = 0;
        // 
        // lblSistema
        // 
        lblSistema.AutoSize = true;
        lblSistema.ForeColor = Color.White;
        lblSistema.Location = new Point(37, 72);
        lblSistema.Name = "lblSistema";
        lblSistema.Size = new Size(189, 20);
        lblSistema.TabIndex = 1;
        lblSistema.Text = "Sistema de Gestion Medica";
        // 
        // lblMarca
        // 
        lblMarca.AutoSize = true;
        lblMarca.Font = new Font("Segoe UI", 20F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(37, 21);
        lblMarca.Name = "lblMarca";
        lblMarca.Size = new Size(125, 46);
        lblMarca.TabIndex = 0;
        lblMarca.Text = "SIGEM";
        // 
        // pnlContenido
        // 
        pnlContenido.AutoScroll = true;
        pnlContenido.BackColor = Color.FromArgb(239, 246, 255);
        pnlContenido.Controls.Add(pnlBorradores);
        pnlContenido.Controls.Add(pnlActividadReciente);
        pnlContenido.Controls.Add(pnlPacientesRecientes);
        pnlContenido.Controls.Add(cardHoy);
        pnlContenido.Controls.Add(cardSignos);
        pnlContenido.Controls.Add(cardBorradores);
        pnlContenido.Controls.Add(cardPacientes);
        pnlContenido.Controls.Add(lblSubtituloContenido);
        pnlContenido.Controls.Add(lblTituloContenido);
        pnlContenido.Dock = DockStyle.Fill;
        pnlContenido.Location = new Point(315, 0);
        pnlContenido.Margin = new Padding(3, 4, 3, 4);
        pnlContenido.Name = "pnlContenido";
        pnlContenido.Padding = new Padding(48, 56, 48, 56);
        pnlContenido.Size = new Size(1152, 961);
        pnlContenido.TabIndex = 1;
        // 
        // pnlBorradores
        // 
        pnlBorradores.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlBorradores.BackColor = Color.White;
        pnlBorradores.BorderStyle = BorderStyle.FixedSingle;
        pnlBorradores.Controls.Add(lstBorradores);
        pnlBorradores.Controls.Add(lblBorradoresTitulo);
        pnlBorradores.Location = new Point(48, 711);
        pnlBorradores.Margin = new Padding(3, 4, 3, 4);
        pnlBorradores.Name = "pnlBorradores";
        pnlBorradores.Size = new Size(1044, 199);
        pnlBorradores.TabIndex = 8;
        // 
        // lstBorradores
        // 
        lstBorradores.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        lstBorradores.BorderStyle = BorderStyle.None;
        lstBorradores.Font = new Font("Segoe UI", 10F);
        lstBorradores.FormattingEnabled = true;
        lstBorradores.Location = new Point(27, 69);
        lstBorradores.Margin = new Padding(3, 4, 3, 4);
        lstBorradores.Name = "lstBorradores";
        lstBorradores.Size = new Size(987, 92);
        lstBorradores.TabIndex = 1;
        // 
        // lblBorradoresTitulo
        // 
        lblBorradoresTitulo.AutoSize = true;
        lblBorradoresTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblBorradoresTitulo.Location = new Point(23, 24);
        lblBorradoresTitulo.Name = "lblBorradoresTitulo";
        lblBorradoresTitulo.Size = new Size(308, 32);
        lblBorradoresTitulo.TabIndex = 0;
        lblBorradoresTitulo.Text = "Borradores de enfermeria";
        // 
        // pnlActividadReciente
        // 
        pnlActividadReciente.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        pnlActividadReciente.BackColor = Color.White;
        pnlActividadReciente.BorderStyle = BorderStyle.FixedSingle;
        pnlActividadReciente.Controls.Add(lstActividadReciente);
        pnlActividadReciente.Controls.Add(lblActividadTitulo);
        pnlActividadReciente.Location = new Point(603, 404);
        pnlActividadReciente.Margin = new Padding(3, 4, 3, 4);
        pnlActividadReciente.Name = "pnlActividadReciente";
        pnlActividadReciente.Size = new Size(489, 269);
        pnlActividadReciente.TabIndex = 7;
        // 
        // lstActividadReciente
        // 
        lstActividadReciente.BorderStyle = BorderStyle.None;
        lstActividadReciente.Font = new Font("Segoe UI", 10F);
        lstActividadReciente.FormattingEnabled = true;
        lstActividadReciente.Location = new Point(25, 77);
        lstActividadReciente.Margin = new Padding(3, 4, 3, 4);
        lstActividadReciente.Name = "lstActividadReciente";
        lstActividadReciente.Size = new Size(434, 138);
        lstActividadReciente.TabIndex = 1;
        // 
        // lblActividadTitulo
        // 
        lblActividadTitulo.AutoSize = true;
        lblActividadTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblActividadTitulo.Location = new Point(23, 27);
        lblActividadTitulo.Name = "lblActividadTitulo";
        lblActividadTitulo.Size = new Size(221, 32);
        lblActividadTitulo.TabIndex = 0;
        lblActividadTitulo.Text = "Actividad reciente";
        // 
        // pnlPacientesRecientes
        // 
        pnlPacientesRecientes.BackColor = Color.White;
        pnlPacientesRecientes.BorderStyle = BorderStyle.FixedSingle;
        pnlPacientesRecientes.Controls.Add(lstPacientesRecientes);
        pnlPacientesRecientes.Controls.Add(lblPacientesRecientesTitulo);
        pnlPacientesRecientes.Location = new Point(48, 404);
        pnlPacientesRecientes.Margin = new Padding(3, 4, 3, 4);
        pnlPacientesRecientes.Name = "pnlPacientesRecientes";
        pnlPacientesRecientes.Size = new Size(512, 269);
        pnlPacientesRecientes.TabIndex = 6;
        // 
        // lstPacientesRecientes
        // 
        lstPacientesRecientes.BorderStyle = BorderStyle.None;
        lstPacientesRecientes.Font = new Font("Segoe UI", 10F);
        lstPacientesRecientes.FormattingEnabled = true;
        lstPacientesRecientes.Location = new Point(27, 77);
        lstPacientesRecientes.Margin = new Padding(3, 4, 3, 4);
        lstPacientesRecientes.Name = "lstPacientesRecientes";
        lstPacientesRecientes.Size = new Size(453, 138);
        lstPacientesRecientes.TabIndex = 1;
        // 
        // lblPacientesRecientesTitulo
        // 
        lblPacientesRecientesTitulo.AutoSize = true;
        lblPacientesRecientesTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblPacientesRecientesTitulo.Location = new Point(23, 27);
        lblPacientesRecientesTitulo.Name = "lblPacientesRecientesTitulo";
        lblPacientesRecientesTitulo.Size = new Size(231, 32);
        lblPacientesRecientesTitulo.TabIndex = 0;
        lblPacientesRecientesTitulo.Text = "Pacientes recientes";
        // 
        // cardHoy
        // 
        cardHoy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        cardHoy.BackColor = Color.White;
        cardHoy.BorderStyle = BorderStyle.FixedSingle;
        cardHoy.Controls.Add(lblRegistrosHoy);
        cardHoy.Controls.Add(lblCardHoy);
        cardHoy.Location = new Point(862, 200);
        cardHoy.Margin = new Padding(3, 4, 3, 4);
        cardHoy.Name = "cardHoy";
        cardHoy.Size = new Size(231, 159);
        cardHoy.TabIndex = 5;
        // 
        // lblRegistrosHoy
        // 
        lblRegistrosHoy.AutoSize = true;
        lblRegistrosHoy.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblRegistrosHoy.Location = new Point(23, 69);
        lblRegistrosHoy.Name = "lblRegistrosHoy";
        lblRegistrosHoy.Size = new Size(46, 54);
        lblRegistrosHoy.TabIndex = 1;
        lblRegistrosHoy.Text = "0";
        // 
        // lblCardHoy
        // 
        lblCardHoy.AutoSize = true;
        lblCardHoy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCardHoy.ForeColor = Color.FromArgb(55, 65, 81);
        lblCardHoy.Location = new Point(23, 29);
        lblCardHoy.Name = "lblCardHoy";
        lblCardHoy.Size = new Size(118, 23);
        lblCardHoy.TabIndex = 0;
        lblCardHoy.Text = "Registros hoy";
        // 
        // cardSignos
        // 
        cardSignos.Anchor = AnchorStyles.Top;
        cardSignos.BackColor = Color.White;
        cardSignos.BorderStyle = BorderStyle.FixedSingle;
        cardSignos.Controls.Add(lblRegistrosVitales);
        cardSignos.Controls.Add(lblCardSignos);
        cardSignos.Location = new Point(590, 200);
        cardSignos.Margin = new Padding(3, 4, 3, 4);
        cardSignos.Name = "cardSignos";
        cardSignos.Size = new Size(231, 159);
        cardSignos.TabIndex = 4;
        // 
        // lblRegistrosVitales
        // 
        lblRegistrosVitales.AutoSize = true;
        lblRegistrosVitales.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblRegistrosVitales.Location = new Point(23, 69);
        lblRegistrosVitales.Name = "lblRegistrosVitales";
        lblRegistrosVitales.Size = new Size(46, 54);
        lblRegistrosVitales.TabIndex = 1;
        lblRegistrosVitales.Text = "0";
        // 
        // lblCardSignos
        // 
        lblCardSignos.AutoSize = true;
        lblCardSignos.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCardSignos.ForeColor = Color.FromArgb(55, 65, 81);
        lblCardSignos.Location = new Point(23, 29);
        lblCardSignos.Name = "lblCardSignos";
        lblCardSignos.Size = new Size(178, 23);
        lblCardSignos.TabIndex = 0;
        lblCardSignos.Text = "Signos vitales totales";
        // 
        // cardBorradores
        // 
        cardBorradores.BackColor = Color.White;
        cardBorradores.BorderStyle = BorderStyle.FixedSingle;
        cardBorradores.Controls.Add(lblBorradores);
        cardBorradores.Controls.Add(lblCardBorradores);
        cardBorradores.Location = new Point(318, 200);
        cardBorradores.Margin = new Padding(3, 4, 3, 4);
        cardBorradores.Name = "cardBorradores";
        cardBorradores.Size = new Size(231, 159);
        cardBorradores.TabIndex = 3;
        // 
        // lblBorradores
        // 
        lblBorradores.AutoSize = true;
        lblBorradores.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblBorradores.Location = new Point(23, 69);
        lblBorradores.Name = "lblBorradores";
        lblBorradores.Size = new Size(46, 54);
        lblBorradores.TabIndex = 1;
        lblBorradores.Text = "0";
        // 
        // lblCardBorradores
        // 
        lblCardBorradores.AutoSize = true;
        lblCardBorradores.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCardBorradores.ForeColor = Color.FromArgb(55, 65, 81);
        lblCardBorradores.Location = new Point(23, 29);
        lblCardBorradores.Name = "lblCardBorradores";
        lblCardBorradores.Size = new Size(191, 23);
        lblCardBorradores.TabIndex = 0;
        lblCardBorradores.Text = "Borradores pendientes";
        // 
        // cardPacientes
        // 
        cardPacientes.BackColor = Color.White;
        cardPacientes.BorderStyle = BorderStyle.FixedSingle;
        cardPacientes.Controls.Add(lblTotalPacientes);
        cardPacientes.Controls.Add(lblCardPacientes);
        cardPacientes.Location = new Point(48, 200);
        cardPacientes.Margin = new Padding(3, 4, 3, 4);
        cardPacientes.Name = "cardPacientes";
        cardPacientes.Size = new Size(231, 159);
        cardPacientes.TabIndex = 2;
        cardPacientes.Paint += cardPacientes_Paint;
        // 
        // lblTotalPacientes
        // 
        lblTotalPacientes.AutoSize = true;
        lblTotalPacientes.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblTotalPacientes.Location = new Point(23, 69);
        lblTotalPacientes.Name = "lblTotalPacientes";
        lblTotalPacientes.Size = new Size(46, 54);
        lblTotalPacientes.TabIndex = 1;
        lblTotalPacientes.Text = "0";
        // 
        // lblCardPacientes
        // 
        lblCardPacientes.AutoSize = true;
        lblCardPacientes.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblCardPacientes.ForeColor = Color.FromArgb(55, 65, 81);
        lblCardPacientes.Location = new Point(23, 29);
        lblCardPacientes.Name = "lblCardPacientes";
        lblCardPacientes.Size = new Size(129, 23);
        lblCardPacientes.TabIndex = 0;
        lblCardPacientes.Text = "Total pacientes";
        lblCardPacientes.Click += lblCardPacientes_Click;
        // 
        // lblSubtituloContenido
        // 
        lblSubtituloContenido.AutoSize = true;
        lblSubtituloContenido.Font = new Font("Segoe UI", 12F);
        lblSubtituloContenido.ForeColor = Color.FromArgb(55, 65, 81);
        lblSubtituloContenido.Location = new Point(48, 125);
        lblSubtituloContenido.Name = "lblSubtituloContenido";
        lblSubtituloContenido.Size = new Size(93, 28);
        lblSubtituloContenido.TabIndex = 1;
        lblSubtituloContenido.Text = "Subtitulo";
        // 
        // lblTituloContenido
        // 
        lblTituloContenido.AutoSize = true;
        lblTituloContenido.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblTituloContenido.Location = new Point(48, 56);
        lblTituloContenido.Name = "lblTituloContenido";
        lblTituloContenido.Size = new Size(132, 54);
        lblTituloContenido.TabIndex = 0;
        lblTituloContenido.Text = "Titulo";
        // 
        // MenuPrincipalVista
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1467, 961);
        Controls.Add(pnlContenido);
        Controls.Add(pnlMenu);
        Margin = new Padding(3, 4, 3, 4);
        MinimumSize = new Size(1255, 918);
        Name = "MenuPrincipalVista";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIGEM - Panel Principal";
        pnlMenu.ResumeLayout(false);
        pnlUsuario.ResumeLayout(false);
        pnlMarca.ResumeLayout(false);
        pnlMarca.PerformLayout();
        pnlContenido.ResumeLayout(false);
        pnlContenido.PerformLayout();
        pnlBorradores.ResumeLayout(false);
        pnlBorradores.PerformLayout();
        pnlActividadReciente.ResumeLayout(false);
        pnlActividadReciente.PerformLayout();
        pnlPacientesRecientes.ResumeLayout(false);
        pnlPacientesRecientes.PerformLayout();
        cardHoy.ResumeLayout(false);
        cardHoy.PerformLayout();
        cardSignos.ResumeLayout(false);
        cardSignos.PerformLayout();
        cardBorradores.ResumeLayout(false);
        cardBorradores.PerformLayout();
        cardPacientes.ResumeLayout(false);
        cardPacientes.PerformLayout();
        ResumeLayout(false);
    }
}
