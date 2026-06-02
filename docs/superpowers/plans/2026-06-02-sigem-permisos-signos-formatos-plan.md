# SIGEM Permisos Y Signos Por Formato Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implementar permisos reales por rol y ordenar/visualizar signos vitales en modos Nota de Evolucion e Historia Clinica, sin agregar nuevos campos sensibles de identidad del paciente.

**Architecture:** Agregar clases pequenas de dominio para permisos, filas de visualizacion de signos vitales y alertas clinicas. Las vistas WinForms consumiran esas clases para configurar menus, bloquear acciones no permitidas y mostrar el historial de signos vitales en orden compatible con los formatos revisados.

**Tech Stack:** C# 10+/net10.0-windows, Windows Forms, SIGEM.Modelo, SIGEM.Vista, SIGEM.Presentador, repositorios JSON/PostgreSQL existentes.

---

## File Structure

- Create: `SIGEM/Modelo/PermisosRol.cs`
  - Responsabilidad: centralizar todos los permisos por `RolUsuario`.
- Create: `SIGEM/Modelo/AlertaSignosVitales.cs`
  - Responsabilidad: representar una advertencia clinica simple con campo, valor y mensaje.
- Create: `SIGEM/Modelo/FormatoSignosVitales.cs`
  - Responsabilidad: enum para los modos `NotaEvolucion` e `HistoriaClinica`.
- Create: `SIGEM/Modelo/SignosVitalesVisualizacion.cs`
  - Responsabilidad: generar filas ordenadas para vista Nota de Evolucion, vista Historia Clinica y alertas de rango.
- Modify: `SIGEM/Vista/MenuPrincipalVista.cs`
  - Responsabilidad: usar `PermisosRol` para configurar menu, tarjetas y bloqueo de acciones.
- Modify: `SIGEM/Presentador/SigemPresentador.cs`
  - Responsabilidad: usar el visualizador para mostrar historial ordenado y alertas despues de cargar/guardar.
- Modify: `SIGEM/Vista/SigemVista.cs`
  - Responsabilidad: exponer una visualizacion clara de historial y mensajes de advertencia sin cambiar campos de identidad.
- Modify: `SIGEM.Tests/Program.cs`
  - Responsabilidad: agregar pruebas ligeras de permisos, orden de formatos y alertas antes o aparte de pruebas de base de datos.

---

### Task 1: Permisos Centralizados

**Files:**
- Create: `SIGEM/Modelo/PermisosRol.cs`
- Modify: `SIGEM.Tests/Program.cs`

- [ ] **Step 1: Add permission checks to the console tests**

Add this method to `SIGEM.Tests/Program.cs` near the existing `Assert` helper and call it before database-dependent checks:

```csharp
ProbarPermisosRol();

static void ProbarPermisosRol()
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
```

- [ ] **Step 2: Run build to see missing type failure**

Run: `dotnet build SIGEM.slnx`

Expected: build fails because `PermisosRol` does not exist yet.

- [ ] **Step 3: Create `PermisosRol`**

Create `SIGEM/Modelo/PermisosRol.cs`:

```csharp
namespace SIGEM.Modelo;

public sealed class PermisosRol
{
    private PermisosRol(RolUsuario rol)
    {
        Rol = rol;
    }

    public RolUsuario Rol { get; }

    public bool PuedeVerPacientes => Rol is RolUsuario.Doctor or RolUsuario.Enfermera or RolUsuario.Recepcionista;
    public bool PuedeAltaPaciente => Rol is RolUsuario.Doctor or RolUsuario.Enfermera or RolUsuario.Recepcionista;
    public bool PuedeEditarDatosPersonales => Rol is RolUsuario.Doctor or RolUsuario.Enfermera or RolUsuario.Recepcionista;
    public bool PuedeVerSignosVitales => Rol is RolUsuario.Doctor or RolUsuario.Enfermera;
    public bool PuedeCapturarSignosVitales => Rol is RolUsuario.Doctor or RolUsuario.Enfermera;
    public bool PuedeValidarSignosVitales => Rol is RolUsuario.Doctor;
    public bool PuedeVerHistoriaClinica => Rol is RolUsuario.Doctor or RolUsuario.Enfermera;
    public bool PuedeVerNotaEvolucion => Rol is RolUsuario.Doctor or RolUsuario.Enfermera;
    public bool PuedeAdministrarSistema => Rol is RolUsuario.Administrador;

    public static PermisosRol Para(RolUsuario rol) => new(rol);
}
```

