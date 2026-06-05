namespace SIGEM.Modelo;

public class Paciente
{
    public string Expediente { get; set; } = string.Empty;
    public string Curp { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string Sexo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public bool EsBorrador { get; set; }
    public DateTime FechaRegistro { get; set; }
    public List<SignosVitales> SignosVitales { get; set; } = [];
}
