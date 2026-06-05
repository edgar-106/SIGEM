using SIGEM.Modelo;

namespace SIGEM.Vista;


public partial class MenuPrincipalVista : Form
{
    private readonly Usuario usuario;
    private readonly ISigemRepositorio repositorio = new SigemRepositorioJson();
    private Form? formularioEmbebido;
    private string subSeccionConsulta = "Nueva Consulta";

    public MenuPrincipalVista(Usuario usuario)
    {
        this.usuario = usuario;
        InitializeComponent();
        lblUsuario.Text = $"Usuario: {usuario.NombreCompleto} ({ObtenerRolTexto(usuario.Rol)})";

        // Evaluar los permisos del rol antes de mostrar cualquier pantalla
        VerificarPermisosPorRol();

        MostrarPanelPrincipal();
        // Solo el administrador ve el bot�n
        btnAdministracion.Visible = usuario.Rol == RolUsuario.Administrador;
        btnPacientes.Visible = usuario.Rol != RolUsuario.Administrador;
        btnConsulta.Visible = usuario.Rol != RolUsuario.Administrador;
        btnPanelPrincipal.Visible = usuario.Rol != RolUsuario.Administrador;
    }

    // Nuevo método para ocultar botones del menú y cambiar colores a Verde Lima según el rol
    private void VerificarPermisosPorRol()
    {
        if (usuario.Rol == RolUsuario.Enfermera)
        {
            // Oculta los botones del menú lateral izquierdo
            btnPacientes.Visible = false;
            btnAdministracion.Visible = false;

            // Mueve el botón de Consulta Médica hacia arriba 
            btnConsulta.Location = btnPacientes.Location;

            // CAMBIO: Verde lima suave para el fondo del menú de la enfermera
            pnlMenu.BackColor = Color.FromArgb(236, 253, 245);
            btnPanelPrincipal.BackColor = Color.FromArgb(236, 253, 245);
            btnConsulta.BackColor = Color.FromArgb(236, 253, 245);

            // CAMBIO NUEVO: Cambia también el fondo del logo "SIGEM" a verde para la enfermera
            // (Asegúrate de que el panel azul superior de tu diseño se llame pnlLogo)
            pnlMarca.BackColor = Color.FromArgb(16, 185, 129);
        }
        else if (usuario.Rol == RolUsuario.Doctor)
        {
            // El doctor tiene acceso completo
            btnPacientes.Visible = true;
            btnAdministracion.Visible = true;

            // Asegurar que el Doctor mantenga sus colores originales (Blanco y Azul)
            pnlMenu.BackColor = Color.White;
            btnPanelPrincipal.BackColor = Color.White;
            btnConsulta.BackColor = Color.White;
            pnlMarca.BackColor = Color.FromArgb(47, 124, 246); // Azul original
        }
    }

    private static string ObtenerRolTexto(RolUsuario rol) => rol switch
    {
        RolUsuario.Doctor => "Doctor",
        RolUsuario.Enfermera => "Enfermera",
        RolUsuario.Administrador => "Administrador",
        _ => "Usuario"
    };

    private void MostrarContenido(string titulo, string     subtitulo)
    {
        LimpiarContenido();
        lblTituloContenido.Text = titulo;
        lblSubtituloContenido.Text = subtitulo;
        pnlContenido.Controls.Add(lblTituloContenido);
        pnlContenido.Controls.Add(lblSubtituloContenido);
    }

    private void MostrarPanelPrincipal()
    {
        MostrarContenido("Panel Principal", "Bienvenido al Sistema de Gestion Medica - SIGEM");
        SeleccionarBoton(btnPanelPrincipal);
        ConstruirPanelPrincipal();
        CargarPanelPrincipal();
    }

    private void MostrarGestionPacientes()
    {
        if (usuario.Rol == RolUsuario.Enfermera) return;

        MostrarContenido("Gestion de Pacientes", "Administracion completa de expedientes medicos");
        SeleccionarBoton(btnPacientes);
        ConstruirGestionPacientes();
    }

    private void MostrarConsultaMedica()
    {
        MostrarContenido("Consulta Medica", "Gestion de consultas, diagnosticos y tratamientos");
        SeleccionarBoton(btnConsulta);
        ConstruirConsultaMedica();
    }

    private void MostrarAdministracion()
    {
        if (usuario.Rol == RolUsuario.Enfermera) return;

        LimpiarContenido();
        SeleccionarBoton(btnAdministracion);
        var adminControl = new AdministracionControl();
        pnlContenido.Controls.Add(adminControl);
    }

