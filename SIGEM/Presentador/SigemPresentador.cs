using System.IO;
using SIGEM.Modelo;
using SIGEM.Vista;

namespace SIGEM.Presentador;

public class SigemPresentador
{
    private readonly ISigemVista vista;
    private readonly ISigemRepositorio repositorio;
    private readonly Usuario usuario;
    private Paciente? pacienteActual;
    private bool esNuevoPaciente;

    public SigemPresentador(ISigemVista vista, ISigemRepositorio repositorio, Usuario usuario)
    {
        this.vista = vista;
        this.repositorio = repositorio;
        this.usuario = usuario;

        this.vista.SetUsuario(usuario);

        this.vista.BuscarSolicitado += BuscarPaciente;
        this.vista.GuardarSolicitado += GuardarRegistro;
        this.vista.LimpiarSolicitado += Limpiar;
        this.vista.NuevoPacienteSolicitado += IniciarNuevoPaciente;
        this.vista.ValidarSolicitado += ValidarRegistro;
        this.vista.CalcularSolicitado += Calcular;
        this.vista.RecetaSolicitado += (_, _) => GenerarDocumento("Receta");
        this.vista.NotaEvolucionSolicitado += (_, _) => GenerarDocumento("NotaEvolucion");
        this.vista.HistoriaClinicaSolicitado += (_, _) => GenerarDocumento("HistoriaClinica");
    }

    private void BuscarPaciente(object? sender, EventArgs e)
    {
        string identificador = vista.Expediente;

        if (string.IsNullOrWhiteSpace(identificador))
        {
            vista.MostrarError("Ingrese un número de expediente.");
            return;
        }

        var paciente = repositorio.BuscarPorIdentificador(identificador);

        if (paciente is null)
        {
            vista.MostrarError($"Paciente con CURP o expediente '{identificador}' no encontrado. Use 'Nuevo Paciente' para crearlo.");
            vista.LimpiarFormulario();
            vista.HabilitarCamposPaciente(false);
            vista.HabilitarCamposSignosVitales(false);
            pacienteActual = null;
            esNuevoPaciente = false;
            return;
        }

        pacienteActual = paciente;
        esNuevoPaciente = false;
        vista.MostrarPaciente(paciente);
        vista.MostrarMensaje($"Paciente encontrado: {paciente.Nombre} {paciente.Apellido}");

        vista.MostrarDocumentos(usuario.Rol == RolUsuario.Doctor);
        ActualizarHistorial(paciente);
        MostrarAlertasDelUltimoRegistro(paciente);

        if (usuario.Rol == RolUsuario.Doctor && paciente.EsBorrador)
        {
            vista.MostrarMensaje("Este es un borrador pendiente de validación.");
            vista.MostrarBotonValidar(true);
        }
    }

    private void IniciarNuevoPaciente(object? sender, EventArgs e)
    {
        string identificador = vista.Expediente;

        if (string.IsNullOrWhiteSpace(identificador))
        {
            vista.MostrarError("Ingrese un número de expediente antes de crear un nuevo paciente.");
            return;
        }

        var existente = repositorio.BuscarPorIdentificador(identificador);
        if (existente is not null)
        {
            vista.MostrarPaciente(existente);
            pacienteActual = existente;
            esNuevoPaciente = false;
            vista.MostrarMensaje($"El paciente {existente.Nombre} ya existe. Puede registrar signos vitales.");
            return;
        }

        esNuevoPaciente = true;
        bool identificadorEsCurp = EsCurp(identificador);
        pacienteActual = new Paciente
        {
            Expediente = identificadorEsCurp ? GenerarExpedienteTemporal(identificador) : identificador,
            Curp = identificadorEsCurp ? identificador.ToUpperInvariant() : string.Empty,
            EsBorrador = usuario.Rol == RolUsuario.Enfermera,
            FechaRegistro = DateTime.Now
        };

        vista.EstablecerModoNuevoPaciente();
        if (identificadorEsCurp)
        {
            vista.PacienteCurp = identificador.ToUpperInvariant();
        }

        vista.MostrarMensaje(identificadorEsCurp
            ? $"Ingrese los datos del nuevo paciente (CURP: {identificador.ToUpperInvariant()})."
            : $"Ingrese los datos del nuevo paciente (expediente: {identificador}).");
    }

