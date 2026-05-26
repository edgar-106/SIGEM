namespace SIGEM.Modelo;

public class SignosVitales
{
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public double Peso { get; set; }
    public double Estatura { get; set; }
    public double Temperatura { get; set; }
    public int Pulso { get; set; }
    public int FrecuenciaRespiratoria { get; set; }
    public int PresionSistolica { get; set; }
    public int PresionDiastolica { get; set; }
    public double CC { get; set; }
    public double SaturacionO2 { get; set; }

    public double IMC => Estatura > 0 ? Math.Round(Peso / (Estatura * Estatura), 2) : 0;

    public double PAM => Math.Round(PresionDiastolica + (PresionSistolica - PresionDiastolica) / 3.0, 0);

    public string RegistradoPor { get; set; } = string.Empty;
    public bool Validado { get; set; }
    public string? ValidadoPor { get; set; }

}