    private void LimpiarContenido()
    {
        if (formularioEmbebido is not null)
        {
            formularioEmbebido.Close();
            formularioEmbebido.Dispose();
            formularioEmbebido = null;
        }

        pnlContenido.Controls.Clear();
    }

    private void ConstruirPanelPrincipal()
    {
        pnlContenido.Controls.Add(cardPacientes);
        pnlContenido.Controls.Add(cardBorradores);
        pnlContenido.Controls.Add(cardSignos);
        pnlContenido.Controls.Add(cardHoy);
        pnlContenido.Controls.Add(pnlPacientesRecientes);
        pnlContenido.Controls.Add(pnlActividadReciente);
        pnlContenido.Controls.Add(pnlBorradores);

        if (usuario.Rol == RolUsuario.Enfermera)
        {
            cardPacientes.Visible = false;
            pnlPacientesRecientes.Visible = false;
            cardBorradores.Visible = false;
            pnlBorradores.Visible = false;
        }
        else
        {
            cardPacientes.Visible = true;
            pnlPacientesRecientes.Visible = true;
            cardBorradores.Visible = true;
            pnlBorradores.Visible = true;
        }

        cardSignos.Visible = true;
        cardHoy.Visible = true;
        pnlActividadReciente.Visible = true;
    }

    private void ConstruirGestionPacientes()
    {
        FlowLayoutPanel tabs = CrearTabs(200, [
            ("Registros", true),
            ("Buscar Expediente", false),
            ("Historial Clinico", false),
            ("Agregar Evolucion", false)
        ]);
        pnlContenido.Controls.Add(tabs);

        Label titulo = CrearTituloSeccion("Pacientes", 48, 270);
        Label subtitulo = CrearTexto("Gestiona la informacion de los pacientes", 48, 323, 620, 28, 12F);
        pnlContenido.Controls.Add(titulo);
        pnlContenido.Controls.Add(subtitulo);

        TextBox buscador = new()
        {
            Location = new Point(48, 375),
            Size = new Size(660, 35),
            Font = new Font("Segoe UI", 11F),
            PlaceholderText = "Buscar paciente..."
        };
        pnlContenido.Controls.Add(buscador);

        Button nuevo = CrearBotonPrimario("+  Nuevo Paciente", 730, 375, 220, 46);
        nuevo.Click += (_, _) => SeleccionarConsulta("Nueva Consulta");
        pnlContenido.Controls.Add(nuevo);

        FlowLayoutPanel lista = new()
        {
            Location = new Point(48, 450),
            Size = new Size(1000, 390),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            AutoScroll = true,
            BackColor = pnlContenido.BackColor,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = true
        };
        pnlContenido.Controls.Add(lista);

        void CargarTarjetas(string filtro)
        {
            lista.Controls.Clear();
            List<Paciente> pacientes = repositorio.ObtenerTodos()
                .Where(paciente => CoincidePaciente(paciente, filtro))
                .OrderByDescending(paciente => paciente.FechaRegistro)
                .ToList();

            if (pacientes.Count == 0)
            {
                lista.Controls.Add(CrearTarjetaVacia("No hay pacientes para mostrar."));
                return;
            }

            foreach (Paciente paciente in pacientes)
            {
                lista.Controls.Add(CrearTarjetaPaciente(paciente));
            }
        }

        buscador.TextChanged += (_, _) => CargarTarjetas(buscador.Text);
        CargarTarjetas(string.Empty);
    }

    private void ConstruirConsultaMedica()
    {
        var listaTabs = new List<(string Texto, bool Activo)>
        {
            ("Nueva Consulta", subSeccionConsulta == "Nueva Consulta"),
            ("Receta Medica", subSeccionConsulta == "Receta Medica")
        };

        if (usuario.Rol == RolUsuario.Doctor)
        {
            listaTabs.Add(("Diagnostico", subSeccionConsulta == "Diagnostico"));
            listaTabs.Add(("Tratamiento", subSeccionConsulta == "Tratamiento"));
        }

        FlowLayoutPanel tabs = CrearTabs(200, listaTabs.ToArray());
        pnlContenido.Controls.Add(tabs);

        switch (subSeccionConsulta)
        {
            case "Receta Medica":
                ConstruirMensajeSimple("Emitir Receta Medica", "Esta funcionalidad se encuentra en la seccion de gestion de pacientes. Selecciona un paciente para emitir una receta medica.", 330);
                break;
            case "Diagnostico":
                if (usuario.Rol == RolUsuario.Doctor) ConstruirFormularioDiagnostico();
                else SeleccionarConsulta("Nueva Consulta");
                break;
            case "Tratamiento":
                if (usuario.Rol == RolUsuario.Doctor) ConstruirFormularioTratamiento();
                else SeleccionarConsulta("Nueva Consulta");
                break;
            default:
                ConstruirNuevaConsulta();
                break;
        }
    }

