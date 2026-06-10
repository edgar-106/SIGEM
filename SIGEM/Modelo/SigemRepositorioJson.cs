using System.Text.Json;

namespace SIGEM.Modelo;

public class SigemRepositorioJson : ISigemRepositorio
{
    private readonly string rutaArchivo;
    private readonly JsonSerializerOptions jsonOptions = new() { WriteIndented = true };

    public SigemRepositorioJson()
    {
        string carpetaDatos = Path.Combine(AppContext.BaseDirectory, "Datos", "ims");
        Directory.CreateDirectory(carpetaDatos);
        rutaArchivo = Path.Combine(carpetaDatos, "pacientes.json");

        string rutaAnterior = Path.Combine(AppContext.BaseDirectory, "Datos", "pacientes.json");
        if (!File.Exists(rutaArchivo) && File.Exists(rutaAnterior))
        {
            File.Copy(rutaAnterior, rutaArchivo);
        }
    }

    public Paciente? BuscarPorExpediente(string expediente)
    {
        var pacientes = CargarPacientes();
        return pacientes.Find(p =>
            string.Equals(p.Expediente, expediente, StringComparison.OrdinalIgnoreCase));
    }

    public Paciente? BuscarPorCurp(string curp)
    {
        var pacientes = CargarPacientes();
        return pacientes.Find(p =>
            string.Equals(p.Curp, curp, StringComparison.OrdinalIgnoreCase));
    }

    public Paciente? BuscarPorIdentificador(string identificador)
    {
        var pacientes = CargarPacientes();
        return pacientes.Find(p =>
            string.Equals(p.Expediente, identificador, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, identificador, StringComparison.OrdinalIgnoreCase));
    }

    public List<Paciente> ObtenerTodos()
    {
        return CargarPacientes();
    }

    public void GuardarPaciente(Paciente paciente)
    {
        var pacientes = CargarPacientes();
        var existente = pacientes.FindIndex(p =>
            string.Equals(p.Expediente, paciente.Expediente, StringComparison.OrdinalIgnoreCase) ||
            (!string.IsNullOrWhiteSpace(paciente.Curp) &&
             string.Equals(p.Curp, paciente.Curp, StringComparison.OrdinalIgnoreCase)));

        if (existente >= 0)
        {
            paciente.SignosVitales = pacientes[existente].SignosVitales;
            pacientes[existente] = paciente;
        }
        else
        {
            paciente.FechaRegistro = DateTime.Now;
            pacientes.Add(paciente);
        }

        GuardarPacientes(pacientes);
    }

    public void AgregarSignosVitales(string expediente, SignosVitales sv)
    {
        var pacientes = CargarPacientes();
        var idx = pacientes.FindIndex(p =>
            string.Equals(p.Expediente, expediente, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, expediente, StringComparison.OrdinalIgnoreCase));

        if (idx >= 0)
        {
            pacientes[idx].SignosVitales.Add(sv);
            GuardarPacientes(pacientes);
        }
    }

    public void ActualizarSignosVitales(string expediente, int indiceSignoVital, SignosVitales sv)
    {
        var pacientes = CargarPacientes();
        var idx = pacientes.FindIndex(p =>
            string.Equals(p.Expediente, expediente, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, expediente, StringComparison.OrdinalIgnoreCase));

        if (idx < 0 || indiceSignoVital < 0 || indiceSignoVital >= pacientes[idx].SignosVitales.Count)
            return;

        pacientes[idx].SignosVitales[indiceSignoVital] = sv;
        pacientes[idx].EsBorrador = pacientes[idx].SignosVitales.Any(signos => !signos.Validado);
        GuardarPacientes(pacientes);
    }

    public void ValidarRegistro(string expediente, int indiceSignoVital, string validadoPor)
    {
        var pacientes = CargarPacientes();
        var idx = pacientes.FindIndex(p =>
            string.Equals(p.Expediente, expediente, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, expediente, StringComparison.OrdinalIgnoreCase));

        if (idx >= 0 && indiceSignoVital >= 0 && indiceSignoVital < pacientes[idx].SignosVitales.Count)
        {
            var sv = pacientes[idx].SignosVitales[indiceSignoVital];
            sv.Validado = true;
            sv.ValidadoPor = validadoPor;
            pacientes[idx].EsBorrador = false;
            GuardarPacientes(pacientes);
        }
    }

    public List<Paciente> ObtenerBorradores()
    {
        var pacientes = CargarPacientes();
        return pacientes.Where(p => p.EsBorrador).ToList();
    }

    public Usuario? AutenticarUsuario(string nombreUsuario, string contrasena)
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, "Datos", "ims", "usuarios.json");
        if (!File.Exists(ruta)) return null;

        string json = File.ReadAllText(ruta);
        var usuarios = JsonSerializer.Deserialize<List<Usuario>>(json) ?? [];
        return usuarios.Find(u =>
            string.Equals(u.NombreUsuario, nombreUsuario, StringComparison.OrdinalIgnoreCase)
            && u.Contrasena == contrasena);
    }

    public bool EliminarPaciente(string identificador)
    {
        var pacientes = CargarPacientes();
        int eliminados = pacientes.RemoveAll(p =>
            string.Equals(p.Expediente, identificador, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, identificador, StringComparison.OrdinalIgnoreCase));

        if (eliminados > 0)
            GuardarPacientes(pacientes);

        return eliminados > 0;
    }

    public void GuardarDiagnostico(string identificador, string diagnostico)
    {
        var pacientes = CargarPacientes();
        var idx = pacientes.FindIndex(p =>
            string.Equals(p.Expediente, identificador, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, identificador, StringComparison.OrdinalIgnoreCase));

        if (idx >= 0)
        {
            pacientes[idx].Diagnostico = diagnostico;
            GuardarPacientes(pacientes);
        }
    }

    public void GuardarTratamiento(string identificador, string tratamiento)
    {
        var pacientes = CargarPacientes();
        var idx = pacientes.FindIndex(p =>
            string.Equals(p.Expediente, identificador, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(p.Curp, identificador, StringComparison.OrdinalIgnoreCase));

        if (idx >= 0)
        {
            pacientes[idx].Tratamiento = tratamiento;
            GuardarPacientes(pacientes);
        }
    }

    private List<Paciente> CargarPacientes()
    {
        if (!File.Exists(rutaArchivo))
            return [];

        string json = File.ReadAllText(rutaArchivo);
        return JsonSerializer.Deserialize<List<Paciente>>(json) ?? [];
    }

    private void GuardarPacientes(List<Paciente> pacientes)
    {
        string json = JsonSerializer.Serialize(pacientes, jsonOptions);
        File.WriteAllText(rutaArchivo, json);
    }
}
