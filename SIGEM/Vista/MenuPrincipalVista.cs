using SIGEM.Modelo;

namespace SIGEM.Vista;


public partial class MenuPrincipalVista : Form
{
    private readonly Usuario usuario;
    private readonly PermisosRol permisos;
    private readonly ISigemRepositorio repositorio;
    private Form? formularioEmbebido;
    private string subSeccionConsulta = "Nueva Consulta";

    public MenuPrincipalVista(Usuario usuario)
    {
        this.usuario = usuario;
        repositorio = ConexionBD.CrearRepositorio();
        permisos = PermisosRol.Para(usuario.Rol);
        InitializeComponent();

        lblUsuario.Text = $"Usuario: {usuario.NombreCompleto} ({ObtenerRolTexto(usuario.Rol)})";

        VerificarPermisosPorRol();

        MostrarPanelPrincipal();
        // Solo el administrador ve el boton
    }

    // Nuevo método para ocultar botones del menú y cambiar colores a Verde Lima según el rol
    private void VerificarPermisosPorRol()
    {
        btnPanelPrincipal.Visible = true;
        btnPacientes.Visible = permisos.PuedeVerPacientes;
        btnConsulta.Visible = permisos.PuedeVerSignosVitales;
        btnAdministracion.Visible = permisos.PuedeAdministrarSistema;

        Color fondoMenu = usuario.Rol == RolUsuario.Enfermera
            ? Color.FromArgb(236, 253, 245)
            : Color.White;
        Color marca = usuario.Rol == RolUsuario.Enfermera
            ? Color.FromArgb(16, 185, 129)
            : Color.FromArgb(47, 124, 246);

        pnlMenu.BackColor = fondoMenu;
        pnlMarca.BackColor = marca;

        foreach (Button boton in new[] { btnPanelPrincipal, btnPacientes, btnConsulta, btnAdministracion })
        {
            boton.BackColor = fondoMenu;
            boton.ForeColor = Color.FromArgb(31, 41, 55);
        }
    }