    private FlowLayoutPanel CrearTabs(int y, (string Texto, bool Activo)[] tabs)
    {
        FlowLayoutPanel panel = new()
        {
            Location = new Point(48, y),
            Size = new Size(940, 78),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.White,
            Padding = new Padding(18, 14, 18, 14),
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false
        };

        // CAMBIO NUEVO: Definir el color de la pestaña activa según el rol
        Color colorTabActiva = (usuario.Rol == RolUsuario.Enfermera)
            ? Color.FromArgb(16, 185, 129)   // Verde para enfermera
            : Color.FromArgb(47, 124, 246);  // Azul para doctor

        foreach ((string texto, bool activo) in tabs)
        {
            Button boton = new()
            {
                Text = texto,
                Size = new Size(Math.Max(170, texto.Length * 11 + 52), 46),
                Margin = new Padding(0, 0, 10, 0),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = activo ? colorTabActiva : Color.FromArgb(243, 244, 246), // Aplica color dinámico
                ForeColor = activo ? Color.White : Color.FromArgb(31, 41, 55),
                Cursor = Cursors.Hand
            };
            boton.FlatAppearance.BorderSize = 0;

            if (texto is "Nueva Consulta" or "Receta Medica" or "Diagnostico" or "Tratamiento")
            {
                boton.Click += (_, _) => SeleccionarConsulta(texto);
            }

            panel.Controls.Add(boton);
        }

        return panel;
    }

    private void SeleccionarConsulta(string seccion)
    {
        subSeccionConsulta = seccion;
        MostrarConsultaMedica();
    }

    private void ConstruirNuevaConsulta()
    {
        Panel contenedor = CrearTarjetaContenido(48, 310, 940, 650);
        contenedor.AutoScroll = true;
        pnlContenido.Controls.Add(contenedor);

        Panel pnlBusqueda = new()
        {
            Location = new Point(28, 48),
            Size = new Size(580, 40),
            BackColor = Color.FromArgb(243, 244, 246),
            BorderStyle = BorderStyle.FixedSingle
        };
        TextBox buscador = new()
        {
            Location = new Point(12, 8),
            Size = new Size(550, 28),
            Font = new Font("Segoe UI", 11F),
            BorderStyle = BorderStyle.None,
            BackColor = Color.FromArgb(243, 244, 246),
            PlaceholderText = "Buscar paciente por nombre, expediente o CURP..."
        };
        pnlBusqueda.Controls.Add(buscador);
        contenedor.Controls.Add(pnlBusqueda);

        Label lblContador = new()
        {
            Location = new Point(28, 96),
            Size = new Size(400, 20),
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(156, 163, 175)
        };
        contenedor.Controls.Add(lblContador);

        FlowLayoutPanel lista = new()
        {
            Location = new Point(28, 120),
            Size = new Size(880, 490),
            AutoScroll = true,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false,
            BackColor = Color.White
        };
        contenedor.Controls.Add(lista);

        Button btnNuevo = CrearBotonPrimario("+ Nuevo Paciente", 628, 46, 280, 42);
        btnNuevo.BackColor = Color.FromArgb(16, 185, 129);
        contenedor.Controls.Add(btnNuevo);

        void CargarTarjetas(string filtro)
        {
            lista.Controls.Clear();
            List<Paciente> pacientes = repositorio.ObtenerTodos();

            if (!string.IsNullOrWhiteSpace(filtro))
            {
                pacientes = pacientes.Where(p =>
                    (p.Nombre + " " + p.Apellido).Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Expediente ?? "").Contains(filtro, StringComparison.OrdinalIgnoreCase) ||
                    (p.Curp ?? "").Contains(filtro, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            lblContador.Text = pacientes.Count == 1
                ? "1 paciente encontrado"
                : $"{pacientes.Count} pacientes encontrados";

            if (pacientes.Count == 0)
            {
                Panel empty = CrearTarjetaVacia("No hay pacientes registrados. Use el boton \"+ Nuevo Paciente\" para crear uno.");
                lista.Controls.Add(empty);
                return;
            }

            foreach (Paciente p in pacientes.OrderByDescending(p => p.FechaRegistro))
            {
                lista.Controls.Add(CrearTarjetaCrud(p, () => CargarTarjetas(buscador.Text)));
            }
        }

        btnNuevo.Click += (_, _) =>
        {
            SigemVista sv = new(usuario);
            sv.SetUsuario(usuario);
            sv.EstablecerModoNuevoPaciente();
            sv.ShowDialog();
            CargarTarjetas(buscador.Text);
        };

        buscador.TextChanged += (_, _) => CargarTarjetas(buscador.Text);
        buscador.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter) e.SuppressKeyPress = true;
        };
        CargarTarjetas(string.Empty);
    }

