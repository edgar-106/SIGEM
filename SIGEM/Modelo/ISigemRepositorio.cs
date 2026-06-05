namespace SIGEM.Modelo;

public interface ISigemRepositorio
{
    Paciente? BuscarPorExpediente(string expediente);
    Paciente? BuscarPorCurp(string curp);
    Paciente? BuscarPorIdentificador(string identificador);
    List<Paciente> ObtenerTodos();
    void GuardarPaciente(Paciente paciente);
    void AgregarSignosVitales(string expediente, SignosVitales sv);
    void ValidarRegistro(string expediente, int indiceSignoVital, string validadoPor);
    List<Paciente> ObtenerBorradores();
}
