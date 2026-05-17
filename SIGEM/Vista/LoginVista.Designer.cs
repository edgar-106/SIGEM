namespace SIGEM.Vista;

partial class LoginVista
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlTarjeta;
    private Label lblMarca;
    private Label lblSubtitulo;
    private Label lblTitulo;
    private Label lblUsuario;
    private TextBox txtUsuario;
    private Label lblContrasena;
    private TextBox txtContrasena;
    private Button btnIniciarSesion;
    private Label lblCredenciales;
    private Label lblError;

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
        pnlTarjeta = new Panel();
        lblError = new Label();
        lblCredenciales = new Label();
        btnIniciarSesion = new Button();
        txtContrasena = new TextBox();
        lblContrasena = new Label();
        txtUsuario = new TextBox();
        lblUsuario = new Label();
        lblTitulo = new Label();
        lblSubtitulo = new Label();
        lblMarca = new Label();
        pnlTarjeta.SuspendLayout();
        SuspendLayout();
        // 
        // pnlTarjeta
        // 
        pnlTarjeta.Anchor = AnchorStyles.None;
        pnlTarjeta.BackColor = Color.White;
        pnlTarjeta.BorderStyle = BorderStyle.FixedSingle;
        pnlTarjeta.Controls.Add(lblError);
        pnlTarjeta.Controls.Add(lblCredenciales);
        pnlTarjeta.Controls.Add(btnIniciarSesion);
        pnlTarjeta.Controls.Add(txtContrasena);
        pnlTarjeta.Controls.Add(lblContrasena);
        pnlTarjeta.Controls.Add(txtUsuario);
        pnlTarjeta.Controls.Add(lblUsuario);
        pnlTarjeta.Controls.Add(lblTitulo);
        pnlTarjeta.Location = new Point(235, 140);
        pnlTarjeta.Name = "pnlTarjeta";
        pnlTarjeta.Size = new Size(430, 360);
        pnlTarjeta.TabIndex = 2;
        // 
        // lblError
        // 
        lblError.AutoSize = true;
        lblError.ForeColor = Color.FromArgb(184, 54, 54);
        lblError.Location = new Point(48, 249);
        lblError.Name = "lblError";
        lblError.Size = new Size(0, 15);
        lblError.TabIndex = 7;
        lblError.Visible = false;
        // 
        // lblCredenciales
        // 
        lblCredenciales.BackColor = Color.FromArgb(239, 246, 255);
        lblCredenciales.BorderStyle = BorderStyle.FixedSingle;
        lblCredenciales.Location = new Point(48, 282);
        lblCredenciales.Name = "lblCredenciales";
        lblCredenciales.Size = new Size(332, 48);
        lblCredenciales.TabIndex = 6;
        lblCredenciales.Text = "Credenciales de prueba:\r\nUsuario: admin    Contraseña: admin123";
        lblCredenciales.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // btnIniciarSesion
        // 
        btnIniciarSesion.BackColor = Color.FromArgb(47, 124, 246);
        btnIniciarSesion.FlatAppearance.BorderSize = 0;
        btnIniciarSesion.FlatStyle = FlatStyle.Flat;
        btnIniciarSesion.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnIniciarSesion.ForeColor = Color.White;
        btnIniciarSesion.Location = new Point(48, 201);
        btnIniciarSesion.Name = "btnIniciarSesion";
        btnIniciarSesion.Size = new Size(332, 38);
        btnIniciarSesion.TabIndex = 5;
        btnIniciarSesion.Text = "Iniciar Sesión";
        btnIniciarSesion.UseVisualStyleBackColor = false;
        btnIniciarSesion.Click += BtnIniciarSesion_Click;
        // 
        // txtContrasena
        // 
        txtContrasena.Location = new Point(48, 153);
        txtContrasena.Name = "txtContrasena";
        txtContrasena.PlaceholderText = "Ingresa tu contraseña";
        txtContrasena.Size = new Size(332, 23);
        txtContrasena.TabIndex = 4;
        txtContrasena.UseSystemPasswordChar = true;
        txtContrasena.KeyDown += TxtContrasena_KeyDown;
        // 
        // lblContrasena
        // 
        lblContrasena.AutoSize = true;
        lblContrasena.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblContrasena.Location = new Point(48, 130);
        lblContrasena.Name = "lblContrasena";
        lblContrasena.Size = new Size(70, 15);
        lblContrasena.TabIndex = 3;
        lblContrasena.Text = "Contraseña";
        // 
        // txtUsuario
        // 
        txtUsuario.Location = new Point(48, 96);
        txtUsuario.Name = "txtUsuario";
        txtUsuario.PlaceholderText = "Ingresa tu usuario";
        txtUsuario.Size = new Size(332, 23);
        txtUsuario.TabIndex = 2;
        // 
        // lblUsuario
        // 
        lblUsuario.AutoSize = true;
        lblUsuario.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUsuario.Location = new Point(48, 73);
        lblUsuario.Name = "lblUsuario";
        lblUsuario.Size = new Size(49, 15);
        lblUsuario.TabIndex = 1;
        lblUsuario.Text = "Usuario";
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTitulo.Location = new Point(127, 25);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(170, 32);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Iniciar Sesión";
        // 
        // lblSubtitulo
        // 
        lblSubtitulo.Anchor = AnchorStyles.Top;
        lblSubtitulo.AutoSize = true;
        lblSubtitulo.Font = new Font("Segoe UI", 12F);
        lblSubtitulo.ForeColor = Color.White;
        lblSubtitulo.Location = new Point(354, 86);
        lblSubtitulo.Name = "lblSubtitulo";
        lblSubtitulo.Size = new Size(192, 21);
        lblSubtitulo.TabIndex = 1;
        lblSubtitulo.Text = "Sistema de Gestión Médica";
        // 
        // lblMarca
        // 
        lblMarca.Anchor = AnchorStyles.Top;
        lblMarca.AutoSize = true;
        lblMarca.Font = new Font("Segoe UI", 28F, FontStyle.Bold);
        lblMarca.ForeColor = Color.White;
        lblMarca.Location = new Point(373, 31);
        lblMarca.Name = "lblMarca";
        lblMarca.Size = new Size(154, 51);
        lblMarca.TabIndex = 0;
        lblMarca.Text = "SIGEM";
        // 
        // LoginVista
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(47, 124, 246);
        ClientSize = new Size(900, 620);
        Controls.Add(pnlTarjeta);
        Controls.Add(lblSubtitulo);
        Controls.Add(lblMarca);
        Name = "LoginVista";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIGEM - Iniciar Sesión";
        pnlTarjeta.ResumeLayout(false);
        pnlTarjeta.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }
}