    private void GuardarRegistro(object? sender, EventArgs e)
    {
        if (pacienteActual is null)
        {
            vista.MostrarError("Debe buscar o crear un paciente primero.");
            return;
        }

        string? error = ValidarCampos();
        if (error is not null)
        {
            vista.MostrarError(error);
            return;
        }

        bool actualizarPendienteEnfermeria = PuedeActualizarUltimoPendiente();
        int indicePendiente = pacienteActual.SignosVitales.Count - 1;
        DateTime fechaRegistro = actualizarPendienteEnfermeria
            ? pacienteActual.SignosVitales[indicePendiente].FechaHora
            : DateTime.Now;

        var sv = new SignosVitales
        {
            FechaHora = fechaRegistro,
            Peso = vista.Peso,
            Estatura = vista.Estatura,
            Temperatura = vista.Temperatura,
            Pulso = vista.Pulso,
            FrecuenciaRespiratoria = vista.FrecuenciaRespiratoria,
            PresionSistolica = vista.PresionSistolica,
            PresionDiastolica = vista.PresionDiastolica,
            CC = vista.CC,
            SaturacionO2 = vista.SaturacionO2,
            RegistradoPor = vista.UsuarioActual,
            Validado = usuario.Rol == RolUsuario.Doctor,
            ValidadoPor = usuario.Rol == RolUsuario.Doctor ? vista.UsuarioActual : null,
            IdDoctor = usuario.Rol == RolUsuario.Doctor ? usuario.IdDoctor : null
        };

        if (esNuevoPaciente)
        {
            pacienteActual.Nombre = vista.PacienteNombre;
            pacienteActual.Apellido = vista.PacienteApellido;
            pacienteActual.Curp = vista.PacienteCurp;
            pacienteActual.FechaNacimiento = vista.PacienteFechaNacimiento;
            pacienteActual.Sexo = vista.PacienteSexo;
            pacienteActual.SignosVitales.Add(sv);
            repositorio.GuardarPaciente(pacienteActual);
        }
        else
        {
            if (actualizarPendienteEnfermeria)
                repositorio.ActualizarSignosVitales(pacienteActual.Expediente, indicePendiente, sv);
            else
                repositorio.AgregarSignosVitales(pacienteActual.Expediente, sv);
        }

        pacienteActual = repositorio.BuscarPorIdentificador(pacienteActual.Curp) ??
                         repositorio.BuscarPorExpediente(pacienteActual.Expediente);

        if (usuario.Rol == RolUsuario.Enfermera)
        {
            vista.MostrarMensaje("Signos vitales guardados como borrador. El doctor los validará.");
        }
        else if (actualizarPendienteEnfermeria)
        {
            vista.MostrarMensaje("Registro de enfermeria corregido y validado por el medico.");
        }
        else
        {
            vista.MostrarMensaje("Registro guardado y validado correctamente.");
        }

        ActualizarHistorial(pacienteActual!);
        MostrarAlertasDelUltimoRegistro(pacienteActual!);
        vista.HabilitarCamposPaciente(false);
    }

    private void ValidarRegistro(object? sender, EventArgs e)
    {
        if (pacienteActual is null || pacienteActual.SignosVitales.Count == 0)
        {
            vista.MostrarError("No hay registros pendientes por validar.");
            return;
        }

        int ultimoIndice = pacienteActual.SignosVitales.Count - 1;
        var ultimoSV = pacienteActual.SignosVitales[ultimoIndice];

        if (ultimoSV.Validado)
        {
            vista.MostrarMensaje("El último registro ya está validado.");
            return;
        }

        repositorio.ValidarRegistro(pacienteActual.Expediente, ultimoIndice, vista.UsuarioActual);
        pacienteActual = repositorio.BuscarPorIdentificador(pacienteActual.Curp) ??
                         repositorio.BuscarPorExpediente(pacienteActual.Expediente);

        vista.MostrarMensaje($"Registro validado por {vista.UsuarioActual}.");
        if (pacienteActual is not null)
            ActualizarHistorial(pacienteActual);
    }

    private void Calcular(object? sender, EventArgs e)
    {
        double peso = vista.Peso;
        double estatura = vista.Estatura;
        int sistolica = vista.PresionSistolica;
        int diastolica = vista.PresionDiastolica;

        double imc = estatura > 0 ? Math.Round(peso / (estatura * estatura), 2) : 0;
        double pam = sistolica > 0 && diastolica > 0
            ? Math.Round(diastolica + (sistolica - diastolica) / 3.0, 0)
            : 0;

        vista.MostrarCalculos(imc, pam);
    }

    private void Limpiar(object? sender, EventArgs e)
    {
        pacienteActual = null;
        esNuevoPaciente = false;
        vista.LimpiarFormulario();
    }

    private bool PuedeActualizarUltimoPendiente()
    {
        return usuario.Rol == RolUsuario.Doctor &&
               !esNuevoPaciente &&
               pacienteActual is not null &&
               pacienteActual.SignosVitales.Count > 0 &&
               !pacienteActual.SignosVitales[^1].Validado;
    }

