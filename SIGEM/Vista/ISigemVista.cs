using SIGEM.Modelo;

namespace SIGEM.Vista;

public interface ISigemVista
{
    event EventHandler? BuscarSolicitado;
    event EventHandler? GuardarSolicitado;
    event EventHandler? LimpiarSolicitado;
    event EventHandler? NuevoPacienteSolicitado;
    event EventHandler? ValidarSolicitado;
    event EventHandler? CalcularSolicitado;

    string Expediente { get; }
    string PacienteNombre { get; set; }
    string PacienteApellido { get; set; }
    DateTime PacienteFechaNacimiento { get; set; }
    string PacienteSexo { get; set; }
    string PacienteTelefono { get; set; }
    double Peso { get; }
    double Estatura { get; }
    double Temperatura { get; }
    int Pulso { get; }
    int FrecuenciaRespiratoria { get; }
    int PresionSistolica { get; }
    int PresionDiastolica { get; }
    double CC { get; }
    double SaturacionO2 { get; }
    string UsuarioActual { get; }
    RolUsuario RolActual { get; }

    void MostrarPaciente(Paciente paciente);
    void MostrarCalculos(double imc, double pam);
    void MostrarHistorial(IReadOnlyList<string> registros);
    void MostrarMensaje(string mensaje);
    void MostrarError(string mensaje);
    void LimpiarFormulario();
    void HabilitarCamposPaciente(bool habilitar);
    void HabilitarCamposSignosVitales(bool habilitar);
    void MostrarBotonValidar(bool visible);
    void EstablecerModoNuevoPaciente();
    void SetUsuario(Usuario usuario);
}
