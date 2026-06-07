using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace SIGEM.Modelo;

public sealed record DocumentoClinicoGenerado(string RutaDocx, string RutaPdf);

public static class GeneradorDocumentosClinicos
{
    public static DocumentoClinicoGenerado GenerarReceta(Paciente paciente, Usuario usuario, string carpetaBase)
    {
        var lineas = CrearEncabezado("RECETA MEDICA", paciente, usuario);
        lineas.AddRange(
        [
            $"Fecha: {DateTime.Today:dd/MM/yyyy}",
            "Servicio: Consulta externa",
            "Diagnostico: ______________________________",
            "Alergias: ______________________________",
            "",
            "Medicamentos",
            "Clave | Cantidad prescrita | Nombre generico | Presentacion | Concentracion | Indicaciones",
            "______ | __________________ | _______________ | ____________ | _____________ | ______________________________",
            "",
            $"Medico: {usuario.NombreCompleto}",
            "Cedula profesional: ______________________________",
            "Firma: ______________________________"
        ]);

        return Generar("receta-medica", lineas, paciente, carpetaBase);
    }

    public static DocumentoClinicoGenerado GenerarNotaEvolucion(Paciente paciente, Usuario usuario, string carpetaBase)
    {
        var lineas = CrearEncabezado("NOTA DE EVOLUCION", paciente, usuario);
        SignosVitales? signos = ObtenerUltimosSignos(paciente);

        if (signos is null)
        {
            lineas.Add("Sin signos vitales registrados.");
        }
        else
        {
            lineas.AddRange(SignosVitalesVisualizacion
                .CrearFilas(signos, FormatoSignosVitales.NotaEvolucion)
                .Select(fila => $"{fila.Etiqueta}: {fila.Valor}"));
            lineas.Add("");
            lineas.Add("Nota medica: ________________________________________________________________");
            lineas.Add("____________________________________________________________________________");
        }

        lineas.Add("");
        lineas.Add($"Elaboro: {usuario.NombreCompleto}");

        return Generar("nota-evolucion", lineas, paciente, carpetaBase);
    }

    public static DocumentoClinicoGenerado GenerarHistoriaClinica(Paciente paciente, Usuario usuario, string carpetaBase)
    {
        var lineas = CrearEncabezado("HISTORIA CLINICA GENERAL", paciente, usuario);
        SignosVitales? signos = ObtenerUltimosSignos(paciente);

        lineas.AddRange(
        [
            $"Fecha de atencion: {DateTime.Today:dd/MM/yyyy}",
            $"Edad: {CalcularEdad(paciente.FechaNacimiento)}",
            $"Sexo: {ValorSeguro(paciente.Sexo)}"
        ]);

        if (signos is null)
        {
            lineas.Add("Sin signos vitales registrados.");
        }
        else
        {
            lineas.AddRange(SignosVitalesVisualizacion
                .CrearFilas(signos, FormatoSignosVitales.HistoriaClinica)
                .Select(fila => $"{fila.Etiqueta}: {fila.Valor}"));
        }

        lineas.AddRange(
        [
            "",
            "Diagnosticos: _______________________________________________________________",
            "Antecedentes: ______________________________________________________________",
            "Exploracion fisica: ________________________________________________________",
            "",
            $"Medico responsable: {usuario.NombreCompleto}"
        ]);

        return Generar("historia-clinica", lineas, paciente, carpetaBase);
    }

    private static DocumentoClinicoGenerado Generar(string tipo, IReadOnlyList<string> lineas, Paciente paciente, string carpetaBase)
    {
        string carpetaPaciente = CrearCarpetaPaciente(carpetaBase, paciente);
        string marca = DateTime.Now.ToString("yyyyMMdd-HHmmss", CultureInfo.InvariantCulture);
        string rutaDocx = Path.Combine(carpetaPaciente, $"{tipo}-{marca}.docx");
        string rutaPdf = Path.Combine(carpetaPaciente, $"{tipo}-{marca}.pdf");

        EscribirDocx(rutaDocx, lineas);
        EscribirPdf(rutaPdf, lineas);

        return new DocumentoClinicoGenerado(rutaDocx, rutaPdf);
    }

    private static List<string> CrearEncabezado(string titulo, Paciente paciente, Usuario usuario)
    {
        return
        [
            titulo,
            "SIGEM - Sistema de Gestion Medica",
            "",
            $"Paciente: {ValorSeguro(paciente.Nombre)} {ValorSeguro(paciente.Apellido)}".Trim(),
            $"CURP: {ValorSeguro(paciente.Curp)}",
            $"Expediente: {ValorSeguro(paciente.Expediente)}",
            $"Generado por: {usuario.NombreCompleto} ({usuario.Rol})",
            ""
        ];
    }

    private static string CrearCarpetaPaciente(string carpetaBase, Paciente paciente)
    {
        string expediente = string.IsNullOrWhiteSpace(paciente.Expediente)
            ? "sin-expediente"
            : paciente.Expediente;
        string carpeta = Path.Combine(carpetaBase, LimpiarNombreArchivo(expediente));
        Directory.CreateDirectory(carpeta);
        return carpeta;
    }

    private static SignosVitales? ObtenerUltimosSignos(Paciente paciente)
    {
        return paciente.SignosVitales
            .OrderByDescending(signos => signos.FechaHora)
            .FirstOrDefault();
    }