- [ ] **Step 4: Run build**

Run: `dotnet build SIGEM.slnx`

Expected: build passes or shows only unrelated pre-existing errors. If unrelated errors appear, record them before continuing.

- [ ] **Step 5: Commit task**

Run:

```bash
git add SIGEM/Modelo/PermisosRol.cs SIGEM.Tests/Program.cs
git commit -m "Agregar permisos centralizados por rol"
```

---

### Task 2: Signos Vitales Ordenados Y Alertas

**Files:**
- Create: `SIGEM/Modelo/AlertaSignosVitales.cs`
- Create: `SIGEM/Modelo/FormatoSignosVitales.cs`
- Create: `SIGEM/Modelo/SignosVitalesVisualizacion.cs`
- Modify: `SIGEM.Tests/Program.cs`

- [ ] **Step 1: Add format and alert tests**

Add this call before database-dependent checks:

```csharp
ProbarVisualizacionSignosVitales();
```

Add this method near other test helpers:

```csharp
static void ProbarVisualizacionSignosVitales()
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
```

- [ ] **Step 2: Run build to verify missing types**

Run: `dotnet build SIGEM.slnx`

Expected: build fails for `FormatoSignosVitales`, `SignosVitalesVisualizacion` or `AlertaSignosVitales` missing.

- [ ] **Step 3: Create data records and enum**

Create `SIGEM/Modelo/FormatoSignosVitales.cs`:

```csharp
namespace SIGEM.Modelo;

public enum FormatoSignosVitales
{
    NotaEvolucion,
    HistoriaClinica
}
```

Create `SIGEM/Modelo/AlertaSignosVitales.cs`:

```csharp
namespace SIGEM.Modelo;

public sealed record AlertaSignosVitales(string Campo, string Valor, string Mensaje);
```

- [ ] **Step 4: Create visualizer**

Create `SIGEM/Modelo/SignosVitalesVisualizacion.cs`:

```csharp
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
            yield return new("Temperatura", $"{signos.Temperatura:F1} °C", "Temperatura fuera de rango esperado (36.0-37.5 °C).");
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
        yield return ("Temperatura", $"{signos.Temperatura:F1} °C");
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
        yield return ("Temperatura", $"{signos.Temperatura:F1} °C");
        yield return ("Talla/estatura", $"{signos.Estatura:F2} m");
        yield return ("IMC", signos.IMC > 0 ? signos.IMC.ToString("F2") : "--");
        yield return ("PAM", signos.PAM > 0 ? signos.PAM.ToString("F0") : "--");
        yield return ("Estado", signos.Validado ? "Validado" : "Pendiente");
    }
}
```

- [ ] **Step 5: Run build**

Run: `dotnet build SIGEM.slnx`

Expected: build passes or only unrelated pre-existing errors remain.

- [ ] **Step 6: Commit task**

Run:

```bash
git add SIGEM/Modelo/AlertaSignosVitales.cs SIGEM/Modelo/FormatoSignosVitales.cs SIGEM/Modelo/SignosVitalesVisualizacion.cs SIGEM.Tests/Program.cs
git commit -m "Ordenar signos vitales por formato clinico"
```

---

### Task 3: Integrar Permisos En Menu Principal

**Files:**
- Modify: `SIGEM/Vista/MenuPrincipalVista.cs`

- [ ] **Step 1: Add a permissions field**

At the top of `MenuPrincipalVista`, add:

```csharp
private readonly PermisosRol permisos;
```

In the constructor, after `this.usuario = usuario;`, add:

```csharp
permisos = PermisosRol.Para(usuario.Rol);
```

- [ ] **Step 2: Replace role-only menu logic**

