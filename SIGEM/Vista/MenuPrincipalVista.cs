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
        MostrarPanelPrincipal();
    }

    private static string ObtenerRolTexto(RolUsuario rol) => rol switch
    {
        RolUsuario.Doctor => "Doctor",
        RolUsuario.Enfermera => "Enfermera",
        _ => "Usuario"
    };

    private void MostrarContenido(string titulo, string subtitulo)
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
        MostrarContenido("Administracion del Sistema", "Configuracion del sistema");
        SeleccionarBoton(btnAdministracion);
        ConstruirMensajeSimple("Administracion del Sistema", "Las opciones administrativas se mostraran aqui.");
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

        cardPacientes.Visible = true;
        cardBorradores.Visible = true;
        cardSignos.Visible = true;
        cardHoy.Visible = true;
        pnlPacientesRecientes.Visible = true;
        pnlActividadReciente.Visible = true;
        pnlBorradores.Visible = true;
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
        FlowLayoutPanel tabs = CrearTabs(200, [
            ("Nueva Consulta", subSeccionConsulta == "Nueva Consulta"),
            ("Receta Medica", subSeccionConsulta == "Receta Medica"),
            ("Diagnostico", subSeccionConsulta == "Diagnostico"),
            ("Tratamiento", subSeccionConsulta == "Tratamiento")
        ]);
        pnlContenido.Controls.Add(tabs);

        switch (subSeccionConsulta)
        {
            case "Receta Medica":
                ConstruirMensajeSimple("Emitir Receta Medica", "Esta funcionalidad se encuentra en la seccion de gestion de pacientes. Selecciona un paciente para emitir una receta medica.", 330);
                break;
            case "Diagnostico":
                ConstruirFormularioDiagnostico();
                break;
            case "Tratamiento":
                ConstruirFormularioTratamiento();
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

        foreach ((string texto, bool activo) in tabs)
        {
            Button boton = new()
            {
                Text = texto,
                Size = new Size(Math.Max(170, texto.Length * 11 + 52), 46),
                Margin = new Padding(0, 0, 10, 0),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                BackColor = activo ? Color.FromArgb(47, 124, 246) : Color.FromArgb(243, 244, 246),
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

        SigemVista vistaSignos = new(usuario)
        {
            TopLevel = false,
            FormBorderStyle = FormBorderStyle.None,
            Dock = DockStyle.Fill
        };
        formularioEmbebido = vistaSignos;
        contenedor.Controls.Add(vistaSignos);
        vistaSignos.Show();
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

    private static bool CoincidePaciente(Paciente paciente, string filtro)
    {
        if (string.IsNullOrWhiteSpace(filtro))
            return true;

        string texto = $"{paciente.Expediente} {paciente.Nombre} {paciente.Apellido} {paciente.Telefono} {paciente.Sexo} {paciente.Direccion}";
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

        Label telefono = new()
        {
            Text = $"Tel: {TextoSeguro(paciente.Telefono, "No registrado")}",
            Location = new Point(24, 102),
            Size = new Size(380, 24),
            Font = new Font("Segoe UI", 10F),
            ForeColor = Color.FromArgb(31, 41, 55)
        };
        tarjeta.Controls.Add(telefono);

        Label direccion = new()
        {
            Text = $"Direccion: {TextoSeguro(paciente.Direccion, "No registrada")}",
            Location = new Point(24, 132),
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
        string estado = paciente.EsBorrador ? "Borrador" : "Validado";

        return $"{expediente} - {paciente.Nombre} {paciente.Apellido} ({estado})";
    }

    private void SeleccionarBoton(Button botonActivo)
    {
        Button[] botones = [btnPanelPrincipal, btnPacientes, btnConsulta, btnAdministracion];

        foreach (Button boton in botones)
        {
            boton.BackColor = Color.White;
            boton.ForeColor = Color.FromArgb(31, 41, 55);
        }

        botonActivo.BackColor = Color.FromArgb(47, 124, 246);
        botonActivo.ForeColor = Color.White;
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

    private void BtnCerrarSesion_Click(object sender, EventArgs e)
    {
        LoginVista login = new();
        login.Show();
        Close();
    }

    private void cardPacientes_Paint(object sender, PaintEventArgs e)
    {

    }

    private void lblCardPacientes_Click(object sender, EventArgs e)
    {

    }
}
