using Npgsql;
using NpgsqlTypes;

namespace SIGEM.Modelo;

public class SigemRepositorioPostgres : ISigemRepositorio
{
    public Paciente? BuscarPorExpediente(string expediente)
    {
        if (string.IsNullOrWhiteSpace(expediente)) return null;

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        string sql = int.TryParse(expediente.Trim(), out int idPaciente)
            ? PacienteSelect() + " WHERE id_paciente = @id"
            : PacienteSelect() + " WHERE false";

        using var cmd = new NpgsqlCommand(sql, conexion);
        if (idPaciente > 0)
            cmd.Parameters.AddWithValue("@id", idPaciente);

        Paciente? paciente = LeerPaciente(cmd, conexion);
        return paciente;
    }

    public Paciente? BuscarPorCurp(string curp)
    {
        if (string.IsNullOrWhiteSpace(curp)) return null;

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using var cmd = new NpgsqlCommand(PacienteSelect() + " WHERE curp = @curp", conexion);
        cmd.Parameters.AddWithValue("@curp", curp.Trim().ToUpperInvariant());

        return LeerPaciente(cmd, conexion);
    }

    public Paciente? BuscarPorIdentificador(string identificador)
    {
        if (string.IsNullOrWhiteSpace(identificador)) return null;

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        string texto = identificador.Trim();
        bool esNumero = int.TryParse(texto, out int idPaciente);
        string sql = esNumero
            ? PacienteSelect() + " WHERE id_paciente = @id OR curp = @curp"
            : PacienteSelect() + " WHERE curp = @curp";

        using var cmd = new NpgsqlCommand(sql, conexion);
        if (esNumero)
            cmd.Parameters.AddWithValue("@id", idPaciente);
        cmd.Parameters.AddWithValue("@curp", texto.ToUpperInvariant());

        return LeerPaciente(cmd, conexion);
    }

    public List<Paciente> ObtenerTodos()
    {
        var pacientes = new List<Paciente>();

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        using (var cmd = new NpgsqlCommand(PacienteSelect() + " ORDER BY id_paciente", conexion))
        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
                pacientes.Add(MapearPaciente(reader));
        }

        foreach (Paciente paciente in pacientes)
        {
            if (int.TryParse(paciente.Expediente, out int idPaciente))
            {
                paciente.SignosVitales = CargarSignosVitales(idPaciente, conexion);
                paciente.EsBorrador = paciente.SignosVitales.Any(signos => !signos.Validado);
            }
        }

