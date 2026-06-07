using SIGEM.Modelo;
using System.Drawing.Drawing2D;

namespace SIGEM.Vista;

public class AdministracionControl : UserControl
{
    private readonly Usuario usuarioActual;
    private readonly ServicioAdministracionSistema servicioAdministracion;

    private Panel pnlScroll = null!;
    private Label lblTitulo = null!, lblSubtitulo = null!;

    // Card Config General
    private Panel pnlConfigGeneral = null!;
    private CheckBox chkNotificaciones = null!, chkRespaldoAuto = null!;
    private ComboBox cmbTiempoSesion = null!;
    private Button btnGuardarConfig = null!;

    // Card Base de Datos
    private Panel pnlBaseDatos = null!;
    private Label lblUltimoRespaldo = null!, lblTamanoBD = null!;
    private Button btnCrearRespaldo = null!, btnRestaurar = null!, btnLimpiar = null!;

    // Card Gestión Usuarios
    private Panel pnlGestionUsuarios = null!;

    public AdministracionControl(Usuario usuarioActual)
    {
        this.usuarioActual = usuarioActual ?? throw new ArgumentNullException(nameof(usuarioActual));
        servicioAdministracion = new ServicioAdministracionSistema();

        InitControls();
        CargarDatosIniciales();
    }

    private void InitControls()
    {
        BackColor = Color.FromArgb(239, 246, 255);
        Dock = DockStyle.Fill;

        pnlScroll = new Panel
        {
            Dock = DockStyle.Fill,
            AutoScroll = true,
            AutoScrollMinSize = new Size(850, 920),
            BackColor = Color.FromArgb(239, 246, 255)
        };
        Controls.Add(pnlScroll);

        lblTitulo = new Label
        {
            Text = "Configuración del Sistema",
            Font = new Font("Segoe UI", 22F, FontStyle.Bold),
            ForeColor = Color.FromArgb(17, 24, 39),
            AutoSize = true,
            Location = new Point(40, 30)
        };
        pnlScroll.Controls.Add(lblTitulo);

        lblSubtitulo = new Label
        {
            Text = "Gestiona la configuración, seguridad, usuarios y respaldos del sistema",
            Font = new Font("Segoe UI", 11F),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(40, 92)
        };
        pnlScroll.Controls.Add(lblSubtitulo);

        pnlGestionUsuarios = CrearCard(40, 140, 760, 270);
        ConstruirGestionUsuarios();
        pnlScroll.Controls.Add(pnlGestionUsuarios);

        pnlConfigGeneral = CrearCard(40, 430, 370, 330);
        ConstruirConfigGeneral();
        pnlScroll.Controls.Add(pnlConfigGeneral);

        pnlBaseDatos = CrearCard(430, 430, 370, 330);
        ConstruirBaseDatos();
        pnlScroll.Controls.Add(pnlBaseDatos);
    }

    // ==========================================================
    // Helpers visuales
    // ==========================================================

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

        using var sombra = new SolidBrush(Color.FromArgb(15, 0, 0, 0));
        g.FillRectangle(sombra, new Rectangle(3, 3, p.Width - 2, p.Height - 2));

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

    // ==========================================================
    // Card: Configuración general
    // ==========================================================

