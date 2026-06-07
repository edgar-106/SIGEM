using SIGEM.Modelo;
using Npgsql;

ProbarPermisosRol();
ProbarVisualizacionSignosVitales();

ConexionBD.CadenaConexion = "Host=localhost;Database=IMSS;Username=postgres;Password=postgres";
string marcaPrueba = $"SIGEM_TEST_{Guid.NewGuid():N}";

var repo = new SigemRepositorioPostgres();

Assert(repo.InicializarBaseDeDatos(), "La conexion a la base IMSS debe inicializar correctamente.");

List<Paciente> pacientes = repo.ObtenerTodos();
Assert(pacientes.Count >= 10, "Debe cargar los pacientes existentes de la base IMSS.");

Paciente? miguel = repo.BuscarPorCurp("GARM900101HDFRRN01");
Assert(miguel is not null, "Debe encontrar al paciente Miguel por CURP.");
Assert(miguel!.Expediente == "1", "El expediente de Miguel debe mapearse desde id_paciente.");
Assert(miguel.Nombre == "Miguel", "Debe mapear el nombre del paciente.");
Assert(miguel.Apellido == "Garcia Rodriguez" || miguel.Apellido == "García Rodríguez", "Debe unir apellido paterno y materno.");
Assert(miguel.Sexo == "Masculino", "Debe usar genero como sexo visible.");

Usuario? doctor = repo.AutenticarUsuario("1234567", "1234567");
Assert(doctor is not null, "Debe autenticar doctores por cedula cuando usuario/contrasena estan vacios.");
Assert(doctor!.Rol == RolUsuario.Doctor, "El usuario de doctores debe tener rol Doctor.");
Assert(doctor.NombreCompleto.Contains("Roberto", StringComparison.OrdinalIgnoreCase), "Debe mapear el nombre del doctor.");
Assert(doctor.IdDoctor == 1, "Debe conservar id_doctor para validar registros.");

Usuario? enfermera = repo.AutenticarUsuario("enfermera", "enfermera123");
Assert(enfermera is not null, "Debe autenticar enfermera desde la tabla usuarios.");
Assert(enfermera!.Rol == RolUsuario.Enfermera, "La enfermera debe tener rol Enfermera.");

Usuario? recepcion = repo.AutenticarUsuario("recepcion", "recepcion123");
Assert(recepcion is not null, "Debe autenticar recepcionista desde la tabla usuarios.");
Assert(recepcion!.Rol == RolUsuario.Recepcionista, "Recepcion debe tener rol Recepcionista.");

Usuario? admin = repo.AutenticarUsuario("admin", "admin123");
Assert(admin is not null, "Debe autenticar administrador desde la tabla usuarios.");
Assert(admin!.Rol == RolUsuario.Administrador, "Admin debe tener rol Administrador.");

int registrosAntes = miguel.SignosVitales.Count;
var signos = new SignosVitales
{
    FechaHora = DateTime.Now,
    Peso = 72.5,
    Estatura = 1.72,
    Temperatura = 36.7,
    Pulso = 78,
    FrecuenciaRespiratoria = 18,
    PresionSistolica = 120,
    PresionDiastolica = 80,
    CC = 94,
    SaturacionO2 = 98,
    RegistradoPor = marcaPrueba,
    Validado = true,
    ValidadoPor = doctor.NombreCompleto,
    IdDoctor = doctor.IdDoctor
};

var signosPendientes = new SignosVitales
{
    FechaHora = DateTime.Now.AddMinutes(1),
    Peso = 73,
    Estatura = 1.72,
    Temperatura = 36.8,
    Pulso = 80,
    FrecuenciaRespiratoria = 19,
    PresionSistolica = 121,
    PresionDiastolica = 81,
    CC = 95,
    SaturacionO2 = 97,
    RegistradoPor = marcaPrueba,
    Validado = false
};

try
{
    repo.AgregarSignosVitales(miguel.Expediente, signos);
    repo.AgregarSignosVitales(miguel.Expediente, signosPendientes);
    Paciente? actualizado = repo.BuscarPorIdentificador(miguel.Expediente);
    Assert(actualizado is not null, "Debe recargar el paciente despues de guardar signos vitales.");
    Assert(actualizado!.SignosVitales.Count == registrosAntes + 2, "Debe guardar signos vitales en notas_de_evolucion.");
    Assert(actualizado.EsBorrador, "Un registro de enfermera sin doctor debe quedar pendiente.");
}
finally
{
    EliminarNotasDePrueba(miguel.Id, marcaPrueba);
}

Console.WriteLine("SIGEM.Tests OK");

static void Assert(bool condition, string message)
{
    if (!condition)
        throw new InvalidOperationException(message);
}

