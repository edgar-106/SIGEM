namespace SIGEM.Vista;

public partial class RecetaDialog : Form
{
    public string Diagnostico => txtDiagnostico.Text.Trim();
    public string Tratamiento => txtTratamiento.Text.Trim();

    public RecetaDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        var lblTitulo = new Label();
        var lblDiagnostico = new Label();
        txtDiagnostico = new TextBox();
        var lblTratamiento = new Label();
        txtTratamiento = new TextBox();
        var btnOk = new Button();
        var btnCancel = new Button();
        var pnlHeader = new Panel();

        SuspendLayout();

        pnlHeader.BackColor = Color.FromArgb(37, 99, 235);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.Height = 48;

        lblTitulo.AutoSize = true;
        lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        lblTitulo.ForeColor = Color.White;
        lblTitulo.Location = new Point(16, 10);
        lblTitulo.Text = "Receta Médica - Diagnóstico y Tratamiento";

        pnlHeader.Controls.Add(lblTitulo);

        lblDiagnostico.AutoSize = true;
        lblDiagnostico.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblDiagnostico.Location = new Point(16, 64);
        lblDiagnostico.Text = "Diagnóstico:";

        txtDiagnostico.Font = new Font("Segoe UI", 10F);
        txtDiagnostico.Location = new Point(16, 88);
        txtDiagnostico.Multiline = true;
        txtDiagnostico.Size = new Size(520, 80);
        txtDiagnostico.ScrollBars = ScrollBars.Vertical;
        txtDiagnostico.PlaceholderText = "Describa el diagnóstico del paciente...";

        lblTratamiento.AutoSize = true;
        lblTratamiento.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblTratamiento.Location = new Point(16, 180);
        lblTratamiento.Text = "Tratamiento:";

        txtTratamiento.Font = new Font("Segoe UI", 10F);
        txtTratamiento.Location = new Point(16, 204);
        txtTratamiento.Multiline = true;
        txtTratamiento.Size = new Size(520, 80);
        txtTratamiento.ScrollBars = ScrollBars.Vertical;
        txtTratamiento.PlaceholderText = "Describa el tratamiento recetado...";

        btnOk.BackColor = Color.FromArgb(47, 124, 246);
        btnOk.FlatAppearance.BorderSize = 0;
        btnOk.FlatStyle = FlatStyle.Flat;
        btnOk.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnOk.ForeColor = Color.White;
        btnOk.Location = new Point(280, 300);
        btnOk.Size = new Size(120, 36);
        btnOk.Text = "Generar";
        btnOk.UseVisualStyleBackColor = false;
        btnOk.DialogResult = DialogResult.OK;

        btnCancel.BackColor = Color.FromArgb(107, 114, 128);
        btnCancel.FlatAppearance.BorderSize = 0;
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(416, 300);
        btnCancel.Size = new Size(120, 36);
        btnCancel.Text = "Cancelar";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.DialogResult = DialogResult.Cancel;

        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(239, 246, 255);
        ClientSize = new Size(552, 350);
        Controls.Add(btnCancel);
        Controls.Add(btnOk);
        Controls.Add(txtTratamiento);
        Controls.Add(lblTratamiento);
        Controls.Add(txtDiagnostico);
        Controls.Add(lblDiagnostico);
        Controls.Add(pnlHeader);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Receta Médica";
        AcceptButton = btnOk;
        CancelButton = btnCancel;

        ResumeLayout(false);
        PerformLayout();
    }

    private TextBox txtDiagnostico = null!;
    private TextBox txtTratamiento = null!;
}
