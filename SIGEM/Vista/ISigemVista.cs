namespace SIGEM.Vista;

public interface ISigemVista
{
    event EventHandler? GuardarSigemSolicitado;

    string NombreSigem { get; }

    void MostrarRegistros(IReadOnlyList<string> nombres);

    void MostrarMensaje(string mensaje);

    void MostrarError(string mensaje);

    void LimpiarFormulario();
}
