namespace SIGEM.Vista;

partial class LoginVista
{
    private System.ComponentModel.IContainer components = null;

    private Panel pnlLateral;
    private Label lblMarca;
    private Label lblSubtitulo;
    private Label lblDescripcion;

    private Panel pnlTarjeta;
    private Label lblTitulo;
    private Label lblAcceso;
    private Panel pnlUsuarioBorder;
    private TextBox txtUsuario;
    private Panel pnlContrasenaBorder;
    private TextBox txtContrasena;
    private Button btnTogglePassword;
    private Label lblError;
    private Button btnIniciarSesion;
    private Panel pnlCredenciales;
    private Label lblCredenciales;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlLateral = new Panel();
        lblDescripcion = new Label();
        lblSubtitulo = new Label();
        lblMarca = new Label();
        pnlTarjeta = new Panel();
        lblError = new Label();
        pnlCredenciales = new Panel();
        lblCredenciales = new Label();
        btnTogglePassword = new Button();
        pnlContrasenaBorder = new Panel();
        txtContrasena = new TextBox();
        pnlUsuarioBorder = new Panel();
        txtUsuario = new TextBox();
        lblAcceso = new Label();
        lblTitulo = new Label();
        btnIniciarSesion = new Button();
        pnlLateral.SuspendLayout();
        pnlTarjeta.SuspendLayout();
        pnlCredenciales.SuspendLayout();
        pnlContrasenaBorder.SuspendLayout();
        pnlUsuarioBorder.SuspendLayout();
        SuspendLayout();
        // 
        // pnlLateral
        // 
        pnlLateral.BackColor = Color.FromArgb(30, 58, 138);
        pnlLateral.Controls.Add(lblDescripcion);
        pnlLateral.Controls.Add(lblSubtitulo);
        pnlLateral.Controls.Add(lblMarca);
        pnlLateral.Dock = DockStyle.Left;
        pnlLateral.Location = new Point(0, 0);
        pnlLateral.Margin = new Padding(3, 4, 3, 4);
        pnlLateral.Name = "pnlLateral";
        pnlLateral.Size = new Size(457, 1013);
        pnlLateral.TabIndex = 0;
        // 
        // lblDescripcion
        // 
        lblDescripcion.Font = new Font("Segoe UI", 10F);
        lblDescripcion.ForeColor = Color.FromArgb(147, 197, 253);
        lblDescripcion.Location = new Point(71, 413);
        lblDescripcion.Name = "lblDescripcion";
        lblDescripcion.Size = new Size(320, 107);
        lblDescripcion.TabIndex = 0;
        lblDescripcion.Text = "Plataforma integral para el registro clínico. Gestión de pacientes, signos vitales y consultas médicas.";
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 14F);
        lblSubtitulo.ForeColor = Color.FromArgb(191, 219, 254);
        lblSubtitulo.Location = new Point(71, 357);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(304, 32);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Sistema de Gestión Médica";
        // 
        // lblMarca
        // 
        lblMarca.AutoSize = true;
        lblMarca.Font = new Font("Segoe UI", 36F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(69, 267);
        lblMarca.Name = "lblMarca";
        lblMarca.Size = new Size(220, 81);
        lblMarca.TabIndex = 2;
        lblMarca.Text = "SIGEM";
        // 
        // pnlTarjeta
        // 
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.Controls.Add(lblError);
        pnlTarjeta.Controls.Add(pnlCredenciales);
        pnlTarjeta.Controls.Add(btnIniciarSesion);
        pnlTarjeta.Controls.Add(pnlContrasenaBorder);
        pnlTarjeta.Controls.Add(pnlUsuarioBorder);
        pnlTarjeta.Controls.Add(lblAcceso);
        pnlTarjeta.Controls.Add(lblTitulo);
        pnlTarjeta.Dock = DockStyle.Fill;
        pnlTarjeta.Location = new Point(457, 0);
        pnlTarjeta.Margin = new Padding(3, 4, 3, 4);
        pnlTarjeta.Name = "pnlTarjeta";
        pnlTarjeta.Padding = new Padding(69, 0, 69, 0);
        pnlTarjeta.Size = new Size(719, 1013);
        pnlTarjeta.TabIndex = 1;
        // 
        // lblError
        // 
        lblError.AutoSize = true;
        lblError.Font = new Font("Segoe UI", 9F);
        lblError.ForeColor = Color.FromArgb(220, 38, 38);
        lblError.Location = new Point(71, 533);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 20);
        lblError.TabIndex = 0;
        lblError.Visible = false;
        // 
        // pnlCredenciales
        // 
        pnlCredenciales.BackColor = Color.FromArgb(239, 246, 255);
        pnlCredenciales.BorderStyle = BorderStyle.FixedSingle;
        pnlCredenciales.Controls.Add(lblCredenciales);
        pnlCredenciales.Location = new Point(69, 660);
        pnlCredenciales.Margin = new Padding(3, 4, 3, 4);
        pnlCredenciales.Name = "pnlCredenciales";
        pnlCredenciales.Padding = new Padding(18, 16, 18, 16);
        pnlCredenciales.Size = new Size(581, 193);
        pnlCredenciales.TabIndex = 8;
        // 
        // lblCredenciales
        // 
        lblCredenciales.Dock = DockStyle.Fill;
        lblCredenciales.Font = new Font("Segoe UI", 9F);
        lblCredenciales.ForeColor = Color.FromArgb(55, 65, 81);
        lblCredenciales.Location = new Point(18, 16);
        lblCredenciales.Name = "lblCredenciales";
        lblCredenciales.Size = new Size(543, 159);
        lblCredenciales.TabIndex = 0;
        lblCredenciales.Text = "Credenciales de prueba:\r\n\r\n👨‍⚕️ Doctor      →  Usuario: admin      Contraseña: admin123\r\n👩‍⚕️ Enfermera  →  Usuario: enfermera  Contraseña: enfermera123";
        // 
        // btnIniciarSesion
        // 
        btnIniciarSesion.BackColor = Color.FromArgb(37, 99, 235);
        btnIniciarSesion.Cursor = Cursors.Hand;
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.FlatStyle = FlatStyle.Flat;
        btnIniciarSesion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        btnIniciarSesion.ForeColor = Color.White;
        btnIniciarSesion.Location = new Point(69, 567);
        btnIniciarSesion.Margin = new Padding(3, 4, 3, 4);
        btnIniciarSesion.Name = "btnIniciarSesion";
        btnIniciarSesion.Size = new Size(582, 67);
        btnIniciarSesion.TabIndex = 7;
        btnIniciarSesion.Text = "Iniciar Sesión";
        btnIniciarSesion.UseVisualStyleBackColor = false;
        btnIniciarSesion.Click += BtnIniciarSesion_Click;
        // 
        // btnTogglePassword
        // 
        btnTogglePassword.BackColor = Color.Transparent;
        btnTogglePassword.Cursor = Cursors.Hand;
        btnTogglePassword.FlatAppearance.BorderSize = 0;
        btnTogglePassword.FlatStyle = FlatStyle.Flat;
        btnTogglePassword.Font = new Font("Segoe UI", 9F);
        btnTogglePassword.ForeColor = Color.FromArgb(107, 114, 128);
        btnTogglePassword.Location = new Point(506, -1);
        btnTogglePassword.Margin = new Padding(3, 4, 3, 4);
        btnTogglePassword.Name = "btnTogglePassword";
        btnTogglePassword.Size = new Size(75, 64);
        btnTogglePassword.TabIndex = 6;
        btnTogglePassword.Text = "Mostrar";
        btnTogglePassword.TextAlign = ContentAlignment.MiddleLeft;
        btnTogglePassword.UseVisualStyleBackColor = false;
        btnTogglePassword.Click += BtnTogglePassword_Click;
        // 
        // pnlContrasenaBorder
        // 
        pnlContrasenaBorder.BackColor = Color.FromArgb(209, 213, 219);
        pnlContrasenaBorder.Controls.Add(txtContrasena);
        pnlContrasenaBorder.Controls.Add(btnTogglePassword);
        pnlContrasenaBorder.Location = new Point(69, 453);
        pnlContrasenaBorder.Margin = new Padding(3, 4, 3, 4);
        pnlContrasenaBorder.Name = "pnlContrasenaBorder";
        pnlContrasenaBorder.Padding = new Padding(18, 1, 1, 1);
        pnlContrasenaBorder.Size = new Size(582, 64);
        pnlContrasenaBorder.TabIndex = 5;
        // 
        // txtContrasena
        // 
        txtContrasena.BorderStyle = BorderStyle.None;
        txtContrasena.Font = new Font("Segoe UI", 11F);
        txtContrasena.Location = new Point(18, 17);
        txtContrasena.Margin = new Padding(3, 4, 3, 4);
        txtContrasena.Name = "txtContrasena";
        txtContrasena.PlaceholderText = "🔒  Contraseña";
        txtContrasena.Size = new Size(434, 25);
        txtContrasena.TabIndex = 0;
        txtContrasena.UseSystemPasswordChar = true;
        txtContrasena.Enter += TxtContrasena_Enter;
        txtContrasena.KeyDown += TxtContrasena_KeyDown;
        txtContrasena.Leave += TxtContrasena_Leave;
        // 
        // pnlUsuarioBorder
        // 
        pnlUsuarioBorder.BackColor = Color.FromArgb(209, 213, 219);
        pnlUsuarioBorder.Controls.Add(txtUsuario);
        pnlUsuarioBorder.Location = new Point(69, 363);
        pnlUsuarioBorder.Margin = new Padding(3, 4, 3, 4);
        pnlUsuarioBorder.Name = "pnlUsuarioBorder";
        pnlUsuarioBorder.Padding = new Padding(18, 1, 1, 1);
        pnlUsuarioBorder.Size = new Size(582, 64);
        pnlUsuarioBorder.TabIndex = 4;
        // 
        // txtUsuario
        // 
        txtUsuario.BorderStyle = BorderStyle.None;
        txtUsuario.Font = new Font("Segoe UI", 11F);
        txtUsuario.Location = new Point(18, 17);
        txtUsuario.Margin = new Padding(3, 4, 3, 4);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.PlaceholderText = "👤  Nombre de usuario";
        txtUsuario.Size = new Size(544, 25);
        txtUsuario.TabIndex = 0;
        txtUsuario.Enter += TxtUsuario_Enter;
        txtUsuario.Leave += TxtUsuario_Leave;
        // 
        // lblAcceso
        // 
        lblAcceso.AutoSize = true;
        lblAcceso.Font = new Font("Segoe UI", 10F);
        lblAcceso.ForeColor = Color.FromArgb(107, 114, 128);
        lblAcceso.Location = new Point(71, 304);
        lblAcceso.Name = "lblAcceso";
        lblAcceso.Size = new Size(224, 23);
        lblAcceso.TabIndex = 9;
        lblAcceso.Text = "Accede con tus credenciales";
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.FromArgb(17, 24, 39);
        lblTitulo.Location = new Point(69, 240);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(234, 54);
        lblTitulo.TabIndex = 10;
        lblTitulo.Text = "Bienvenido";
        // 
        // LoginVista
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(243, 244, 246);
        ClientSize = new Size(1176, 1013);
        Controls.Add(pnlTarjeta);
        Controls.Add(pnlLateral);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Margin = new Padding(3, 4, 3, 4);
        MaximizeBox = false;
        Name = "LoginVista";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIGEM - Iniciar Sesión";
        pnlLateral.ResumeLayout(false);
        pnlLateral.PerformLayout();
        pnlTarjeta.ResumeLayout(false);
        pnlTarjeta.PerformLayout();
        pnlCredenciales.ResumeLayout(false);
        pnlContrasenaBorder.ResumeLayout(false);
        pnlContrasenaBorder.PerformLayout();
        pnlUsuarioBorder.ResumeLayout(false);
        pnlUsuarioBorder.PerformLayout();
        ResumeLayout(false);
    }
}
