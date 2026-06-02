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
