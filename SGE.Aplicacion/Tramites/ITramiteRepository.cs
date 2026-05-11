using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites;

public interface ITramiteRepository{
    void Agregar(Tramite tramite);

    void Modificar(Tramite tramite);

    void Eliminar(Guid id);

    void EliminarPorExpedienteId(Guid expedienteId);

    IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId);

    Tramite? ObtenerPorId(Guid id);
}