    private static int CalcularEdad(DateTime fechaNacimiento)
    {
        if (fechaNacimiento == default)
            return 0;

        int edad = DateTime.Today.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad))
            edad--;

        return Math.Max(0, edad);
    }

    private static void EscribirDocx(string ruta, IReadOnlyList<string> lineas)
    {
        using var archivo = ZipFile.Open(ruta, ZipArchiveMode.Create);
        AgregarEntrada(archivo, "[Content_Types].xml", """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
              <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml"/>
              <Default Extension="xml" ContentType="application/xml"/>
              <Override PartName="/word/document.xml" ContentType="application/vnd.openxmlformats-officedocument.wordprocessingml.document.main+xml"/>
            </Types>
            """);
        AgregarEntrada(archivo, "_rels/.rels", """
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
              <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="word/document.xml"/>
            </Relationships>
            """);
        AgregarEntrada(archivo, "word/document.xml", CrearDocumentoWordXml(lineas));
    }

    private static string CrearDocumentoWordXml(IReadOnlyList<string> lineas)
    {
        var cuerpo = new StringBuilder();
        foreach (string linea in lineas)
        {
            bool titulo = cuerpo.Length == 0;
            string propiedades = titulo
                ? "<w:pPr><w:jc w:val=\"center\"/></w:pPr><w:rPr><w:b/><w:sz w:val=\"32\"/></w:rPr>"
                : "<w:rPr><w:sz w:val=\"22\"/></w:rPr>";

            cuerpo.Append("<w:p><w:r>");
            cuerpo.Append(propiedades);
            cuerpo.Append("<w:t xml:space=\"preserve\">");
            cuerpo.Append(EscapeXml(linea));
            cuerpo.Append("</w:t></w:r></w:p>");
        }

        return $$"""
            <?xml version="1.0" encoding="UTF-8" standalone="yes"?>
            <w:document xmlns:w="http://schemas.openxmlformats.org/wordprocessingml/2006/main">
              <w:body>
                {{cuerpo}}
                <w:sectPr>
                  <w:pgSz w:w="12240" w:h="15840"/>
                  <w:pgMar w:top="1440" w:right="1440" w:bottom="1440" w:left="1440"/>
                </w:sectPr>
              </w:body>
            </w:document>
            """;
    }

    private static void EscribirPdf(string ruta, IReadOnlyList<string> lineas)
    {
        string contenido = CrearContenidoPdf(lineas);
        var objetos = new List<string>
        {
            "<< /Type /Catalog /Pages 2 0 R >>",
            "<< /Type /Pages /Kids [3 0 R] /Count 1 >>",
            "<< /Type /Page /Parent 2 0 R /MediaBox [0 0 612 792] /Resources << /Font << /F1 4 0 R >> >> /Contents 5 0 R >>",
            "<< /Type /Font /Subtype /Type1 /BaseFont /Helvetica >>",
            $"<< /Length {Encoding.ASCII.GetByteCount(contenido)} >>\nstream\n{contenido}\nendstream"
        };

        using var stream = new FileStream(ruta, FileMode.Create, FileAccess.Write);
        using var writer = new StreamWriter(stream, Encoding.ASCII);
        writer.NewLine = "\n";
        writer.Write("%PDF-1.4\n");

        var offsets = new List<long> { 0 };
        for (int i = 0; i < objetos.Count; i++)
        {
            writer.Flush();
            offsets.Add(stream.Position);
            writer.Write($"{i + 1} 0 obj\n{objetos[i]}\nendobj\n");
        }

        writer.Flush();
        long xref = stream.Position;
        writer.Write($"xref\n0 {objetos.Count + 1}\n");
        writer.Write("0000000000 65535 f \n");
        foreach (long offset in offsets.Skip(1))
            writer.Write($"{offset:0000000000} 00000 n \n");
        writer.Write($"trailer\n<< /Size {objetos.Count + 1} /Root 1 0 R >>\nstartxref\n{xref}\n%%EOF\n");
    }

    private static string CrearContenidoPdf(IReadOnlyList<string> lineas)
    {
        var contenido = new StringBuilder();
        contenido.Append("BT\n/F1 12 Tf\n50 742 Td\n16 TL\n");
        foreach (string linea in lineas.Take(42))
        {
            contenido.Append('(');
            contenido.Append(EscapePdf(linea));
            contenido.Append(") Tj\nT*\n");
        }

        contenido.Append("ET");
        return contenido.ToString();
    }

    private static void AgregarEntrada(ZipArchive archivo, string nombre, string contenido)
    {
        ZipArchiveEntry entrada = archivo.CreateEntry(nombre);
        using var writer = new StreamWriter(entrada.Open(), new UTF8Encoding(false));
        writer.Write(contenido.TrimStart());
    }

    private static string ValorSeguro(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? "--" : valor.Trim();
    }

    private static string LimpiarNombreArchivo(string texto)
    {
        var invalidos = Path.GetInvalidFileNameChars();
        return new string(texto.Select(c => invalidos.Contains(c) ? '-' : c).ToArray());
    }

    private static string EscapeXml(string texto)
    {
        return texto
            .Replace("&", "&amp;", StringComparison.Ordinal)
            .Replace("<", "&lt;", StringComparison.Ordinal)
            .Replace(">", "&gt;", StringComparison.Ordinal)
            .Replace("\"", "&quot;", StringComparison.Ordinal);
    }

    private static string EscapePdf(string texto)
    {
        return texto
            .Replace("\\", "\\\\", StringComparison.Ordinal)
            .Replace("(", "\\(", StringComparison.Ordinal)
            .Replace(")", "\\)", StringComparison.Ordinal);
    }
}
