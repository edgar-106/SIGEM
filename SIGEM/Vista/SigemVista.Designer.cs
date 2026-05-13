namespace SIGEM.Vista;

partial class SigemVista
{
    private System.ComponentModel.IContainer components = null;
    private Label lblTitulo;
    private Label lblNombre;
    private TextBox txtNombre;
    private Button btnGuardar;
    private ListBox lstRegistros;
    private Label lblRegistros;
    private Label lblEstado;

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
        lblTitulo = new Label();
        lblNombre = new Label();
        txtNombre = new TextBox();
        btnGuardar = new Button();
        lstRegistros = new ListBox();
        lblRegistros = new Label();
        lblEstado = new Label();
        SuspendLayout();
        // 
        // lblTitulo
        // 
        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitulo.Location = new Point(32, 28);
        lblTitulo.Name = "lblTitulo";
        lblTitulo.Size = new Size(191, 30);
        lblTitulo.TabIndex = 0;
        lblTitulo.Text = "Registro SIGEM";
        // 
        // lblNombre
        // 
        lblNombre.AutoSize = true;
        lblNombre.Location = new Point(35, 86);
        lblNombre.Name = "lblNombre";
        lblNombre.Size = new Size(51, 15);
        lblNombre.TabIndex = 1;
        lblNombre.Text = "Nombre";
        // 
        // txtNombre
        // 
        txtNombre.Location = new Point(35, 108);
        txtNombre.Name = "txtNombre";
        txtNombre.PlaceholderText = "Escribe un nombre";
        txtNombre.Size = new Size(316, 23);
        txtNombre.TabIndex = 2;
        // 
        // btnGuardar
        // 
        btnGuardar.Location = new Point(367, 107);
        btnGuardar.Name = "btnGuardar";
        btnGuardar.Size = new Size(91, 25);
        btnGuardar.TabIndex = 3;
        btnGuardar.Text = "Guardar";
        btnGuardar.UseVisualStyleBackColor = true;
        btnGuardar.Click += BtnGuardar_Click;
        // 
        // lstRegistros
        // 
        lstRegistros.FormattingEnabled = true;
        lstRegistros.ItemHeight = 15;
        lstRegistros.Location = new Point(35, 191);
        lstRegistros.Name = "lstRegistros";
        lstRegistros.Size = new Size(423, 184);
        lstRegistros.TabIndex = 6;
        // 
        // lblRegistros
        // 
        lblRegistros.AutoSize = true;
        lblRegistros.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblRegistros.Location = new Point(35, 158);
        lblRegistros.Name = "lblRegistros";
        lblRegistros.Size = new Size(141, 19);
        lblRegistros.TabIndex = 5;
        lblRegistros.Text = "Registros guardados";
        // 
        // lblEstado
        // 
        lblEstado.AutoSize = true;
        lblEstado.Location = new Point(35, 394);
        lblEstado.Name = "lblEstado";
        lblEstado.Size = new Size(0, 15);
        lblEstado.TabIndex = 7;
        // 
        // SigemVista
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(500, 430);
        Controls.Add(lblEstado);
        Controls.Add(lblRegistros);
        Controls.Add(lstRegistros);
        Controls.Add(btnGuardar);
        Controls.Add(txtNombre);
        Controls.Add(lblNombre);
        Controls.Add(lblTitulo);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "SigemVista";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "SIGEM";
        ResumeLayout(false);
        PerformLayout();
    }
}