void ProbarPermisosRol()
{
    var medico = PermisosRol.Para(RolUsuario.Doctor);
    Assert(medico.PuedeVerPacientes, "Medico debe ver pacientes.");
    Assert(medico.PuedeCapturarSignosVitales, "Medico debe capturar signos vitales.");
    Assert(medico.PuedeValidarSignosVitales, "Medico debe validar signos vitales.");
    Assert(medico.PuedeVerHistoriaClinica, "Medico debe ver historia clinica.");
    Assert(medico.PuedeVerNotaEvolucion, "Medico debe ver nota de evolucion.");
    Assert(!medico.PuedeAdministrarSistema, "Medico no debe administrar sistema.");

    var enfermera = PermisosRol.Para(RolUsuario.Enfermera);
    Assert(enfermera.PuedeCapturarSignosVitales, "Enfermera debe capturar signos vitales.");
    Assert(enfermera.PuedeVerNotaEvolucion, "Enfermera debe ver nota de evolucion.");
    Assert(!enfermera.PuedeValidarSignosVitales, "Enfermera no debe validar signos vitales.");
    Assert(!enfermera.PuedeAdministrarSistema, "Enfermera no debe administrar sistema.");

    var recepcion = PermisosRol.Para(RolUsuario.Recepcionista);
    Assert(recepcion.PuedeVerPacientes, "Recepcionista debe ver pacientes administrativos.");
    Assert(recepcion.PuedeAltaPaciente, "Recepcionista debe dar alta de pacientes.");
    Assert(!recepcion.PuedeVerSignosVitales, "Recepcionista no debe ver signos vitales.");
    Assert(!recepcion.PuedeCapturarSignosVitales, "Recepcionista no debe capturar signos vitales.");

    var admin = PermisosRol.Para(RolUsuario.Administrador);
    Assert(admin.PuedeAdministrarSistema, "Administrador debe administrar sistema.");
    Assert(!admin.PuedeCapturarSignosVitales, "Administrador no debe capturar signos vitales.");
    Assert(!admin.PuedeValidarSignosVitales, "Administrador no debe validar signos vitales.");
}

void ProbarVisualizacionSignosVitales()
{
    var registro = new SignosVitales
    {
        FechaHora = new DateTime(2026, 6, 2, 9, 30, 0),
        Peso = 72.4,
        Estatura = 1.70,
        Temperatura = 38.1,
        Pulso = 104,
        FrecuenciaRespiratoria = 22,
        PresionSistolica = 145,
        PresionDiastolica = 95,
        SaturacionO2 = 92,
        RegistradoPor = "Enfermera SIGEM",
        Validado = false
    };

    var nota = SignosVitalesVisualizacion.CrearFilas(registro, FormatoSignosVitales.NotaEvolucion).ToList();
    Assert(nota[0].Etiqueta == "Fecha", "Nota de Evolucion debe iniciar con Fecha.");
    Assert(nota[1].Etiqueta == "Hora", "Nota de Evolucion debe seguir con Hora.");
    Assert(nota[2].Etiqueta == "Peso", "Peso debe ser tercer dato en Nota de Evolucion.");
    Assert(nota.Any(f => f.Etiqueta == "PAM"), "Nota de Evolucion debe incluir PAM.");
    Assert(nota.Any(f => f.Etiqueta == "Estado" && f.Valor == "Pendiente"), "Registro no validado debe verse pendiente.");

    var historia = SignosVitalesVisualizacion.CrearFilas(registro, FormatoSignosVitales.HistoriaClinica).ToList();
    Assert(historia.Any(f => f.Etiqueta == "Fecha de atencion"), "Historia Clinica debe mostrar fecha de atencion.");
    Assert(historia.Any(f => f.Etiqueta == "Talla/estatura"), "Historia Clinica debe mostrar talla/estatura.");
    Assert(!historia.Any(f => f.Etiqueta == "Hora"), "Historia Clinica no debe priorizar hora.");

    var alertas = SignosVitalesVisualizacion.CrearAlertas(registro).ToList();
    Assert(alertas.Count >= 5, "Debe detectar varios signos fuera de rango.");
    Assert(alertas.Any(a => a.Campo == "Temperatura"), "Debe alertar temperatura alta.");
    Assert(alertas.Any(a => a.Campo == "Saturacion O2"), "Debe alertar saturacion baja.");
}

static void EliminarNotasDePrueba(int idPaciente, string marcaPrueba)
{
    using var conexion = ConexionBD.CrearConexion();
    conexion.Open();

    using var borrar = new NpgsqlCommand(
        "DELETE FROM notas_de_evolucion n USING historiales_medicos h " +
        "WHERE h.codigo_historial = n.codigo_historial " +
        "AND h.id_paciente = @pid " +
        "AND n.nota_medica = @nota", conexion);
    borrar.Parameters.AddWithValue("@pid", idPaciente);
    borrar.Parameters.AddWithValue("@nota", $"Signos vitales registrados por {marcaPrueba}");
    borrar.ExecuteNonQuery();

    using var borrarHistorialVacio = new NpgsqlCommand(
        "DELETE FROM historiales_medicos h " +
        "WHERE h.id_paciente = @pid " +
        "AND h.antecedentes_hereditarios IS NULL " +
        "AND h.aparatos_sistemas IS NULL " +
        "AND h.inspeccion_general IS NULL " +
        "AND NOT EXISTS (SELECT 1 FROM notas_de_evolucion n WHERE n.codigo_historial = h.codigo_historial)", conexion);
    borrarHistorialVacio.Parameters.AddWithValue("@pid", idPaciente);
    borrarHistorialVacio.ExecuteNonQuery();
}
