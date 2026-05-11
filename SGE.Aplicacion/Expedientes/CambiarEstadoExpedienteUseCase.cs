using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Expedientes;

public class CambiarEstadoExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;

    public CambiarEstadoExpedienteUseCase(IExpedienteRepository expedienteRepository,
                                          IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
    }

    public CambiarEstadoExpedienteResponse Ejecutar(CambiarEstadoExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permiso para modificar expedientes");
        }

        var expediente = _expedienteRepository.ObtenerPorId(request.ExpedienteId);

        if (expediente is null)
        {
            throw new EntidadNoEncontradaException("No se encontró el expediente");
        }

        expediente.CambiarEstado(request.NuevoEstado, request.IdUsuario);

        _expedienteRepository.Modificar(expediente);

        return new CambiarEstadoExpedienteResponse(expediente.Id);
    }
}