    private static string ObtenerRolTexto(RolUsuario rol) => rol switch
    {
        RolUsuario.Doctor => "Doctor",
        RolUsuario.Enfermera => "Enfermera",
        RolUsuario.Recepcionista => "Recepcionista",
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
        if (!permisos.PuedeVerPacientes)
        {
            MessageBox.Show("Tu rol no tiene permiso para gestionar pacientes.", "Permisos SIGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        MostrarContenido("Gestion de Pacientes", "Administracion completa de expedientes medicos");
        SeleccionarBoton(btnPacientes);
        ConstruirGestionPacientes();
    }

    private void MostrarConsultaMedica()
    {
        if (!permisos.PuedeVerSignosVitales)
        {
            MessageBox.Show("Tu rol no tiene permiso para consultar signos vitales.", "Permisos SIGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        MostrarContenido("Consulta Medica", "Gestion de consultas, diagnosticos y tratamientos");
        SeleccionarBoton(btnConsulta);
        ConstruirConsultaMedica();
    }

    private void MostrarAdministracion()
    {
        if (!permisos.PuedeAdministrarSistema)
        {
            MessageBox.Show("Tu rol no tiene permiso para administrar el sistema.", "Permisos SIGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        LimpiarContenido();
        SeleccionarBoton(btnAdministracion);

        var adminControl = new AdministracionControl(usuario);
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

        cardPacientes.Visible = permisos.PuedeVerPacientes;
        pnlPacientesRecientes.Visible = permisos.PuedeVerPacientes;
        cardBorradores.Visible = permisos.PuedeVerPacientes;
        pnlBorradores.Visible = permisos.PuedeVerPacientes;
        cardSignos.Visible = permisos.PuedeVerSignosVitales;
        cardHoy.Visible = permisos.PuedeVerSignosVitales;
        pnlActividadReciente.Visible = permisos.PuedeVerSignosVitales;

        if (permisos.PuedeAdministrarSistema && !permisos.PuedeVerPacientes)
            ConstruirMensajeSimple("Administracion del Sistema", "Usa el menu lateral para administrar usuarios y configuracion tecnica de SIGEM.", 200);
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
            ("Nueva Consulta", subSeccionConsulta == "Nueva Consulta")
        };

        if (usuario.Rol == RolUsuario.Doctor)
        {
            listaTabs.Add(("Receta Medica", subSeccionConsulta == "Receta Medica"));
            listaTabs.Add(("Diagnostico", subSeccionConsulta == "Diagnostico"));
            listaTabs.Add(("Tratamiento", subSeccionConsulta == "Tratamiento"));
        }

        if (usuario.Rol != RolUsuario.Doctor && subSeccionConsulta is "Receta Medica" or "Diagnostico" or "Tratamiento")
            subSeccionConsulta = "Nueva Consulta";

        FlowLayoutPanel tabs = CrearTabs(200, listaTabs.ToArray());
        pnlContenido.Controls.Add(tabs);

        switch (subSeccionConsulta)
        {
            case "Receta Medica":
                if (usuario.Rol == RolUsuario.Doctor)
                    ConstruirFormularioRecetaMedica();
                else
                    SeleccionarConsulta("Nueva Consulta");
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

    private void ConstruirFormularioRecetaMedica()
    {
        Panel tarjeta = CrearTarjetaContenido(48, 310, 940, 500);
        pnlContenido.Controls.Add(tarjeta);

        tarjeta.Controls.Add(CrearTituloSeccion("Emitir Receta Medica", 28, 28));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Buscar Paciente (CURP o Expediente)", 28, 92));
        TextBox txtBuscar = CrearTextBox("Ingrese CURP o expediente...", 28, 122, 400, 36);
        tarjeta.Controls.Add(txtBuscar);

        Label lblInfo = CrearTexto("Ingrese identificador del paciente y presione Buscar", 450, 122, 460, 36, 10F);
        lblInfo.ForeColor = Color.FromArgb(107, 114, 128);
        tarjeta.Controls.Add(lblInfo);

        Label lblPacienteInfo = CrearTexto(string.Empty, 28, 170, 860, 24, 12F);
        lblPacienteInfo.ForeColor = Color.FromArgb(31, 41, 55);
        tarjeta.Controls.Add(lblPacienteInfo);

        Button btnBuscar = CrearBotonPrimario("Buscar Paciente", 28, 220, 200, 40);
        btnBuscar.BackColor = Color.FromArgb(47, 124, 246);
        tarjeta.Controls.Add(btnBuscar);

        Button btnReceta = CrearBotonPrimario("Generar Receta Medica (DOCX + PDF)", 260, 220, 300, 40);
        btnReceta.BackColor = Color.FromArgb(124, 58, 237);
        btnReceta.Enabled = false;
        tarjeta.Controls.Add(btnReceta);

        Button btnNota = CrearBotonPrimario("Generar Nota Evolucion (DOCX + PDF)", 580, 220, 310, 40);
        btnNota.BackColor = Color.FromArgb(217, 119, 6);
        btnNota.Enabled = false;
        tarjeta.Controls.Add(btnNota);

        Button btnHistoria = CrearBotonPrimario("Generar Historial Clinico (DOCX + PDF)", 260, 280, 310, 40);
        btnHistoria.BackColor = Color.FromArgb(220, 38, 38);
        btnHistoria.Enabled = false;
        tarjeta.Controls.Add(btnHistoria);

        Button btnValidarDoc = CrearBotonPrimario("Validar Borrador + Generar Documentos", 580, 280, 310, 40);
        btnValidarDoc.BackColor = Color.FromArgb(16, 185, 129);
        btnValidarDoc.Enabled = false;
        tarjeta.Controls.Add(btnValidarDoc);

        Paciente? pacienteSeleccionado = null;

        btnBuscar.Click += (_, _) =>
        {
            string id = txtBuscar.Text.Trim();
            if (string.IsNullOrWhiteSpace(id))
            {
                lblPacienteInfo.Text = "Ingrese un CURP o expediente.";
                lblPacienteInfo.ForeColor = Color.FromArgb(184, 54, 54);
                return;
            }

            var p = repositorio.BuscarPorIdentificador(id) ?? repositorio.BuscarPorExpediente(id);
            if (p is null)
            {
                lblPacienteInfo.Text = $"Paciente '{id}' no encontrado.";
                lblPacienteInfo.ForeColor = Color.FromArgb(184, 54, 54);
                pacienteSeleccionado = null;
                btnReceta.Enabled = false;
                btnNota.Enabled = false;
                btnHistoria.Enabled = false;
                btnValidarDoc.Enabled = false;
                return;
            }

            pacienteSeleccionado = p;
            string estado = p.EsBorrador ? "BORRADOR" : "VALIDADO";
            lblPacienteInfo.Text = $"Paciente: {p.Nombre} {p.Apellido} | CURP: {p.Curp} | Exp: {p.Expediente} | Estado: {estado}";
            lblPacienteInfo.ForeColor = Color.FromArgb(31, 41, 55);
            btnReceta.Enabled = true;
            btnNota.Enabled = p.SignosVitales.Count > 0;
            btnHistoria.Enabled = p.SignosVitales.Count > 0;
            btnValidarDoc.Enabled = p.EsBorrador && p.SignosVitales.Count > 0;
        };

        txtBuscar.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnBuscar.PerformClick();
                e.SuppressKeyPress = true;
            }
        };

        btnReceta.Click += (_, _) =>
        {
            if (pacienteSeleccionado is null) return;
            try
            {
                string carpeta = Path.Combine(AppContext.BaseDirectory, "Documentos");
                var res = GeneradorDocumentosClinicos.GenerarReceta(pacienteSeleccionado, usuario, carpeta);
                MessageBox.Show($"Receta generada:\nDOCX: {res.RutaDocx}\nPDF: {res.RutaPdf}", "Documento Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{res.RutaDocx}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        btnNota.Click += (_, _) =>
        {
            if (pacienteSeleccionado is null) return;
            try
            {
                string carpeta = Path.Combine(AppContext.BaseDirectory, "Documentos");
                var res = GeneradorDocumentosClinicos.GenerarNotaEvolucion(pacienteSeleccionado, usuario, carpeta);
                MessageBox.Show($"Nota de evolucion generada:\nDOCX: {res.RutaDocx}\nPDF: {res.RutaPdf}", "Documento Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{res.RutaDocx}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        btnHistoria.Click += (_, _) =>
        {
            if (pacienteSeleccionado is null) return;
            try
            {
                string carpeta = Path.Combine(AppContext.BaseDirectory, "Documentos");
                var res = GeneradorDocumentosClinicos.GenerarHistoriaClinica(pacienteSeleccionado, usuario, carpeta);
                MessageBox.Show($"Historia clinica generada:\nDOCX: {res.RutaDocx}\nPDF: {res.RutaPdf}", "Documento Generado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{res.RutaDocx}\"");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        };

        btnValidarDoc.Click += (_, _) =>
        {
            if (pacienteSeleccionado is null || pacienteSeleccionado.SignosVitales.Count == 0) return;

            int idx = pacienteSeleccionado.SignosVitales.Count - 1;
            var ultimo = pacienteSeleccionado.SignosVitales[idx];
            if (!ultimo.Validado && !string.IsNullOrWhiteSpace(pacienteSeleccionado.Expediente))
            {
                repositorio.ValidarRegistro(pacienteSeleccionado.Expediente, idx, usuario.NombreCompleto);
                pacienteSeleccionado = repositorio.BuscarPorExpediente(pacienteSeleccionado.Expediente) ?? pacienteSeleccionado;
                string estado = pacienteSeleccionado.EsBorrador ? "BORRADOR" : "VALIDADO";
                lblPacienteInfo.Text = $"Paciente: {pacienteSeleccionado.Nombre} {pacienteSeleccionado.Apellido} | Estado: {estado}";
                btnValidarDoc.Enabled = false;
                MessageBox.Show("Registro validado. Ahora puede generar documentos.", "Validado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        };
    }

    private void ConstruirFormularioDiagnostico()
    {
        Panel tarjeta = CrearTarjetaContenido(48, 310, 940, 480);
        pnlContenido.Controls.Add(tarjeta);

        TextBox txtDiagnostico = CrearTextBox("Ej: Hipertension Arterial", 28, 122, 860, 36);
        TextBox txtSintomas = CrearTextArea("Lista de sintomas separados por comas...", 28, 210, 860, 92);
        TextBox txtObservaciones = CrearTextArea("Observaciones medicas adicionales...", 28, 354, 860, 92);
        Label lblInfo = CrearTexto(string.Empty, 28, 424, 860, 24, 10F);
        lblInfo.ForeColor = Color.FromArgb(31, 41, 55);

        tarjeta.Controls.Add(CrearTituloSeccion("Diagnostico", 28, 28));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Diagnostico Principal", 28, 92));
        tarjeta.Controls.Add(txtDiagnostico);
        tarjeta.Controls.Add(CrearEtiquetaCampo("Sintomas", 28, 180));
        tarjeta.Controls.Add(txtSintomas);
        tarjeta.Controls.Add(CrearEtiquetaCampo("Observaciones", 28, 324));
        tarjeta.Controls.Add(txtObservaciones);
        tarjeta.Controls.Add(lblInfo);
        Button btnGuardar = CrearBotonPrimario("Guardar Diagnostico", 28, 450, 860, 42);
        tarjeta.Controls.Add(btnGuardar);

        btnGuardar.Click += (_, _) =>
        {
            string diagnostico = txtDiagnostico.Text.Trim();
            string sintomas = txtSintomas.Text.Trim();
            string observaciones = txtObservaciones.Text.Trim();

            if (string.IsNullOrWhiteSpace(diagnostico))
            {
                MessageBox.Show("El diagnostico principal es obligatorio.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string archivo = Path.Combine(AppContext.BaseDirectory, "Datos", "ims", "diagnosticos.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(archivo)!);
            System.IO.File.AppendAllText(archivo,
                $"[{DateTime.Now:dd/MM/yyyy HH:mm}] {usuario.NombreCompleto}\n" +
                $"Diagnostico: {diagnostico}\nSintomas: {sintomas}\nObservaciones: {observaciones}\n---\n");
            lblInfo.Text = "Diagnostico guardado correctamente.";
            lblInfo.ForeColor = Color.FromArgb(16, 185, 129);
        };
    }

    private void ConstruirFormularioTratamiento()
    {
        Panel tarjeta = CrearTarjetaContenido(48, 310, 940, 510);
        pnlContenido.Controls.Add(tarjeta);

        TextBox txtTratamiento = CrearTextArea("Descripcion detallada del tratamiento...", 28, 122, 860, 100);
        TextBox txtDuracion = CrearTextBox("Ej: 30 dias", 28, 280, 400, 36);
        TextBox txtProximaCita = CrearTextBox("dd/mm/aaaa", 468, 280, 420, 36);
        TextBox txtIndicaciones = CrearTextArea("Indicaciones adicionales para el paciente...", 28, 368, 860, 90);
        Label lblInfo = CrearTexto(string.Empty, 28, 440, 860, 24, 10F);
        lblInfo.ForeColor = Color.FromArgb(31, 41, 55);

        tarjeta.Controls.Add(CrearTituloSeccion("Plan de Tratamiento", 28, 28));
        tarjeta.Controls.Add(CrearEtiquetaCampo("Tratamiento Prescrito", 28, 92));
        tarjeta.Controls.Add(txtTratamiento);
        tarjeta.Controls.Add(CrearEtiquetaCampo("Duracion", 28, 250));
        tarjeta.Controls.Add(txtDuracion);
        tarjeta.Controls.Add(CrearEtiquetaCampo("Proxima Cita", 468, 250));
        tarjeta.Controls.Add(txtProximaCita);
        tarjeta.Controls.Add(CrearEtiquetaCampo("Indicaciones Especiales", 28, 338));
        tarjeta.Controls.Add(txtIndicaciones);
        tarjeta.Controls.Add(lblInfo);
        Button btnGuardar = CrearBotonPrimario("Guardar Tratamiento", 28, 470, 860, 42);
        tarjeta.Controls.Add(btnGuardar);

        btnGuardar.Click += (_, _) =>
        {
            string tratamiento = txtTratamiento.Text.Trim();
            if (string.IsNullOrWhiteSpace(tratamiento))
            {
                MessageBox.Show("La descripcion del tratamiento es obligatoria.", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string archivo = Path.Combine(AppContext.BaseDirectory, "Datos", "ims", "tratamientos.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(archivo)!);
            System.IO.File.AppendAllText(archivo,
                $"[{DateTime.Now:dd/MM/yyyy HH:mm}] {usuario.NombreCompleto}\n" +
                $"Tratamiento: {tratamiento}\nDuracion: {txtDuracion.Text.Trim()}\nProxima cita: {txtProximaCita.Text.Trim()}\nIndicaciones: {txtIndicaciones.Text.Trim()}\n---\n");
            lblInfo.Text = "Tratamiento guardado correctamente.";
            lblInfo.ForeColor = Color.FromArgb(16, 185, 129);
        };
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
        AdministracionControl administracionControl = new(usuario)
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
