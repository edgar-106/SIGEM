namespace SIGEM.Modelo;

public interface ISigemRepositorio
{
    IReadOnlyList<Sigem> ObtenerTodos();

    void Guardar(Sigem sigem);
}
