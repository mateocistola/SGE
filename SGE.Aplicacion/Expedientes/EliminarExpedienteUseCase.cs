using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Tramites;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes;

// Eliminar los expedientes en cascada.
public class EliminarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarExpedienteUseCase(IExpedienteRepository expedienteRepository,
                                        ITramiteRepository tramiteRepository,
                                        IAutorizacionService autorizacionService,
                                        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarExpedienteResponse Ejecutar(EliminarExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteBaja))
        {
            throw new AutorizacionException("El usuario no tiene permiso para eliminar expedientes");
        }

        var expediente = _expedienteRepository.ObtenerPorId(request.ExpedienteId);

        if (expediente is null)
        {
            throw new EntidadNoEncontradaException("No se encontró el expediente");
        }

        // Primero se eliminan los trámites asociados.
        _tramiteRepository.EliminarPorExpedienteId(request.ExpedienteId);

        // Después se elimina el expediente.
        _expedienteRepository.Eliminar(request.ExpedienteId);
        _unidadDeTrabajo.Guardar();

        return new EliminarExpedienteResponse(request.ExpedienteId);
    }
}