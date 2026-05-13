namespace SIGEM.Modelo;

public class SigemRepositorioTxt : ISigemRepositorio
{
    private readonly string rutaArchivo;

    public SigemRepositorioTxt()
    {
        string carpetaDatos = Path.Combine(AppContext.BaseDirectory, "Datos");
        Directory.CreateDirectory(carpetaDatos);
        rutaArchivo = Path.Combine(carpetaDatos, "sigem.txt");
    }

    public IReadOnlyList<Sigem> ObtenerTodos()
    {
        if (!File.Exists(rutaArchivo))
        {
            return [];
        }

        return File.ReadAllLines(rutaArchivo)
            .Where(linea => !string.IsNullOrWhiteSpace(linea))
            .Select(nombre => new Sigem(nombre.Trim()))
            .ToList();
    }

    public void Guardar(Sigem sigem)
    {
        File.AppendAllLines(rutaArchivo, [sigem.Nombre]);
    }
}