Update `VerificarPermisosPorRol()` so it sets visibility from permissions:

```csharp
private void VerificarPermisosPorRol()
{
    btnPacientes.Visible = permisos.PuedeVerPacientes;
    btnConsulta.Visible = permisos.PuedeVerSignosVitales;
    btnAdministracion.Visible = permisos.PuedeAdministrarSistema;

    if (!btnPacientes.Visible && btnConsulta.Visible)
        btnConsulta.Location = btnPacientes.Location;
    if (!btnConsulta.Visible && btnAdministracion.Visible)
        btnAdministracion.Location = btnPacientes.Location;

    bool esEnfermeria = usuario.Rol == RolUsuario.Enfermera;
    bool esAdmin = usuario.Rol == RolUsuario.Administrador;

    Color fondoMenu = esEnfermeria ? Color.FromArgb(236, 253, 245) : Color.White;
    Color marca = esEnfermeria
        ? Color.FromArgb(16, 185, 129)
        : esAdmin ? Color.FromArgb(75, 85, 99) : Color.FromArgb(47, 124, 246);

    pnlMenu.BackColor = fondoMenu;
    btnPanelPrincipal.BackColor = fondoMenu;
    btnPacientes.BackColor = fondoMenu;
    btnConsulta.BackColor = fondoMenu;
    btnAdministracion.BackColor = fondoMenu;
    pnlMarca.BackColor = marca;
}
```

- [ ] **Step 3: Block methods as well as buttons**

At the start of `MostrarGestionPacientes()` add:

```csharp
if (!permisos.PuedeVerPacientes)
{
    MessageBox.Show("Tu rol no tiene permiso para gestionar pacientes.", "Permisos SIGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
    return;
}
```

At the start of `MostrarConsultaMedica()` add:

```csharp
if (!permisos.PuedeVerSignosVitales)
{
    MessageBox.Show("Tu rol no tiene permiso para consultar signos vitales.", "Permisos SIGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
    return;
}
```

At the start of `MostrarAdministracion()` replace the current nurse-only guard with:

```csharp
if (!permisos.PuedeAdministrarSistema)
{
    MessageBox.Show("Tu rol no tiene permiso para administrar el sistema.", "Permisos SIGEM", MessageBoxButtons.OK, MessageBoxIcon.Information);
    return;
}
```

- [ ] **Step 4: Restrict clinical tabs**

In `ConstruirConsultaMedica()`, only add medical tabs when `usuario.Rol == RolUsuario.Doctor` and keep recipe/diagnosis/treatment inaccessible for nurse and receptionist. If `subSeccionConsulta` is clinical and not permitted, call `SeleccionarConsulta("Nueva Consulta")`.

- [ ] **Step 5: Build**

Run: `dotnet build SIGEM.slnx`

Expected: build passes or only unrelated pre-existing errors remain.

- [ ] **Step 6: Commit task**

Run:

```bash
git add SIGEM/Vista/MenuPrincipalVista.cs
git commit -m "Aplicar permisos en menu principal"
```

---

### Task 4: Integrar Vista De Signos Por Formato

**Files:**
- Modify: `SIGEM/Presentador/SigemPresentador.cs`
- Modify: `SIGEM/Vista/SigemVista.cs`
- Modify: `SIGEM/Vista/ISigemVista.cs`

- [ ] **Step 1: Extend the view contract**

Add this method to `ISigemVista`:

```csharp
void MostrarAlertasSignosVitales(IReadOnlyList<string> alertas);
```

- [ ] **Step 2: Implement alert display in `SigemVista`**

Add the method to `SigemVista.cs`:

```csharp
public void MostrarAlertasSignosVitales(IReadOnlyList<string> alertas)
{
    if (alertas.Count == 0)
    {
        lblEstado.ForeColor = Color.FromArgb(38, 120, 79);
        return;
    }

    lblEstado.ForeColor = Color.FromArgb(217, 119, 6);
    lblEstado.Text = "Advertencias: " + string.Join(" | ", alertas);
}
```

- [ ] **Step 3: Replace history formatting in presenter**