    private string? ValidarCampos()
    {
        if (esNuevoPaciente)
        {
            if (string.IsNullOrWhiteSpace(vista.PacienteNombre))
                return "El nombre del paciente es obligatorio.";
            if (string.IsNullOrWhiteSpace(vista.PacienteApellido))
                return "El apellido del paciente es obligatorio.";
            if (string.IsNullOrWhiteSpace(vista.PacienteCurp))
                return "La CURP del paciente es obligatoria.";
            if (vista.PacienteCurp.Length != 18)
                return "La CURP debe tener 18 caracteres.";
            if (string.IsNullOrWhiteSpace(vista.PacienteSexo))
                return "El sexo del paciente es obligatorio.";
        }

        if (vista.Peso <= 0) return "El peso debe ser mayor a 0.";
        if (vista.Estatura <= 0) return "La estatura debe ser mayor a 0.";
        if (vista.Temperatura <= 0) return "La temperatura debe ser mayor a 0.";
        if (vista.Pulso <= 0) return "El pulso debe ser mayor a 0.";
        if (vista.FrecuenciaRespiratoria <= 0) return "La frecuencia respiratoria debe ser mayor a 0.";
        if (vista.PresionSistolica <= 0) return "La presión sistólica debe ser mayor a 0.";
        if (vista.PresionDiastolica <= 0) return "La presión diastólica debe ser mayor a 0.";
        if (vista.CC <= 0) return "La CC debe ser mayor a 0.";
        if (vista.SaturacionO2 <= 0) return "La saturación de O2 debe ser mayor a 0.";

        return null;
    }

    private void ActualizarHistorial(Paciente paciente)
    {
        var signosOrdenados = paciente.SignosVitales
            .OrderByDescending(sv => sv.FechaHora)
            .ToList();

        var registros = new List<string>();
        if (signosOrdenados.Count > 0)
        {
            registros.Add("Formato Excel: NOTA_DE_EVOLUCION AM.xls");
            registros.AddRange(signosOrdenados.Select((sv, i) => FormatearRegistroNotaEvolucion(i + 1, sv)));
            registros.Add(string.Empty);
            registros.Add("Formato Excel: HISTORIA_CLINICA_GENERAL AM.xls - resumen del ultimo registro");
            registros.Add(FormatearRegistroHistoriaClinica(signosOrdenados[0]));
        }

        vista.MostrarHistorial(registros);
    }

    private void MostrarAlertasDelUltimoRegistro(Paciente paciente)
    {
        var ultimo = paciente.SignosVitales.OrderByDescending(sv => sv.FechaHora).FirstOrDefault();
        if (ultimo is null)
        {
            vista.MostrarAlertasSignosVitales([]);
            return;
        }

        var alertas = SignosVitalesVisualizacion.CrearAlertas(ultimo)
            .Select(alerta => $"{alerta.Campo}: {alerta.Valor}")
            .ToList();
        vista.MostrarAlertasSignosVitales(alertas);
    }

    private static string FormatearRegistroNotaEvolucion(int indice, SignosVitales signos)
    {
        string datos = string.Join(" | ", SignosVitalesVisualizacion
            .CrearFilas(signos, FormatoSignosVitales.NotaEvolucion)
            .Select(fila => $"{fila.Etiqueta}: {fila.Valor}"));

        return $"#{indice} - {datos}";
    }

    private static string FormatearRegistroHistoriaClinica(SignosVitales signos)
    {
        return string.Join(" | ", SignosVitalesVisualizacion
            .CrearFilas(signos, FormatoSignosVitales.HistoriaClinica)
            .Select(fila => $"{fila.Etiqueta}: {fila.Valor}"));
    }

    private static bool EsCurp(string identificador)
    {
        return identificador.Trim().Length == 18;
    }

    private static string GenerarExpedienteTemporal(string curp)
    {
        string sufijo = curp.Length >= 4 ? curp[^4..] : curp;
        return $"EXP-{DateTime.Now:yyyyMMddHHmmss}-{sufijo}".ToUpperInvariant();
    }

    private void GenerarDocumento(string tipo)
    {
        if (pacienteActual is null)
        {
            vista.MostrarError("Debe buscar o crear un paciente primero.");
            return;
        }

        string carpeta = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "SIGEM", "Documentos");

        string formato = vista.FormatoDocumento;
        bool generarPdf = formato is "PDF" or "PDF + DOCX";
        bool generarDocx = formato is "DOCX" or "PDF + DOCX";

        try
        {
            DocumentoClinicoGenerado resultado = tipo switch
            {
                "Receta" => GeneradorDocumentosClinicos.GenerarReceta(pacienteActual, usuario, carpeta),
                "NotaEvolucion" => GeneradorDocumentosClinicos.GenerarNotaEvolucion(pacienteActual, usuario, carpeta),
                "HistoriaClinica" => GeneradorDocumentosClinicos.GenerarHistoriaClinica(pacienteActual, usuario, carpeta),
                _ => throw new ArgumentException($"Tipo de documento desconocido: {tipo}")
            };

            var generados = new List<string>();
            if (generarDocx) { generados.Add(resultado.RutaDocx); }
            if (generarPdf) { generados.Add(resultado.RutaPdf); }

            string mensaje = "Documentos generados:\n" + string.Join("\n", generados.Select(r => $"  - {r}"));
            vista.MostrarMensaje($"Documento(s) generado(s) correctamente.");
            MessageBox.Show(mensaje, "SIGEM - Documentos", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            vista.MostrarError($"Error al generar {tipo}: {ex.Message}");
        }
    }
}
