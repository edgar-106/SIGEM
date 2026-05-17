using System.Drawing.Drawing2D;

namespace SIGEM.Vista;

public class AdministracionControl : UserControl
{
    private Panel pnlScroll;
    private Label lblTitulo, lblSubtitulo;

    // Card Info Admin
    private Panel pnlInfoAdmin;
    private Label lblNombreL, lblCorreoL, lblTelefonoL;
    private TextBox txtNombre, txtCorreo, txtTelefono;
    private Button btnGuardarInfo;

    // Card Contraseña
    private Panel pnlPassword;
    private TextBox txtPassActual, txtPassNueva, txtPassConfirm;
    private Button btnActualizarPass;

    // Card Config General
    private Panel pnlConfigGeneral;
    private CheckBox chkNotificaciones, chkRespaldoAuto;
    private ComboBox cmbTiempoSesion;
    private Button btnGuardarConfig;

    // Card Base de Datos
    private Panel pnlBaseDatos;
    private Label lblUltimoRespaldo, lblTamanoBD;
    private Button btnCrearRespaldo, btnRestaurar, btnLimpiar;

    public AdministracionControl()
    {
        InitControls();
    }

    private void InitControls()
    {
        BackColor = Color.FromArgb(239, 246, 255);
        Dock = DockStyle.Fill;

        pnlScroll = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true
        };
        Controls.Add(pnlScroll);

        // Título
        lblTitulo = new Label
        {
            Text = "Configuración del Sistema",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = Color.FromArgb(17, 24, 39),
            AutoSize = true,
            Location = new Point(70, 30)
        };
        pnlScroll.Controls.Add(lblTitulo);

        lblSubtitulo = new Label
        {
            Text = "Gestiona la configuración y seguridad del sistema",
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(40, 68)
        };
        pnlScroll.Controls.Add(lblSubtitulo);

        // Fila 1
        pnlInfoAdmin = CrearCard(40, 110, 370, 320);
        ConstruirInfoAdmin();
        pnlScroll.Controls.Add(pnlInfoAdmin);

        pnlPassword = CrearCard(430, 110, 370, 320);
        ConstruirPassword();
        pnlScroll.Controls.Add(pnlPassword);

        // Fila 2
        pnlConfigGeneral = CrearCard(40, 450, 370, 330);
        ConstruirConfigGeneral();
        pnlScroll.Controls.Add(pnlConfigGeneral);