In `SigemPresentador.ActualizarHistorial`, replace the manual string construction with `SignosVitalesVisualizacion`:

```csharp
private void ActualizarHistorial(Paciente paciente)
{
    var registros = paciente.SignosVitales
        .OrderByDescending(sv => sv.FechaHora)
        .Select((sv, i) => FormatearRegistroNotaEvolucion(i + 1, sv))
        .ToList();

    vista.MostrarHistorial(registros);
}

private static string FormatearRegistroNotaEvolucion(int indice, SignosVitales signos)
{
    string datos = string.Join(" | ", SignosVitalesVisualizacion
        .CrearFilas(signos, FormatoSignosVitales.NotaEvolucion)
        .Select(fila => $"{fila.Etiqueta}: {fila.Valor}"));

    return $"#{indice} - {datos}";
}
```

- [ ] **Step 4: Show alerts after loading or saving signs**

After `vista.MostrarPaciente(paciente);` in `BuscarPaciente`, add:

```csharp
MostrarAlertasDelUltimoRegistro(paciente);
```

After `ActualizarHistorial(pacienteActual!);` in `GuardarRegistro`, add:

```csharp
MostrarAlertasDelUltimoRegistro(pacienteActual!);
```

Add helper:

```csharp
private void MostrarAlertasDelUltimoRegistro(Paciente paciente)
{
    var ultimo = paciente.SignosVitales.OrderByDescending(sv => sv.FechaHora).FirstOrDefault();
    if (ultimo is null)
    {
        vista.MostrarAlertasSignosVitales([]);
        return;
    }

    var alertas = SignosVitalesVisualizacion.CrearAlertas(ultimo)
        .Select(alerta => $"{alerta.Campo}: {alerta.Valor}")
        .ToList();
    vista.MostrarAlertasSignosVitales(alertas);
}
```

- [ ] **Step 5: Build**

Run: `dotnet build SIGEM.slnx`

Expected: build passes or only unrelated pre-existing errors remain.

- [ ] **Step 6: Commit task**

Run:

```bash
git add SIGEM/Presentador/SigemPresentador.cs SIGEM/Vista/SigemVista.cs SIGEM/Vista/ISigemVista.cs
git commit -m "Mostrar signos vitales ordenados y alertas"
```

---

### Task 5: Verificacion Final

**Files:**
- No new files.

- [ ] **Step 1: Build full solution**

Run: `dotnet build SIGEM.slnx`

Expected: successful build. If it fails, record exact compiler errors and fix only errors caused by this plan.

- [ ] **Step 2: Run console checks if database is available**

Run: `dotnet run --project SIGEM.Tests/SIGEM.Tests.csproj`

Expected: `SIGEM.Tests OK` when PostgreSQL IMSS is configured. If PostgreSQL is unavailable, record that build verification passed and DB integration tests could not be executed in this environment.

- [ ] **Step 3: Manual role smoke test**

Launch SIGEM and check these accounts if available:

```text
admin / admin123: sees Administracion only, no clinical capture.
enfermera / enfermera123: sees signs capture and Nota de Evolucion, no Administracion.
recepcion / recepcion123: sees patient administration, no signs capture.
doctor or cedula doctor: sees clinical signs, Historia Clinica, Nota de Evolucion, and validation.
```

- [ ] **Step 4: Commit final verification note if code changed after previous commits**

Run:

```bash
git status --short
git add <only files changed by verification fixes>
git commit -m "Verificar permisos y signos vitales"
```

If no files changed, do not create an empty commit.

---

## Self-Review

- Spec coverage: permisos por rol covered in Tasks 1 and 3; signos ordenados and two format modes covered in Tasks 2 and 4; identity-sensitive expansion omitted by leaving `Paciente` unchanged; alerts covered in Tasks 2 and 4; verification covered in Task 5.
- Placeholder scan: no TBD/TODO/fill-in placeholders remain. Commands and code snippets are concrete.
- Type consistency: `PermisosRol`, `FormatoSignosVitales`, `AlertaSignosVitales`, and `SignosVitalesVisualizacion` are defined before being consumed.