        return pacientes;
    }

    public void GuardarPaciente(Paciente paciente)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        int? idPaciente = ObtenerPacienteId(conexion, paciente.Expediente);
        if (idPaciente is null && !string.IsNullOrWhiteSpace(paciente.Curp))
            idPaciente = ObtenerPacienteId(conexion, paciente.Curp);

        (string paterno, string materno) = SepararApellidos(paciente.Apellido);
        string genero = NormalizarGenero(paciente.Sexo);
        string sexo = genero.StartsWith("Fem", StringComparison.OrdinalIgnoreCase) ? "M" : "H";

        if (idPaciente is not null)
        {
            using var cmd = new NpgsqlCommand(
                "UPDATE pacientes SET curp = @curp, nombre = @nombre, apellido_paterno = @ap, " +
                "apellido_materno = @am, sexo = @sexo, genero = @genero WHERE id_paciente = @id", conexion);
            cmd.Parameters.AddWithValue("@id", idPaciente.Value);
            cmd.Parameters.AddWithValue("@curp", paciente.Curp.Trim().ToUpperInvariant());
            cmd.Parameters.AddWithValue("@nombre", (object?)paciente.Nombre.Trim() ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ap", (object?)paterno ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@am", (object?)materno ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@sexo", sexo);
            cmd.Parameters.AddWithValue("@genero", genero);
            cmd.ExecuteNonQuery();

            paciente.Id = idPaciente.Value;
            paciente.Expediente = idPaciente.Value.ToString();
            return;
        }

        using var insert = new NpgsqlCommand(
            "INSERT INTO pacientes (curp, nombre, apellido_paterno, apellido_materno, sexo, genero) " +
            "VALUES (@curp, @nombre, @ap, @am, @sexo, @genero) RETURNING id_paciente", conexion);
        insert.Parameters.AddWithValue("@curp", paciente.Curp.Trim().ToUpperInvariant());
        insert.Parameters.AddWithValue("@nombre", (object?)paciente.Nombre.Trim() ?? DBNull.Value);
        insert.Parameters.AddWithValue("@ap", (object?)paterno ?? DBNull.Value);
        insert.Parameters.AddWithValue("@am", (object?)materno ?? DBNull.Value);
        insert.Parameters.AddWithValue("@sexo", sexo);
        insert.Parameters.AddWithValue("@genero", genero);

        int nuevoId = Convert.ToInt32(insert.ExecuteScalar());
        paciente.Id = nuevoId;
        paciente.Expediente = nuevoId.ToString();

        foreach (SignosVitales signos in paciente.SignosVitales)
            InsertarNotaEvolucion(conexion, nuevoId, signos);
    }

    public void AgregarSignosVitales(string expediente, SignosVitales sv)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        int? idPaciente = ObtenerPacienteId(conexion, expediente);
        if (idPaciente is null) return;

        InsertarNotaEvolucion(conexion, idPaciente.Value, sv);
    }

    public void ActualizarSignosVitales(string expediente, int indiceSignoVital, SignosVitales sv)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        int? idPaciente = ObtenerPacienteId(conexion, expediente);
        if (idPaciente is null) return;

        int? idNota = ObtenerNotaEvolucionIdPorIndice(conexion, idPaciente.Value, indiceSignoVital);
        if (idNota is null) return;

        int? idDoctor = sv.IdDoctor ?? (sv.Validado ? ObtenerDoctorId(conexion, sv.ValidadoPor ?? sv.RegistradoPor) : null);

        using var update = new NpgsqlCommand(
            "UPDATE notas_de_evolucion SET id_doctor = @doctor, fecha = @fecha, hora = @hora, " +
            "presion_arterial = @pa, frecuencia_respiratoria = @fr, nota_medica = @nota, " +
            "peso = @peso, temperatura = @temp, estatura = @estatura, pulso = @pulso, cc = @cc, " +
            "saturacion_oxigeno = @spo2 WHERE numero_expediente = @notaId", conexion);
        update.Parameters.AddWithValue("@notaId", idNota.Value);
        update.Parameters.Add("@doctor", NpgsqlDbType.Integer).Value = idDoctor is null ? DBNull.Value : idDoctor.Value;
        update.Parameters.AddWithValue("@fecha", sv.FechaHora.Date);
        update.Parameters.AddWithValue("@hora", sv.FechaHora.TimeOfDay);
        update.Parameters.AddWithValue("@pa", $"{sv.PresionSistolica}/{sv.PresionDiastolica}");
        update.Parameters.AddWithValue("@fr", sv.FrecuenciaRespiratoria.ToString());
        update.Parameters.AddWithValue("@nota", string.IsNullOrWhiteSpace(sv.RegistradoPor)
            ? "Signos vitales corregidos por medico"
            : $"Signos vitales corregidos y validados por {sv.RegistradoPor}");
        update.Parameters.AddWithValue("@peso", Convert.ToDecimal(sv.Peso));
        update.Parameters.AddWithValue("@temp", Convert.ToDecimal(sv.Temperatura));
        update.Parameters.AddWithValue("@estatura", Convert.ToDecimal(sv.Estatura));
        update.Parameters.AddWithValue("@pulso", sv.Pulso.ToString());
        update.Parameters.AddWithValue("@cc", Convert.ToDecimal(sv.CC));
        update.Parameters.AddWithValue("@spo2", Convert.ToDecimal(sv.SaturacionO2));
        update.ExecuteNonQuery();
    }

    public void ValidarRegistro(string expediente, int indiceSignoVital, string validadoPor)
    {
        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        int? idPaciente = ObtenerPacienteId(conexion, expediente);
        if (idPaciente is null) return;

        int? idDoctor = ObtenerDoctorId(conexion, validadoPor) ?? ObtenerPrimerDoctorId(conexion);
        if (idDoctor is null) return;

        var idsNotas = new List<int>();
        using (var cmd = new NpgsqlCommand(
            "SELECT n.numero_expediente " +
            "FROM notas_de_evolucion n " +
            "JOIN historiales_medicos h ON h.codigo_historial = n.codigo_historial " +
            "WHERE h.id_paciente = @pid ORDER BY n.fecha ASC, n.hora ASC, n.numero_expediente ASC", conexion))
        {
            cmd.Parameters.AddWithValue("@pid", idPaciente.Value);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                idsNotas.Add(GetInt(reader, "numero_expediente"));
        }

        if (indiceSignoVital < 0 || indiceSignoVital >= idsNotas.Count) return;

        using var update = new NpgsqlCommand(
            "UPDATE notas_de_evolucion SET id_doctor = @doctor WHERE numero_expediente = @nota", conexion);
        update.Parameters.AddWithValue("@doctor", idDoctor.Value);
        update.Parameters.AddWithValue("@nota", idsNotas[indiceSignoVital]);
        update.ExecuteNonQuery();
    }

    public List<Paciente> ObtenerBorradores()
    {
        return ObtenerTodos().Where(p => p.EsBorrador).ToList();
    }

    public bool EliminarPaciente(string identificador)
    {
        if (string.IsNullOrWhiteSpace(identificador)) return false;

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        int? idPaciente = ObtenerPacienteId(conexion, identificador);
        if (idPaciente is null) return false;

        using var tx = conexion.BeginTransaction();

        using (var notas = new NpgsqlCommand(
            "DELETE FROM notas_de_evolucion WHERE codigo_historial IN " +
            "(SELECT codigo_historial FROM historiales_medicos WHERE id_paciente = @pid)", conexion, tx))
        {
            notas.Parameters.AddWithValue("@pid", idPaciente.Value);
            notas.ExecuteNonQuery();
        }

        using (var historiales = new NpgsqlCommand(
            "DELETE FROM historiales_medicos WHERE id_paciente = @pid", conexion, tx))
        {
            historiales.Parameters.AddWithValue("@pid", idPaciente.Value);
            historiales.ExecuteNonQuery();
        }

        using var paciente = new NpgsqlCommand(
            "DELETE FROM pacientes WHERE id_paciente = @pid", conexion, tx);
        paciente.Parameters.AddWithValue("@pid", idPaciente.Value);
        int eliminados = paciente.ExecuteNonQuery();

        tx.Commit();
        return eliminados > 0;
    }

    public Usuario? AutenticarUsuario(string nombreUsuario, string contrasena)
    {
        if (string.IsNullOrWhiteSpace(nombreUsuario) || string.IsNullOrWhiteSpace(contrasena))
            return null;

        using var conexion = ConexionBD.CrearConexion();
        conexion.Open();

        Usuario? usuarioSistema = AutenticarUsuarioSistema(conexion, nombreUsuario, contrasena);
        if (usuarioSistema is not null)
            return usuarioSistema;

        using var cmd = new NpgsqlCommand(
            "SELECT id_doctor, cedula_profesional, nombre, apellido_paterno, apellido_materno, usuario, contrasena " +
            "FROM doctores " +
            "WHERE lower(COALESCE(NULLIF(btrim(usuario), ''), cedula_profesional)) = lower(@usuario) " +
            "AND COALESCE(NULLIF(btrim(contrasena), ''), cedula_profesional) = @contrasena", conexion);
        cmd.Parameters.AddWithValue("@usuario", nombreUsuario.Trim());
        cmd.Parameters.AddWithValue("@contrasena", contrasena);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        string cedula = GetString(reader, "cedula_profesional");
        string usuario = GetString(reader, "usuario");
        string nombreCompleto = UnirNombre(
            GetString(reader, "nombre"),
            GetString(reader, "apellido_paterno"),
            GetString(reader, "apellido_materno"));

        return new Usuario
        {
            IdDoctor = GetInt(reader, "id_doctor"),
            NombreUsuario = string.IsNullOrWhiteSpace(usuario) ? cedula : usuario,
            Contrasena = string.Empty,
            NombreCompleto = string.IsNullOrWhiteSpace(nombreCompleto) ? $"Doctor {cedula}" : nombreCompleto,
            Rol = RolUsuario.Doctor
        };
    }

    public bool InicializarBaseDeDatos()
    {
        try
        {
            using var conexion = ConexionBD.CrearConexion();
            conexion.Open();

            using var ctPacientes = new NpgsqlCommand(
                "CREATE TABLE IF NOT EXISTS pacientes (" +
                "id_paciente SERIAL PRIMARY KEY, " +
                "curp VARCHAR(20) UNIQUE, " +
                "nombre VARCHAR(100), " +
                "apellido_paterno VARCHAR(100), " +
                "apellido_materno VARCHAR(100), " +
                "sexo VARCHAR(10), " +
                "genero VARCHAR(20))", conexion);
            ctPacientes.ExecuteNonQuery();

            using var ctDoctores = new NpgsqlCommand(
                "CREATE TABLE IF NOT EXISTS doctores (" +
                "id_doctor SERIAL PRIMARY KEY, " +
                "cedula_profesional VARCHAR(50) UNIQUE NOT NULL, " +
                "nombre VARCHAR(100), " +
                "apellido_paterno VARCHAR(100), " +
                "apellido_materno VARCHAR(100), " +
                "usuario VARCHAR(50), " +
                "contrasena VARCHAR(100))", conexion);
            ctDoctores.ExecuteNonQuery();

            using var ctHistoriales = new NpgsqlCommand(
                "CREATE TABLE IF NOT EXISTS historiales_medicos (" +
                "codigo_historial SERIAL PRIMARY KEY, " +
                "id_paciente INTEGER NOT NULL REFERENCES pacientes(id_paciente) ON DELETE CASCADE, " +
                "fecha DATE DEFAULT CURRENT_DATE NOT NULL)", conexion);
            ctHistoriales.ExecuteNonQuery();

            using var ctNotas = new NpgsqlCommand(
                "CREATE TABLE IF NOT EXISTS notas_de_evolucion (" +
                "numero_expediente SERIAL PRIMARY KEY, " +
                "codigo_historial INTEGER NOT NULL REFERENCES historiales_medicos(codigo_historial) ON DELETE CASCADE, " +
                "id_doctor INTEGER REFERENCES doctores(id_doctor), " +
                "fecha DATE DEFAULT CURRENT_DATE NOT NULL, " +
                "hora TIME DEFAULT CURRENT_TIME NOT NULL, " +
                "presion_arterial VARCHAR(20), " +
                "frecuencia_respiratoria VARCHAR(10), " +
                "nota_medica TEXT, " +
                "peso DECIMAL(5,2), " +
                "temperatura DECIMAL(4,1), " +
                "estatura DECIMAL(4,2), " +
                "pulso VARCHAR(10), " +
                "cc DECIMAL(5,1), " +
                "saturacion_oxigeno DECIMAL(4,1))", conexion);
            ctNotas.ExecuteNonQuery();

            using var idxHist = new NpgsqlCommand(
                "CREATE INDEX IF NOT EXISTS idx_historiales_paciente ON historiales_medicos(id_paciente)", conexion);
            idxHist.ExecuteNonQuery();

            using var idxNotas = new NpgsqlCommand(
                "CREATE INDEX IF NOT EXISTS idx_notas_historial ON notas_de_evolucion(codigo_historial)", conexion);
            idxNotas.ExecuteNonQuery();

            InicializarTablaUsuarios(conexion);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Usuario? AutenticarUsuarioSistema(NpgsqlConnection conexion, string nombreUsuario, string contrasena)
    {
        if (!ExisteTabla(conexion, "usuarios"))
            return null;

        using var cmd = new NpgsqlCommand(
            "SELECT id_usuario, nombre_usuario, nombre_completo, rol, id_doctor " +
            "FROM usuarios " +
            "WHERE lower(nombre_usuario) = lower(@usuario) " +
            "AND contrasena = @contrasena " +
            "AND activo = true", conexion);
        cmd.Parameters.AddWithValue("@usuario", nombreUsuario.Trim());
        cmd.Parameters.AddWithValue("@contrasena", contrasena);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Usuario
        {
            IdUsuario = GetInt(reader, "id_usuario"),
            IdDoctor = GetNullableInt(reader, "id_doctor"),
            NombreUsuario = GetString(reader, "nombre_usuario"),
            Contrasena = string.Empty,
            NombreCompleto = GetString(reader, "nombre_completo"),
            Rol = MapearRol(GetString(reader, "rol"))
        };
    }

    private static void InicializarTablaUsuarios(NpgsqlConnection conexion)
    {
        using (var crear = new NpgsqlCommand(
            "CREATE TABLE IF NOT EXISTS usuarios (" +
            "id_usuario SERIAL PRIMARY KEY, " +
            "nombre_usuario VARCHAR(50) UNIQUE NOT NULL, " +
            "contrasena VARCHAR(100) NOT NULL, " +
            "nombre_completo VARCHAR(200) NOT NULL, " +
            "rol VARCHAR(30) NOT NULL, " +
            "activo BOOLEAN DEFAULT true NOT NULL, " +
            "id_doctor INTEGER NULL REFERENCES doctores(id_doctor))", conexion))
        {
            crear.ExecuteNonQuery();
        }

        InsertarUsuarioSiNoExiste(conexion, "doctor", "doctor123", "Doctor SIGEM", "Doctor", null);
        InsertarUsuarioSiNoExiste(conexion, "enfermera", "enfermera123", "Enfermera SIGEM", "Enfermera", null);
        InsertarUsuarioSiNoExiste(conexion, "recepcion", "recepcion123", "Recepcionista SIGEM", "Recepcionista", null);
        InsertarUsuarioSiNoExiste(conexion, "admin", "admin123", "Administrador SIGEM", "Administrador", null);

        using var doctores = new NpgsqlCommand(
            "INSERT INTO usuarios (nombre_usuario, contrasena, nombre_completo, rol, activo, id_doctor) " +
            "SELECT d.cedula_profesional, d.cedula_profesional, " +
            "btrim(COALESCE(d.nombre, '') || ' ' || COALESCE(d.apellido_paterno, '') || ' ' || COALESCE(d.apellido_materno, '')), " +
            "'Medico', true, d.id_doctor " +
            "FROM doctores d " +
            "WHERE NOT EXISTS (SELECT 1 FROM usuarios u WHERE u.nombre_usuario = d.cedula_profesional) " +
            "AND d.cedula_profesional IS NOT NULL " +
            "AND btrim(d.cedula_profesional) <> ''", conexion);
        doctores.ExecuteNonQuery();
    }

    private static bool ExisteTabla(NpgsqlConnection conexion, string tabla)
    {
        using var cmd = new NpgsqlCommand(
            "SELECT EXISTS (SELECT 1 FROM information_schema.tables " +
            "WHERE table_schema = 'public' AND table_name = @tabla)", conexion);
        cmd.Parameters.AddWithValue("@tabla", tabla);
        return cmd.ExecuteScalar() is bool existe && existe;
    }

    private static void InsertarUsuarioSiNoExiste(
        NpgsqlConnection conexion,
        string nombreUsuario,
        string contrasena,
        string nombreCompleto,
        string rol,
        int? idDoctor)
    {
        using var cmd = new NpgsqlCommand(
            "INSERT INTO usuarios (nombre_usuario, contrasena, nombre_completo, rol, activo, id_doctor) " +
            "SELECT @usuario, @contrasena, @nombre, @rol, true, @idDoctor " +
            "WHERE NOT EXISTS (SELECT 1 FROM usuarios WHERE nombre_usuario = @usuario)", conexion);
        cmd.Parameters.AddWithValue("@usuario", nombreUsuario);
        cmd.Parameters.AddWithValue("@contrasena", contrasena);
        cmd.Parameters.AddWithValue("@nombre", nombreCompleto);
        cmd.Parameters.AddWithValue("@rol", rol);
        cmd.Parameters.Add("@idDoctor", NpgsqlDbType.Integer).Value = idDoctor is null ? DBNull.Value : idDoctor.Value;
        cmd.ExecuteNonQuery();
    }

    private static string PacienteSelect()
    {
        return "SELECT id_paciente, curp, nombre, apellido_paterno, apellido_materno, sexo, genero FROM pacientes";
    }

    private static Paciente? LeerPaciente(NpgsqlCommand cmd, NpgsqlConnection conexion)
    {
        Paciente? paciente = null;
        using (var reader = cmd.ExecuteReader())
        {
            if (reader.Read())
                paciente = MapearPaciente(reader);
        }

        if (paciente is not null && int.TryParse(paciente.Expediente, out int idPaciente))
        {
            paciente.SignosVitales = CargarSignosVitales(idPaciente, conexion);
            paciente.EsBorrador = paciente.SignosVitales.Any(signos => !signos.Validado);
        }

        return paciente;
    }

    private static Paciente MapearPaciente(NpgsqlDataReader reader)
    {
        int idPaciente = GetInt(reader, "id_paciente");
        string genero = GetString(reader, "genero");
        string sexo = string.IsNullOrWhiteSpace(genero) ? GetString(reader, "sexo") : genero;

        var paciente = new Paciente
        {
            Id = idPaciente,
            Expediente = idPaciente.ToString(),
            Curp = GetString(reader, "curp"),
            Nombre = GetString(reader, "nombre"),
            Apellido = UnirNombre(GetString(reader, "apellido_paterno"), GetString(reader, "apellido_materno")),
            Sexo = NormalizarGenero(sexo),
            FechaRegistro = DateTime.Today
        };

        return paciente;
    }

    private static List<SignosVitales> CargarSignosVitales(int pacienteId, NpgsqlConnection conexion)
    {
        var signos = new List<SignosVitales>();

        using var cmd = new NpgsqlCommand(
            "SELECT n.numero_expediente, n.fecha, n.hora, n.presion_arterial, n.frecuencia_respiratoria, " +
            "n.peso, n.temperatura, n.estatura, n.pulso, n.cc, n.saturacion_oxigeno, n.id_doctor, " +
            "d.nombre AS doctor_nombre, d.apellido_paterno AS doctor_apellido_paterno, d.apellido_materno AS doctor_apellido_materno " +
            "FROM notas_de_evolucion n " +
            "JOIN historiales_medicos h ON h.codigo_historial = n.codigo_historial " +
            "LEFT JOIN doctores d ON d.id_doctor = n.id_doctor " +
            "WHERE h.id_paciente = @pid " +
            "ORDER BY n.fecha ASC, n.hora ASC, n.numero_expediente ASC", conexion);
        cmd.Parameters.AddWithValue("@pid", pacienteId);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            (int sistolica, int diastolica) = SepararPresion(GetString(reader, "presion_arterial"));
            int? idDoctor = GetNullableInt(reader, "id_doctor");
            string doctor = UnirNombre(
                GetString(reader, "doctor_nombre"),
                GetString(reader, "doctor_apellido_paterno"),
                GetString(reader, "doctor_apellido_materno"));

            signos.Add(new SignosVitales
            {
                FechaHora = UnirFechaHora(reader),
                Peso = GetDouble(reader, "peso"),
                Estatura = GetDouble(reader, "estatura"),
                Temperatura = GetDouble(reader, "temperatura"),
                Pulso = GetIntFromText(reader, "pulso"),
                FrecuenciaRespiratoria = GetIntFromText(reader, "frecuencia_respiratoria"),
                PresionSistolica = sistolica,
                PresionDiastolica = diastolica,
                CC = GetDouble(reader, "cc"),
                SaturacionO2 = GetDouble(reader, "saturacion_oxigeno"),
                RegistradoPor = string.IsNullOrWhiteSpace(doctor) ? "Enfermera" : doctor,
                Validado = idDoctor is not null,
                ValidadoPor = idDoctor is null ? null : doctor,
                IdDoctor = idDoctor
            });
        }

        return signos;
    }

    private static void InsertarNotaEvolucion(NpgsqlConnection conexion, int idPaciente, SignosVitales sv)
    {
        int codigoHistorial = ObtenerOCrearHistorial(conexion, idPaciente);
        int? idDoctor = sv.IdDoctor ?? (sv.Validado ? ObtenerDoctorId(conexion, sv.ValidadoPor ?? sv.RegistradoPor) : null);

        using var cmd = new NpgsqlCommand(
            "INSERT INTO notas_de_evolucion (codigo_historial, id_doctor, fecha, hora, presion_arterial, " +
            "frecuencia_respiratoria, nota_medica, peso, temperatura, estatura, pulso, cc, saturacion_oxigeno) " +
            "VALUES (@historial, @doctor, @fecha, @hora, @pa, @fr, @nota, @peso, @temp, @estatura, @pulso, @cc, @spo2)", conexion);
        cmd.Parameters.AddWithValue("@historial", codigoHistorial);
        cmd.Parameters.Add("@doctor", NpgsqlDbType.Integer).Value = idDoctor is null ? DBNull.Value : idDoctor.Value;
        cmd.Parameters.AddWithValue("@fecha", sv.FechaHora.Date);
        cmd.Parameters.AddWithValue("@hora", sv.FechaHora.TimeOfDay);
        cmd.Parameters.AddWithValue("@pa", $"{sv.PresionSistolica}/{sv.PresionDiastolica}");
        cmd.Parameters.AddWithValue("@fr", sv.FrecuenciaRespiratoria.ToString());
        cmd.Parameters.AddWithValue("@nota", string.IsNullOrWhiteSpace(sv.RegistradoPor)
            ? "Signos vitales"
            : $"Signos vitales registrados por {sv.RegistradoPor}");
        cmd.Parameters.AddWithValue("@peso", Convert.ToDecimal(sv.Peso));
        cmd.Parameters.AddWithValue("@temp", Convert.ToDecimal(sv.Temperatura));
        cmd.Parameters.AddWithValue("@estatura", Convert.ToDecimal(sv.Estatura));
        cmd.Parameters.AddWithValue("@pulso", sv.Pulso.ToString());
        cmd.Parameters.AddWithValue("@cc", Convert.ToDecimal(sv.CC));
        cmd.Parameters.AddWithValue("@spo2", Convert.ToDecimal(sv.SaturacionO2));
        cmd.ExecuteNonQuery();
    }

    private static int ObtenerOCrearHistorial(NpgsqlConnection conexion, int idPaciente)
    {
        using (var buscar = new NpgsqlCommand(
            "SELECT codigo_historial FROM historiales_medicos WHERE id_paciente = @pid", conexion))
        {
            buscar.Parameters.AddWithValue("@pid", idPaciente);
            object? existente = buscar.ExecuteScalar();
            if (existente is not null && existente != DBNull.Value)
                return Convert.ToInt32(existente);
        }

        using var crear = new NpgsqlCommand(
            "INSERT INTO historiales_medicos (id_paciente, fecha) VALUES (@pid, CURRENT_DATE) RETURNING codigo_historial", conexion);
        crear.Parameters.AddWithValue("@pid", idPaciente);
        return Convert.ToInt32(crear.ExecuteScalar());
    }

    private static int? ObtenerPacienteId(NpgsqlConnection conexion, string identificador)
    {
        if (string.IsNullOrWhiteSpace(identificador)) return null;

        string texto = identificador.Trim();
        bool esNumero = int.TryParse(texto, out int idPaciente);
        string sql = esNumero
            ? "SELECT id_paciente FROM pacientes WHERE id_paciente = @id OR curp = @curp"
            : "SELECT id_paciente FROM pacientes WHERE curp = @curp";

        using var cmd = new NpgsqlCommand(sql, conexion);
        if (esNumero)
            cmd.Parameters.AddWithValue("@id", idPaciente);
        cmd.Parameters.AddWithValue("@curp", texto.ToUpperInvariant());

        object? result = cmd.ExecuteScalar();
        return result is null || result == DBNull.Value ? null : Convert.ToInt32(result);
    }

    private static int? ObtenerNotaEvolucionIdPorIndice(NpgsqlConnection conexion, int idPaciente, int indiceSignoVital)
    {
        if (indiceSignoVital < 0) return null;

        var idsNotas = new List<int>();
        using (var cmd = new NpgsqlCommand(
            "SELECT n.numero_expediente " +
            "FROM notas_de_evolucion n " +
            "JOIN historiales_medicos h ON h.codigo_historial = n.codigo_historial " +
            "WHERE h.id_paciente = @pid ORDER BY n.fecha ASC, n.hora ASC, n.numero_expediente ASC", conexion))
        {
            cmd.Parameters.AddWithValue("@pid", idPaciente);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
                idsNotas.Add(GetInt(reader, "numero_expediente"));
        }

        return indiceSignoVital >= idsNotas.Count ? null : idsNotas[indiceSignoVital];
    }

    private static int? ObtenerDoctorId(NpgsqlConnection conexion, string? nombreCompleto)
    {
        if (string.IsNullOrWhiteSpace(nombreCompleto)) return null;

        using var cmd = new NpgsqlCommand(
            "SELECT id_doctor FROM doctores " +
            "WHERE lower(btrim(nombre || ' ' || apellido_paterno || ' ' || apellido_materno)) = lower(btrim(@nombre)) " +
            "OR lower(COALESCE(NULLIF(btrim(usuario), ''), cedula_profesional)) = lower(btrim(@nombre)) " +
            "ORDER BY id_doctor LIMIT 1", conexion);
        cmd.Parameters.AddWithValue("@nombre", nombreCompleto.Trim());

        object? result = cmd.ExecuteScalar();
        return result is null || result == DBNull.Value ? null : Convert.ToInt32(result);
    }

    private static int? ObtenerPrimerDoctorId(NpgsqlConnection conexion)
    {
        using var cmd = new NpgsqlCommand("SELECT id_doctor FROM doctores ORDER BY id_doctor LIMIT 1", conexion);
        object? result = cmd.ExecuteScalar();
        return result is null || result == DBNull.Value ? null : Convert.ToInt32(result);
    }

    private static (string Paterno, string Materno) SepararApellidos(string apellido)
    {
        string[] partes = apellido.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (partes.Length == 0) return (string.Empty, string.Empty);
        if (partes.Length == 1) return (partes[0], string.Empty);

        return (partes[0], string.Join(' ', partes.Skip(1)));
    }

    private static (int Sistolica, int Diastolica) SepararPresion(string presion)
    {
        string[] partes = presion.Split('/', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        int sistolica = partes.Length > 0 && int.TryParse(partes[0], out int s) ? s : 0;
        int diastolica = partes.Length > 1 && int.TryParse(partes[1], out int d) ? d : 0;
        return (sistolica, diastolica);
    }

    private static DateTime UnirFechaHora(NpgsqlDataReader reader)
    {
        DateTime fecha = reader["fecha"] is DateTime f ? f.Date : DateTime.Today;
        TimeSpan hora = reader["hora"] is TimeSpan h ? h : TimeSpan.Zero;
        return fecha.Add(hora);
    }

    private static string NormalizarGenero(string texto)
    {
        if (string.IsNullOrWhiteSpace(texto)) return string.Empty;

        string valor = texto.Trim();
        if (valor.Equals("H", StringComparison.OrdinalIgnoreCase) ||
            valor.StartsWith("Masc", StringComparison.OrdinalIgnoreCase))
            return "Masculino";
        if (valor.Equals("M", StringComparison.OrdinalIgnoreCase) ||
            valor.StartsWith("Fem", StringComparison.OrdinalIgnoreCase))
            return "Femenino";

        return valor;
    }

    private static RolUsuario MapearRol(string rol)
    {
        return rol.Trim().ToLowerInvariant() switch
        {
            "medico" or "médico" or "doctor" => RolUsuario.Doctor,
            "enfermera" or "enfermero" => RolUsuario.Enfermera,
            "recepcionista" or "recepcion" or "recepción" => RolUsuario.Recepcionista,
            "administrador" or "admin" => RolUsuario.Administrador,
            _ => RolUsuario.Enfermera
        };
    }

    private static string UnirNombre(params string[] partes)
    {
        return string.Join(' ', partes.Where(p => !string.IsNullOrWhiteSpace(p)).Select(p => p.Trim()));
    }

    private static string GetString(NpgsqlDataReader reader, string columna)
    {
        object valor = reader[columna];
        return valor is null || valor == DBNull.Value ? string.Empty : Convert.ToString(valor)?.Trim() ?? string.Empty;
    }

    private static int GetInt(NpgsqlDataReader reader, string columna)
    {
        object valor = reader[columna];
        return valor is null || valor == DBNull.Value ? 0 : Convert.ToInt32(valor);
    }

    private static int? GetNullableInt(NpgsqlDataReader reader, string columna)
    {
        object valor = reader[columna];
        return valor is null || valor == DBNull.Value ? null : Convert.ToInt32(valor);
    }

    private static int GetIntFromText(NpgsqlDataReader reader, string columna)
    {
        string texto = GetString(reader, columna);
        return int.TryParse(texto, out int valor) ? valor : 0;
    }

    private static double GetDouble(NpgsqlDataReader reader, string columna)
    {
        object valor = reader[columna];
        return valor is null || valor == DBNull.Value ? 0 : Convert.ToDouble(valor);
    }
}
