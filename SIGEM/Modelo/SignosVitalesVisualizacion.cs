namespace SIGEM.Modelo;

public static class SignosVitalesVisualizacion
{
    public static IEnumerable<(string Etiqueta, string Valor)> CrearFilas(SignosVitales signos, FormatoSignosVitales formato)
    {
        return formato == FormatoSignosVitales.HistoriaClinica
            ? CrearHistoriaClinica(signos)
            : CrearNotaEvolucion(signos);
    }

    public static IEnumerable<AlertaSignosVitales> CrearAlertas(SignosVitales signos)
    {
        if (signos.Temperatura is > 0 and (< 36.0 or > 37.5))
            yield return new("Temperatura", $"{signos.Temperatura:F1} C", "Temperatura fuera de rango esperado (36.0-37.5 C).");

        if (signos.Pulso is > 0 and (< 60 or > 100))
            yield return new("Pulso/F.C.", signos.Pulso.ToString(), "Pulso fuera de rango esperado (60-100 lpm).");

        if (signos.FrecuenciaRespiratoria is > 0 and (< 12 or > 20))
            yield return new("Frecuencia respiratoria", signos.FrecuenciaRespiratoria.ToString(), "Frecuencia respiratoria fuera de rango esperado (12-20 rpm).");

        if (signos.PresionSistolica is > 0 and (< 90 or > 140))
            yield return new("Presion sistolica", signos.PresionSistolica.ToString(), "Presion sistolica fuera de rango esperado (90-140 mmHg).");

        if (signos.PresionDiastolica is > 0 and (< 60 or > 90))
            yield return new("Presion diastolica", signos.PresionDiastolica.ToString(), "Presion diastolica fuera de rango esperado (60-90 mmHg).");

        if (signos.SaturacionO2 is > 0 and < 95)
            yield return new("Saturacion O2", $"{signos.SaturacionO2:F0}%", "Saturacion de oxigeno menor a 95%.");
    }

    private static IEnumerable<(string Etiqueta, string Valor)> CrearNotaEvolucion(SignosVitales signos)
    {
        yield return ("Fecha", signos.FechaHora.ToString("dd/MM/yyyy"));
        yield return ("Hora", signos.FechaHora.ToString("HH:mm"));
        yield return ("Peso", $"{signos.Peso:F1} kg");
        yield return ("Estatura/talla", $"{signos.Estatura:F2} m");
        yield return ("Temperatura", $"{signos.Temperatura:F1} C");
        yield return ("Pulso/F.C.", signos.Pulso.ToString());
        yield return ("Frecuencia respiratoria", signos.FrecuenciaRespiratoria.ToString());
        yield return ("Presion arterial", $"{signos.PresionSistolica}/{signos.PresionDiastolica} mmHg");
        yield return ("Saturacion O2", $"{signos.SaturacionO2:F0}%");
        yield return ("IMC", signos.IMC > 0 ? signos.IMC.ToString("F2") : "--");
        yield return ("PAM", signos.PAM > 0 ? signos.PAM.ToString("F0") : "--");
        yield return ("Estado", signos.Validado ? "Validado" : "Pendiente");
        yield return ("Capturado por", string.IsNullOrWhiteSpace(signos.RegistradoPor) ? "--" : signos.RegistradoPor);
        yield return ("Validado por", string.IsNullOrWhiteSpace(signos.ValidadoPor) ? "--" : signos.ValidadoPor);
    }

    private static IEnumerable<(string Etiqueta, string Valor)> CrearHistoriaClinica(SignosVitales signos)
    {
        yield return ("Fecha de atencion", signos.FechaHora.ToString("dd/MM/yyyy"));
        yield return ("Peso", $"{signos.Peso:F1} kg");
        yield return ("Temperatura", $"{signos.Temperatura:F1} C");
        yield return ("Talla/estatura", $"{signos.Estatura:F2} m");
        yield return ("IMC", signos.IMC > 0 ? signos.IMC.ToString("F2") : "--");
        yield return ("PAM", signos.PAM > 0 ? signos.PAM.ToString("F0") : "--");
        yield return ("Estado", signos.Validado ? "Validado" : "Pendiente");
    }
}
