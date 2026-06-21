using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Persistencia;

namespace SGE.Infraestructura.Tramites;
public class TramiteRepository : ITramiteRepository
{
    
    private readonly SgeContext _context;

    public TramiteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Tramite tramite)
    {
        _context.Tramites.Add(tramite);
    }

    public void Modificar(Tramite tramite)
    {
        _context.Tramites.Update(tramite);
    }

    public void Eliminar(Guid id)
    {
        var tramite = ObtenerPorId(id);

        if (tramite != null)
            _context.Tramites.Remove(tramite);
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return _context.Tramites.FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerTodos()
    {
        return _context.Tramites.ToList();
    }
    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return _context.Tramites
            .Where(t => t.ExpedienteId == expedienteId)
            .ToList();
    }

    public void EliminarPorExpedienteId(Guid expedienteId)
    {
        var tramites = _context.Tramites
            .Where(t => t.ExpedienteId == expedienteId)
            .ToList();

        _context.Tramites.RemoveRange(tramites);
    }
}