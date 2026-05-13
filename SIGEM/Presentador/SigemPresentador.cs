using SIGEM.Modelo;
using SIGEM.Vista;

namespace SIGEM.Presentador;

public class SigemPresentador
{
    private readonly ISigemVista vista;
    private readonly ISigemRepositorio repositorio;

    public SigemPresentador(ISigemVista vista, ISigemRepositorio repositorio)
    {
        this.vista = vista;
        this.repositorio = repositorio;

        this.vista.GuardarSigemSolicitado += GuardarSigem;
        ActualizarVista();
    }

    private void GuardarSigem(object? sender, EventArgs e)
    {
        string nombre = vista.NombreSigem.Trim();

        if (string.IsNullOrWhiteSpace(nombre))
        {
            vista.MostrarError("Escribe un nombre antes de guardar.");
            return;
        }

        Sigem sigem = new(nombre);
        repositorio.Guardar(sigem);

        vista.LimpiarFormulario();
        ActualizarVista();
        vista.MostrarMensaje("Registro guardado correctamente en sigem.txt.");
    }

    private void ActualizarVista()
    {
        IReadOnlyList<string> nombres = repositorio
            .ObtenerTodos()
            .Select(sigem => sigem.Nombre)
            .ToList();

        vista.MostrarRegistros(nombres);
    }
}