    private Panel CrearTarjetaCrud(Paciente p, Action recargar)
    {
        Panel tarjeta = new()
        {
            Size = new Size(850, 76),
            BackColor = Color.White,
            Margin = new Padding(0, 0, 0, 10)
        };
        tarjeta.Paint += (s, e) =>
        {
            if (s is not Panel pnl) return;
            var g = e.Graphics;
            using var pen = new Pen(Color.FromArgb(229, 231, 235), 1);
            g.DrawRectangle(pen, 0, 0, pnl.Width - 1, pnl.Height - 1);
            using var shadow = new SolidBrush(Color.FromArgb(8, 0, 0, 0));
            g.FillRectangle(shadow, 2, 2, pnl.Width - 2, 3);
        };

        string inicial = p.Nombre.Length > 0 ? p.Nombre[..1].ToUpperInvariant() : "?";
        Color colorAvatar = p.EsBorrador ? Color.FromArgb(245, 158, 11) : Color.FromArgb(47, 124, 246);
        Panel avatar = new()
        {
            Location = new Point(12, 14),
            Size = new Size(48, 48),
            BackColor = colorAvatar
        };
        avatar.Paint += (s, e) =>
        {
            if (s is not Panel a) return;
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            using var brush = new SolidBrush(a.BackColor);
            g.FillEllipse(brush, 0, 0, a.Width - 1, a.Height - 1);
            using var f = new Font("Segoe UI", 16F, FontStyle.Bold);
            g.DrawString(inicial, f, Brushes.White, 12, 10);
        };
        tarjeta.Controls.Add(avatar);

        int edad = p.FechaNacimiento == default ? 0 : CalcularEdad(p.FechaNacimiento);
        string edadStr = edad > 0 ? $"{edad} anos" : "---";
        Label lblNombre = new()
        {
            Text = $"{p.Nombre} {p.Apellido}".Trim(),
            Location = new Point(74, 14),
            Size = new Size(280, 24),
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = Color.FromArgb(17, 24, 39)
        };
        tarjeta.Controls.Add(lblNombre);

        Label lblDetalle = new()
        {
            Text = $"{edadStr} | {TextoSeguro(p.Sexo, "---")} | Exp: {TextoSeguro(p.Expediente, "---")}",
            Location = new Point(74, 40),
            Size = new Size(340, 20),
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(107, 114, 128)
        };
        tarjeta.Controls.Add(lblDetalle);

        int registros = p.SignosVitales.Count;
        Label lblRegistros = new()
        {
            Text = registros == 1 ? "1 registro" : $"{registros} registros",
            Location = new Point(370, 16),
            Size = new Size(80, 20),
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(107, 114, 128),
            TextAlign = ContentAlignment.MiddleCenter
        };
        tarjeta.Controls.Add(lblRegistros);

        Panel badge = new()
        {
            Location = new Point(370, 40),
            Size = new Size(80, 20),
            BackColor = p.EsBorrador ? Color.FromArgb(254, 243, 199) : Color.FromArgb(209, 250, 229)
        };
        Label lblBadge = new()
        {
            Text = p.EsBorrador ? "BORRADOR" : "VALIDADO",
            Location = new Point(0, 0),
            Size = new Size(80, 20),
            Font = new Font("Segoe UI", 7.5F, FontStyle.Bold),
            ForeColor = p.EsBorrador ? Color.FromArgb(146, 64, 14) : Color.FromArgb(6, 95, 70),
            TextAlign = ContentAlignment.MiddleCenter
        };
        badge.Controls.Add(lblBadge);
        tarjeta.Controls.Add(badge);

        Button btnConsultar = CrearBotonAccion("Consultar", Color.FromArgb(47, 124, 246), 476, 16);
        btnConsultar.Click += (_, _) =>
        {
            SigemVista sv = new(usuario);
            var completo = repositorio.BuscarPorIdentificador(p.Expediente) ?? repositorio.BuscarPorExpediente(p.Expediente);
            if (completo is not null)
            {
                sv.MostrarPaciente(completo);
                sv.SetUsuario(usuario);
            }
            sv.ShowDialog();
            recargar();
        };
        tarjeta.Controls.Add(btnConsultar);

        Button btnEditar = CrearBotonAccion("Editar", Color.FromArgb(245, 158, 11), 568, 16);
        btnEditar.Click += (_, _) => MostrarEditorPaciente(p, recargar);
        tarjeta.Controls.Add(btnEditar);

        Button btnEliminar = CrearBotonAccion("Eliminar", Color.FromArgb(239, 68, 68), 660, 16);
        btnEliminar.Click += (_, _) =>
        {
            string id = !string.IsNullOrWhiteSpace(p.Expediente) ? p.Expediente : p.Curp;
            string nombre = $"{p.Nombre} {p.Apellido}".Trim();
            var confirm = MessageBox.Show(
                $"Eliminar paciente '{nombre}'?\n\nNo se podra recuperar esta informacion.",
                "Confirmar eliminacion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes && repositorio.EliminarPaciente(id))
                recargar();
        };
        tarjeta.Controls.Add(btnEliminar);

        return tarjeta;
    }

    private static Button CrearBotonAccion(string texto, Color color, int x, int y)
    {
        Button btn = new()
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(82, 28),
            FlatStyle = FlatStyle.Flat,
            BackColor = color,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            FlatAppearance = { BorderSize = 0 }
        };
        return btn;
    }