        pnlBaseDatos = CrearCard(430, 450, 370, 330);
        ConstruirBaseDatos();
        pnlScroll.Controls.Add(pnlBaseDatos);
    }

    // ── Helpers de layout ────────────────────────────────────────

    private Panel CrearCard(int x, int y, int w, int h)
    {
        var p = new Panel
        {
            BackColor = Color.White,
            Location = new Point(x, y),
            Size = new Size(w, h)
        };
        p.Paint += PintarCard;
        return p;
    }

    private void PintarCard(object? sender, PaintEventArgs e)
    {
        if (sender is not Panel p) return;
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var rect = new Rectangle(1, 1, p.Width - 3, p.Height - 3);

        using var shadowBrush = new SolidBrush(Color.FromArgb(15, 0, 0, 0));
        g.FillRectangle(shadowBrush, new Rectangle(3, 3, p.Width - 2, p.Height - 2));

        using var path = RoundedPath(rect, 14);
        using var brush = new SolidBrush(Color.White);
        g.FillPath(brush, path);

        using var pen = new Pen(Color.FromArgb(229, 231, 235), 1.5f);
        g.DrawPath(pen, path);

        p.Region = new Region(RoundedPath(new Rectangle(0, 0, p.Width, p.Height), 14));
    }

    private GraphicsPath RoundedPath(Rectangle b, int r)
    {
        var path = new GraphicsPath();
        path.AddArc(b.X, b.Y, r * 2, r * 2, 180, 90);
        path.AddArc(b.Right - r * 2, b.Y, r * 2, r * 2, 270, 90);
        path.AddArc(b.Right - r * 2, b.Bottom - r * 2, r * 2, r * 2, 0, 90);
        path.AddArc(b.X, b.Bottom - r * 2, r * 2, r * 2, 90, 90);
        path.CloseFigure();
        return path;
    }

    private Label CrearTituloCard(string texto)
    {
        return new Label
        {
            Text = texto,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(37, 99, 235),
            AutoSize = true,
            Location = new Point(24, 20)
        };
    }

    private Label CrearLabel(string texto, int x, int y)
    {
        return new Label
        {
            Text = texto,
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(75, 85, 99),
            AutoSize = true,
            Location = new Point(x, y)
        };
    }

    private TextBox CrearTextBox(int x, int y, int w, string texto = "", bool password = false)
    {
        var txt = new TextBox
        {
            Text = texto,
            Font = new Font("Segoe UI", 10.5F),
            Location = new Point(x, y),
            Size = new Size(w, 32),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = Color.FromArgb(249, 250, 251),
            UseSystemPasswordChar = password
        };
        txt.Enter += (s, e) => txt.BackColor = Color.White;
        txt.Leave += (s, e) => txt.BackColor = Color.FromArgb(249, 250, 251);
        return txt;
    }

    private Button CrearBoton(string texto, Color color, int x, int y, int w, int h = 40)
    {
        var btn = new Button
        {
            Text = texto,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            BackColor = color,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Location = new Point(x, y),
            Size = new Size(w, h),
            Cursor = Cursors.Hand,
            TextAlign = ContentAlignment.MiddleCenter
        };
        btn.FlatAppearance.BorderSize = 0;
        btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(color, 0.1f);
        return btn;
    }

    private CheckBox CrearToggle(int x, int y, bool valorInicial)
    {
        var toggle = new CheckBox
        {
            Appearance = Appearance.Button,
            Text = "",
            Checked = valorInicial,
            Location = new Point(x, y),
            Size = new Size(52, 28),
            FlatStyle = FlatStyle.Flat,
            BackColor = valorInicial ? Color.FromArgb(37, 99, 235) : Color.FromArgb(203, 213, 225),
            Cursor = Cursors.Hand
        };
        toggle.FlatAppearance.BorderSize = 0;
        toggle.CheckedChanged += (s, e) =>
        {
            toggle.BackColor = toggle.Checked
                ? Color.FromArgb(37, 99, 235)
                : Color.FromArgb(203, 213, 225);
        };
        return toggle;
    }

    // ── Cards ────────────────────────────────────────────────────

    private void ConstruirInfoAdmin()
    {
        pnlInfoAdmin.Controls.Add(CrearTituloCard("👤  Información del Administrador"));

        int y = 60;
        pnlInfoAdmin.Controls.Add(CrearLabel("Nombre Completo", 24, y));
        y += 22;
        txtNombre = CrearTextBox(24, y, 322, "Dr. Admin");
        pnlInfoAdmin.Controls.Add(txtNombre);

        y += 48;
        pnlInfoAdmin.Controls.Add(CrearLabel("Correo Electrónico", 24, y));
        y += 22;
        txtCorreo = CrearTextBox(24, y, 322, "admin@sigem.com");
        pnlInfoAdmin.Controls.Add(txtCorreo);

        y += 48;
        pnlInfoAdmin.Controls.Add(CrearLabel("Teléfono", 24, y));
        y += 22;
        txtTelefono = CrearTextBox(24, y, 322, "+52 55 0000 0000");
        pnlInfoAdmin.Controls.Add(txtTelefono);

        y += 52;
        btnGuardarInfo = CrearBoton("💾  Guardar Cambios", Color.FromArgb(37, 99, 235), 24, y, 322);
        btnGuardarInfo.Click += (s, e) =>
            MessageBox.Show("Información guardada correctamente.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        pnlInfoAdmin.Controls.Add(btnGuardarInfo);
    }

    private void ConstruirPassword()
    {
        pnlPassword.Controls.Add(CrearTituloCard("🔒  Cambiar Contraseña"));

        int y = 60;
        pnlPassword.Controls.Add(CrearLabel("Contraseña Actual", 24, y));
        y += 22;
        txtPassActual = CrearTextBox(24, y, 322, password: true);
        pnlPassword.Controls.Add(txtPassActual);

        y += 48;
        pnlPassword.Controls.Add(CrearLabel("Nueva Contraseña", 24, y));
        y += 22;
        txtPassNueva = CrearTextBox(24, y, 322, password: true);
        pnlPassword.Controls.Add(txtPassNueva);

        y += 48;
        pnlPassword.Controls.Add(CrearLabel("Confirmar Nueva Contraseña", 24, y));
        y += 22;
        txtPassConfirm = CrearTextBox(24, y, 322, password: true);
        pnlPassword.Controls.Add(txtPassConfirm);

        y += 52;
        btnActualizarPass = CrearBoton("🔐  Actualizar Contraseña", Color.FromArgb(37, 99, 235), 24, y, 322);
        btnActualizarPass.Click += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(txtPassActual.Text))
            {
                MessageBox.Show("Ingresa la contraseña actual.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassNueva.Text != txtPassConfirm.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            MessageBox.Show("Contraseña actualizada correctamente.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPassActual.Clear();
            txtPassNueva.Clear();
            txtPassConfirm.Clear();
        };
        pnlPassword.Controls.Add(btnActualizarPass);
    }

    private void ConstruirConfigGeneral()
    {
        pnlConfigGeneral.Controls.Add(CrearTituloCard("🔔  Configuración General"));

        int y = 60;

        // Toggle Notificaciones
        pnlConfigGeneral.Controls.Add(new Label
        {
            Text = "Notificaciones",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(24, y)
        });
        pnlConfigGeneral.Controls.Add(new Label
        {
            Text = "Recibir alertas del sistema",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(24, y + 22)
        });
        chkNotificaciones = CrearToggle(300, y + 8, true);
        pnlConfigGeneral.Controls.Add(chkNotificaciones);

        y += 65;

        // Toggle Respaldo
        pnlConfigGeneral.Controls.Add(new Label
        {
            Text = "Respaldo Automático",
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55),
            AutoSize = true,
            Location = new Point(24, y)
        });
        pnlConfigGeneral.Controls.Add(new Label
        {
            Text = "Crear copias de seguridad diarias",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(24, y + 22)
        });
        chkRespaldoAuto = CrearToggle(300, y + 8, true);
        pnlConfigGeneral.Controls.Add(chkRespaldoAuto);

        y += 70;

        pnlConfigGeneral.Controls.Add(CrearLabel("Tiempo de Sesión (minutos)", 24, y));
        y += 24;

        cmbTiempoSesion = new ComboBox
        {
            Location = new Point(24, y),
            Size = new Size(322, 32),
            Font = new Font("Segoe UI", 10F),
            DropDownStyle = ComboBoxStyle.DropDownList,
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(249, 250, 251)
        };
        cmbTiempoSesion.Items.AddRange(["15 minutos", "30 minutos", "45 minutos", "60 minutos"]);
        cmbTiempoSesion.SelectedIndex = 1;
        pnlConfigGeneral.Controls.Add(cmbTiempoSesion);

        y += 48;
        btnGuardarConfig = CrearBoton("💾  Guardar Configuración", Color.FromArgb(37, 99, 235), 24, y, 322);
        btnGuardarConfig.Click += (s, e) =>
            MessageBox.Show("Configuración guardada correctamente.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        pnlConfigGeneral.Controls.Add(btnGuardarConfig);
    }

    private void ConstruirBaseDatos()
    {
        pnlBaseDatos.Controls.Add(CrearTituloCard("🗄️  Gestión de Base de Datos"));

        // Panel info BD
        var pnlInfo = new Panel
        {
            BackColor = Color.FromArgb(239, 246, 255),
            Location = new Point(24, 60),
            Size = new Size(322, 65),
            Padding = new Padding(12)
        };

        lblUltimoRespaldo = new Label
        {
            Text = "Último respaldo: 22/04/2026 - 08:00 AM",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize = true,
            Location = new Point(12, 12)
        };
        lblTamanoBD = new Label
        {
            Text = "Tamaño de base de datos: 45.2 MB",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize = true,
            Location = new Point(12, 36)
        };
        pnlInfo.Controls.Add(lblUltimoRespaldo);
        pnlInfo.Controls.Add(lblTamanoBD);
        pnlBaseDatos.Controls.Add(pnlInfo);

        int y = 142;

        btnCrearRespaldo = CrearBoton("Crear Respaldo Manual", Color.FromArgb(22, 163, 74), 24, y, 322);
        btnCrearRespaldo.Click += (s, e) =>
        {
            lblUltimoRespaldo.Text = $"Último respaldo: {DateTime.Now:dd/MM/yyyy - hh:mm tt}";
            MessageBox.Show("Respaldo creado correctamente.", "SIGEM",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        pnlBaseDatos.Controls.Add(btnCrearRespaldo);

        y += 52;
        btnRestaurar = CrearBoton("Restaurar desde Respaldo", Color.FromArgb(217, 119, 6), 24, y, 322);
        btnRestaurar.Click += (s, e) =>
        {
            if (MessageBox.Show("¿Restaurar desde el último respaldo?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                MessageBox.Show("Base de datos restaurada.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        pnlBaseDatos.Controls.Add(btnRestaurar);

        y += 52;
        btnLimpiar = CrearBoton("Limpiar Datos Antiguos", Color.FromArgb(220, 38, 38), 24, y, 322);
        btnLimpiar.Click += (s, e) =>
        {
            if (MessageBox.Show("¿Eliminar datos antiguos? Esta acción no se puede deshacer.", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                MessageBox.Show("Datos eliminados correctamente.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        };
        pnlBaseDatos.Controls.Add(btnLimpiar);
    }
}