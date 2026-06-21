using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Infraestructura.Persistencia;

namespace SGE.Infraestructura.Expedientes;

public class ExpedienteRepository : IExpedienteRepository
{
    private readonly SgeContext _context;

    public ExpedienteRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Expediente expediente)
    {
        _context.Expedientes.Add(expediente);
    }

    public void Modificar(Expediente expediente)
    {
        _context.Expedientes.Update(expediente);
    }

    public void Eliminar(Guid id)
    {
        var expediente = ObtenerPorId(id);

        if (expediente != null)
        {
            _context.Expedientes.Remove(expediente);
        }
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        return _context.Expedientes.FirstOrDefault(x => x.Id == id);
    }

    public IEnumerable<Expediente> ObtenerTodos()
    {
        return _context.Expedientes.ToList();
    }
}