    private void MostrarEditorPaciente(Paciente p, Action recargar)
    {
        Form editor = new()
        {
            Text = "Editar Paciente",
            Size = new Size(520, 440),
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false,
            ShowInTaskbar = false,
            BackColor = Color.White
        };

        Label lblExpediente = new() { Text = "Expediente:", Location = new Point(24, 20), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        TextBox txtExpediente = new() { Text = p.Expediente, Location = new Point(150, 18), Size = new Size(320, 28), Font = new Font("Segoe UI", 10F) };

        Label lblCurp = new() { Text = "CURP:", Location = new Point(24, 58), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        TextBox txtCurp = new() { Text = p.Curp, Location = new Point(150, 56), Size = new Size(320, 28), Font = new Font("Segoe UI", 10F) };

        Label lblNombre = new() { Text = "Nombre:", Location = new Point(24, 96), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        TextBox txtNombre = new() { Text = p.Nombre, Location = new Point(150, 94), Size = new Size(320, 28), Font = new Font("Segoe UI", 10F) };

        Label lblApellido = new() { Text = "Apellido:", Location = new Point(24, 134), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        TextBox txtApellido = new() { Text = p.Apellido, Location = new Point(150, 132), Size = new Size(320, 28), Font = new Font("Segoe UI", 10F) };

        Label lblFechaNac = new() { Text = "Fecha Nac.:", Location = new Point(24, 172), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        DateTimePicker dtpFecha = new() { Value = p.FechaNacimiento == default ? DateTime.Now.AddYears(-30) : p.FechaNacimiento, Location = new Point(150, 170), Size = new Size(180, 28), Font = new Font("Segoe UI", 10F) };

        Label lblSexo = new() { Text = "Sexo:", Location = new Point(24, 210), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        ComboBox cmbSexo = new() { Text = p.Sexo, Location = new Point(150, 208), Size = new Size(180, 28), Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList };
        cmbSexo.Items.AddRange(["Masculino", "Femenino"]);

        Label lblDireccion = new() { Text = "Direccion:", Location = new Point(24, 248), Size = new Size(120, 24), Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
        TextBox txtDireccion = new() { Text = p.Direccion, Location = new Point(150, 246), Size = new Size(320, 28), Font = new Font("Segoe UI", 10F) };

        Button btnGuardar = new()
        {
            Text = "Guardar Cambios",
            Location = new Point(150, 330),
            Size = new Size(160, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(16, 185, 129),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        btnGuardar.FlatAppearance.BorderSize = 0;

        Button btnCancelar = new()
        {
            Text = "Cancelar",
            Location = new Point(320, 330),
            Size = new Size(150, 40),
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(156, 163, 175),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand,
            DialogResult = DialogResult.Cancel
        };
        btnCancelar.FlatAppearance.BorderSize = 0;

        btnGuardar.Click += (_, _) =>
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || string.IsNullOrWhiteSpace(txtApellido.Text))
            {
                MessageBox.Show("Nombre y apellido son obligatorios.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            p.Nombre = txtNombre.Text.Trim();
            p.Apellido = txtApellido.Text.Trim();
            p.Curp = txtCurp.Text.Trim().ToUpperInvariant();
            p.Expediente = txtExpediente.Text.Trim();
            p.FechaNacimiento = dtpFecha.Value;
            p.Sexo = cmbSexo.Text;
            p.Direccion = txtDireccion.Text.Trim();

            repositorio.GuardarPaciente(p);
            editor.Close();
            recargar();
        };

        editor.Controls.AddRange([lblExpediente, txtExpediente, lblCurp, txtCurp, lblNombre, txtNombre, lblApellido, txtApellido,
            lblFechaNac, dtpFecha, lblSexo, cmbSexo, lblDireccion, txtDireccion, btnGuardar, btnCancelar]);
        editor.CancelButton = btnCancelar;
        editor.ShowDialog();
    }

    private void ConstruirFormularioDiagnostico()
    {
        Panel tarjeta = CrearTarjetaContenido(48, 310, 940, 480);
        pnlContenido.Controls.Add(tarjeta);

        tarjeta.Controls.Add(CrearTituloSeccion("Diagnostico", 28, 28));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Diagnostico Principal", 28, 92));
        tarjeta.Controls.Add(CrearTextBox("Ej: Hipertension Arterial", 28, 122, 860, 36));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Sintomas", 28, 180));
        tarjeta.Controls.Add(CrearTextArea("Lista de sintomas separados por comas...", 28, 210, 860, 92));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Observaciones", 28, 324));
        tarjeta.Controls.Add(CrearTextArea("Observaciones medicas adicionales...", 28, 354, 860, 92));
        tarjeta.Controls.Add(CrearBotonPrimario("Guardar Diagnostico", 28, 468, 860, 42));
    }

    private void ConstruirFormularioTratamiento()
    {
        Panel tarjeta = CrearTarjetaContenido(48, 310, 940, 510);
        pnlContenido.Controls.Add(tarjeta);

        tarjeta.Controls.Add(CrearTituloSeccion("Plan de Tratamiento", 28, 28));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Tratamiento Prescrito", 28, 92));
        tarjeta.Controls.Add(CrearTextArea("Descripcion detallada del tratamiento...", 28, 122, 860, 100));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Duracion", 28, 250));
        tarjeta.Controls.Add(CrearTextBox("Ej: 30 dias", 28, 280, 400, 36));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Proxima Cita", 468, 250));
        tarjeta.Controls.Add(CrearTextBox("dd/mm/aaaa", 468, 280, 420, 36));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Indicaciones Especiales", 28, 338));
        tarjeta.Controls.Add(CrearTextArea("Indicaciones adicionales para el paciente...", 28, 368, 860, 90));
        tarjeta.Controls.Add(CrearBotonPrimario("Guardar Tratamiento", 28, 475, 860, 42));
    }

    private void ConstruirMensajeSimple(string titulo, string texto, int y = 220)
    {
        Panel tarjeta = CrearTarjetaContenido(48, y, 940, 190);
        pnlContenido.Controls.Add(tarjeta);
        tarjeta.Controls.Add(CrearTituloSeccion(titulo, 28, 30));
        tarjeta.Controls.Add(CrearTexto(texto, 28, 86, 850, 80, 12F));
    }

    private void ConstruirAdministracion()
    {
        AdministracionControl administracionControl = new()
        {
            Dock = DockStyle.Fill
        };

        pnlContenido.Controls.Add(administracionControl);
        administracionControl.BringToFront();
    }

    private static bool CoincidePaciente(Paciente paciente, string filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro))
            return true;

        string texto = $"{paciente.Expediente} {paciente.Curp} {paciente.Nombre} {paciente.Apellido} {paciente.Sexo} {paciente.Direccion}";
        return texto.Contains(filtro, StringComparison.OrdinalIgnoreCase);
    }

    private static Panel CrearTarjetaPaciente(Paciente paciente)
    {
        Panel tarjeta = new()
        {
            Size = new Size(450, 170),
            BackColor = Color.FromArgb(248, 251, 255),
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 20, 20)
        };

        string nombre = $"{paciente.Nombre} {paciente.Apellido}".Trim();
        if (string.IsNullOrWhiteSpace(nombre))
            nombre = "Paciente sin nombre";

        Label lblNombre = new()
        {
            Text = nombre,
            Location = new Point(24, 26),
            Size = new Size(300, 30),
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            ForeColor = Color.FromArgb(17, 24, 39)
        };
        tarjeta.Controls.Add(lblNombre);

        int edad = paciente.FechaNacimiento == default ? 0 : CalcularEdad(paciente.FechaNacimiento);
        string edadTexto = edad > 0 ? $"{edad} anos" : "Edad no registrada";
        Label datos = new()
        {
            Text = $"{edadTexto}    -    {TextoSeguro(paciente.Sexo, "Sexo no registrado")}    -    {TextoSeguro(paciente.Expediente, "Sin expediente")}",
            Location = new Point(24, 64),
            Size = new Size(390, 25),
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(31, 41, 55)
        };
        tarjeta.Controls.Add(datos);

        Label curp = new()
        {
            Text = $"CURP: {TextoSeguro(paciente.Curp, "No registrada")}",
            Location = new Point(24, 88),
            Size = new Size(380, 24),
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(31, 41, 55)
        };
        tarjeta.Controls.Add(curp);

        Label direccion = new()
        {
            Text = $"Direccion: {TextoSeguro(paciente.Direccion, "No registrada")}",
            Location = new Point(24, 112),
            Size = new Size(380, 24),
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(31, 41, 55)
        };
        tarjeta.Controls.Add(direccion);

        Label historial = new()
        {
            Text = "Historial",
            Location = new Point(338, 30),
            Size = new Size(62, 24),
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = Color.FromArgb(16, 185, 129)
        };
        tarjeta.Controls.Add(historial);

        return tarjeta;
    }

    private static Panel CrearTarjetaVacia(string texto)
    {
        Panel tarjeta = new()
        {
            Size = new Size(450, 120),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Margin = new Padding(0, 0, 20, 20)
        };
        tarjeta.Controls.Add(CrearTexto(texto, 24, 42, 390, 30, 11F));
        return tarjeta;
    }

    private static int CalcularEdad(DateTime fechaNacimiento)
    {
        int edad = DateTime.Today.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad))
            edad--;

        return Math.Max(0, edad);
    }

    private static string TextoSeguro(string? texto, string valorDefault)
    {
        return string.IsNullOrWhiteSpace(texto) ? valorDefault : texto.Trim();
    }

    private static Panel CrearTarjetaContenido(int x, int y, int ancho, int alto)
    {
        return new Panel
        {
            Location = new Point(x, y),
            Size = new Size(ancho, alto),
            Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };
    }

    private static Label CrearTituloSeccion(string texto, int x, int y)
    {
        return new Label
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(780, 44),
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            ForeColor = Color.FromArgb(17, 24, 39)
        };
    }

    private static Label CrearTexto(string texto, int x, int y, int ancho, int alto, float tamano)
    {
        return new Label
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(ancho, alto),
            Font = new Font("Segoe UI", tamano),
            ForeColor = Color.FromArgb(55, 65, 81)
        };
    }

    private static Label CrearEtiquetaCampo(string texto, int x, int y)
    {
        return new Label
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(300, 24),
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            ForeColor = Color.FromArgb(31, 41, 55)
        };
    }

    private static TextBox CrearTextBox(string placeholder, int x, int y, int ancho, int alto)
    {
        return new TextBox
        {
            Location = new Point(x, y),
            Size = new Size(ancho, alto),
            Font = new Font("Segoe UI", 11F),
            PlaceholderText = placeholder
        };
    }

    private static TextBox CrearTextArea(string placeholder, int x, int y, int ancho, int alto)
    {
        return new TextBox
        {
            Location = new Point(x, y),
            Size = new Size(ancho, alto),
            Font = new Font("Segoe UI", 11F),
            PlaceholderText = placeholder,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
    }

    private static Button CrearBotonPrimario(string texto, int x, int y, int ancho, int alto)
    {
        Button boton = new()
        {
            Text = texto,
            Location = new Point(x, y),
            Size = new Size(ancho, alto),
            BackColor = Color.FromArgb(47, 124, 246),
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat,
            Font = new Font("Segoe UI", 10F, FontStyle.Bold),
            Cursor = Cursors.Hand
        };
        boton.FlatAppearance.BorderSize = 0;
        return boton;
    }

    private void CargarPanelPrincipal()
    {
        List<Paciente> pacientes = repositorio.ObtenerTodos();
        int totalSignosVitales = pacientes.Sum(paciente => paciente.SignosVitales.Count);
        int borradores = pacientes.Count(paciente => paciente.EsBorrador);
        int registrosHoy = pacientes.Sum(paciente =>
            paciente.SignosVitales.Count(signos => signos.FechaHora.Date == DateTime.Today));

        lblTotalPacientes.Text = pacientes.Count.ToString();
        lblBorradores.Text = borradores.ToString();
        lblRegistrosVitales.Text = totalSignosVitales.ToString();
        lblRegistrosHoy.Text = registrosHoy.ToString();

        lstPacientesRecientes.DataSource = pacientes
            .OrderByDescending(paciente => paciente.FechaRegistro)
            .Take(5)
            .Select(FormatearPaciente)
            .DefaultIfEmpty("Todavia no hay pacientes registrados.")
            .ToList();

        lstActividadReciente.DataSource = pacientes
            .SelectMany(paciente => paciente.SignosVitales.Select(signos => new
            {
                Paciente = paciente,
                Signos = signos
            }))
            .OrderByDescending(registro => registro.Signos.FechaHora)
            .Take(5)
            .Select(registro => $"{registro.Signos.FechaHora:g} - Signos vitales: {registro.Paciente.Nombre} {registro.Paciente.Apellido}")
            .DefaultIfEmpty("Todavia no hay actividad registrada.")
            .ToList();

        lstBorradores.DataSource = pacientes
            .Where(paciente => paciente.EsBorrador)
            .OrderByDescending(paciente => paciente.FechaRegistro)
            .Take(5)
            .Select(FormatearPaciente)
            .DefaultIfEmpty("No hay borradores pendientes.")
            .ToList();
    }

    private static string FormatearPaciente(Paciente paciente)
    {
        string expediente = string.IsNullOrWhiteSpace(paciente.Expediente) ? "Sin expediente" : paciente.Expediente;
        string curp = string.IsNullOrWhiteSpace(paciente.Curp) ? "Sin CURP" : paciente.Curp;
        string estado = paciente.EsBorrador ? "Borrador" : "Validado";

        return $"{expediente} / {curp} - {paciente.Nombre} {paciente.Apellido} ({estado})";
    }

    // Gestión dinámica de colores. Respeta el verde lima/esmeralda si es Enfermera
    private void SeleccionarBoton(Button botonActivo)
    {
        Button[] botones = [btnPanelPrincipal, btnPacientes, btnConsulta, btnAdministracion];

        Color colorFondoPorRol = (usuario.Rol == RolUsuario.Enfermera)
            ? Color.FromArgb(236, 253, 245)  // Fondo verde lima suave para enfermera
            : Color.White;                    // Fondo blanco para doctor

        foreach (Button boton in botones)
        {
            boton.BackColor = colorFondoPorRol;
            boton.ForeColor = Color.FromArgb(31, 41, 55);
        }

        if (usuario.Rol == RolUsuario.Enfermera)
        {
            botonActivo.BackColor = Color.FromArgb(16, 185, 129); // Verde esmeralda vivo corporativo
            botonActivo.ForeColor = Color.White;
        }
        else
        {
            botonActivo.BackColor = Color.FromArgb(47, 124, 246); // Azul SIGEM original para Doctor
            botonActivo.ForeColor = Color.White;
        }
    }

    private void BtnPanelPrincipal_Click(object sender, EventArgs e)
    {
        MostrarPanelPrincipal();
    }

    private void BtnPacientes_Click(object sender, EventArgs e)
    {
        MostrarGestionPacientes();
    }

    private void BtnConsulta_Click(object sender, EventArgs e)
    {
        MostrarConsultaMedica();
    }

    private void BtnAdministracion_Click(object sender, EventArgs e)
    {
        MostrarAdministracion();
    }

    public bool CierreSesionSolicitado { get; private set; }

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
    {
        CierreSesionSolicitado = true;
        Close();
    }

    private void cardPacientes_Paint(object sender, PaintEventArgs e)
    {

    }

    private void lblCardPacientes_Click(object sender, EventArgs e)
    {

    }
}