    private void ConstruirConfigGeneral()
    {
        pnlConfigGeneral.Controls.Add(CrearTituloCard("Configuración General"));

        int y = 60;

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

        pnlConfigGeneral.Controls.Add(CrearLabel("Tiempo de Sesión", 24, y));
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

        cmbTiempoSesion.Items.AddRange(new object[]
        {
            "15 minutos",
            "30 minutos",
            "45 minutos",
            "60 minutos"
        });

        cmbTiempoSesion.SelectedIndex = 1;
        pnlConfigGeneral.Controls.Add(cmbTiempoSesion);

        y += 48;
        btnGuardarConfig = CrearBoton("Guardar Configuración", Color.FromArgb(37, 99, 235), 24, y, 322);

        btnGuardarConfig.Click += (s, e) =>
        {
            try
            {
                int minutos = 30;

                if (cmbTiempoSesion.SelectedItem is not null)
                {
                    string texto = cmbTiempoSesion.SelectedItem.ToString() ?? "30 minutos";
                    string numero = texto.Split(' ')[0];

                    if (!int.TryParse(numero, out minutos))
                        minutos = 30;
                }

                var config = new ConfiguracionSistema
                {
                    Notificaciones = chkNotificaciones.Checked,
                    RespaldoAutomatico = chkRespaldoAuto.Checked,
                    TiempoSesionMinutos = minutos
                };

                servicioAdministracion.GuardarConfiguracion(config);

                MessageBox.Show("Configuración guardada correctamente.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la configuración.\n\nDetalle: {ex.Message}", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        pnlConfigGeneral.Controls.Add(btnGuardarConfig);
    }

    // ==========================================================
    // Card: Base de datos
    // ==========================================================

    private void ConstruirBaseDatos()
    {
        pnlBaseDatos.Controls.Add(CrearTituloCard("Gestión de Base de Datos"));

        var pnlInfo = new Panel
        {
            BackColor = Color.FromArgb(239, 246, 255),
            Location = new Point(24, 60),
            Size = new Size(322, 65),
            Padding = new Padding(12)
        };

        lblUltimoRespaldo = new Label
        {
            Text = "Base de datos PostgreSQL: IMSS",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(55, 65, 81),
            AutoSize = true,
            Location = new Point(12, 12)
        };

        lblTamanoBD = new Label
        {
            Text = "Tamaño de base de datos: calculando...",
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
            try
            {
                string archivo = servicioAdministracion.CrearRespaldoPostgres();

                lblUltimoRespaldo.Text = $"Último respaldo: {DateTime.Now:dd/MM/yyyy - hh:mm tt}";
                lblTamanoBD.Text = $"Tamaño de base de datos: {servicioAdministracion.ObtenerTamanoBaseDatos()}";

                MessageBox.Show($"Respaldo creado correctamente.\n\nArchivo:\n{archivo}", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo crear el respaldo.\n\nDetalle: {ex.Message}", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        pnlBaseDatos.Controls.Add(btnCrearRespaldo);

        y += 52;

        btnRestaurar = CrearBoton("Restaurar desde Respaldo", Color.FromArgb(217, 119, 6), 24, y, 322);

        btnRestaurar.Click += (s, e) =>
        {
            using OpenFileDialog dialogo = new()
            {
                Title = "Seleccionar respaldo SQL",
                Filter = "Archivos SQL (*.sql)|*.sql",
                InitialDirectory = Path.Combine(AppContext.BaseDirectory, "Datos", "ims", "respaldos")
            };

            if (dialogo.ShowDialog() != DialogResult.OK)
                return;

            if (MessageBox.Show(
                "¿Seguro que deseas restaurar este respaldo?\n\nEsta acción puede modificar la información actual.",
                "Confirmar restauración",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                servicioAdministracion.RestaurarRespaldoPostgres(dialogo.FileName);

                lblTamanoBD.Text = $"Tamaño de base de datos: {servicioAdministracion.ObtenerTamanoBaseDatos()}";

                MessageBox.Show("Base de datos restaurada correctamente.", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo restaurar el respaldo.\n\nDetalle: {ex.Message}", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        pnlBaseDatos.Controls.Add(btnRestaurar);

        y += 52;

        btnLimpiar = CrearBoton("Limpiar Datos Antiguos", Color.FromArgb(220, 38, 38), 24, y, 322);

        btnLimpiar.Click += (s, e) =>
        {
            if (MessageBox.Show(
                "¿Eliminar notas de evolución con más de 1 año de antigüedad?\n\nEsta acción no se puede deshacer.",
                "Confirmar limpieza",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            try
            {
                int eliminados = servicioAdministracion.LimpiarDatosAntiguos(365);

                lblTamanoBD.Text = $"Tamaño de base de datos: {servicioAdministracion.ObtenerTamanoBaseDatos()}";

                MessageBox.Show($"Limpieza completada.\nRegistros eliminados: {eliminados}", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron limpiar los datos antiguos.\n\nDetalle: {ex.Message}", "SIGEM",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        pnlBaseDatos.Controls.Add(btnLimpiar);
    }

    // ==========================================================
    // Card: Gestión de usuarios
    // ==========================================================

    private void ConstruirGestionUsuarios()
    {
        pnlGestionUsuarios.Controls.Add(CrearTituloCard("Gestion de Usuarios"));

        pnlGestionUsuarios.Controls.Add(new Label
        {
            Text = "Administra la informacion, credenciales y roles del personal del sistema",
            Font = new Font("Segoe UI", 9.5F),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(24, 52)
        });

        int y = 95;
        const int altoBoton = 68;

        var btnAdmin = CrearBoton("Informacion del\nAdministrador", Color.FromArgb(37, 99, 235), 24, y, 230, altoBoton);
        btnAdmin.Click += (_, _) => AbrirVentanaUsuario("Administrador");
        pnlGestionUsuarios.Controls.Add(btnAdmin);

        var btnDoctor = CrearBoton("Informacion del\nDoctor", Color.FromArgb(22, 163, 74), 270, y, 230, altoBoton);
        btnDoctor.Click += (_, _) => AbrirVentanaUsuario("Doctor");
        pnlGestionUsuarios.Controls.Add(btnDoctor);

        var btnEnfermera = CrearBoton("Informacion de la\nEnfermera", Color.FromArgb(16, 185, 129), 516, y, 230, altoBoton);
        btnEnfermera.Click += (_, _) => AbrirVentanaUsuario("Enfermera");
        pnlGestionUsuarios.Controls.Add(btnEnfermera);

        pnlGestionUsuarios.Controls.Add(new Label
        {
            Text = "Selecciona un rol para abrir su ventana de gestion con opciones de editar, guardar y eliminar.",
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(107, 114, 128),
            AutoSize = true,
            Location = new Point(24, y + altoBoton + 16)
        });
    }

    private void AbrirVentanaUsuario(string rol)
    {
        using var ventana = new VentanaGestionUsuario(servicioAdministracion, usuarioActual, rol);
        ventana.ShowDialog(FindForm());
    }

    // ==========================================================
    // Carga inicial
    // ==========================================================

    private void CargarDatosIniciales()
    {
        try
        {
            var config = servicioAdministracion.ObtenerConfiguracion();

            chkNotificaciones.Checked = config.Notificaciones;
            chkRespaldoAuto.Checked = config.RespaldoAutomatico;

            string tiempo = $"{config.TiempoSesionMinutos} minutos";

            if (cmbTiempoSesion.Items.Contains(tiempo))
                cmbTiempoSesion.SelectedItem = tiempo;
            else
                cmbTiempoSesion.SelectedIndex = 1;

            lblUltimoRespaldo.Text = "Base de datos PostgreSQL: IMSS";
            lblTamanoBD.Text = $"Tamaño de base de datos: {servicioAdministracion.ObtenerTamanoBaseDatos()}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudieron cargar los datos de administración.\n\nDetalle: {ex.Message}",
                "SIGEM",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
        }
    }

}