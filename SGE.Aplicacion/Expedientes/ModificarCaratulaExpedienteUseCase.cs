using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

// Modificar caratula, ejemplo: Expediente mal hecho => Expediente bien hecho.
public class ModificarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;

    public ModificarCaratulaExpedienteUseCase(IExpedienteRepository expedienteRepository, 
                                              IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
    }

    public ModificarCaratulaExpedienteResponse Ejecutar(ModificarCaratulaExpedienteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteModificacion))
        {
            throw new AutorizacionException("El usuario no tiene permiso para modificar expedientes");
        }

        var expediente = _expedienteRepository.ObtenerPorId(request.ExpedienteId);

        if (expediente == null)
        {
            throw new EntidadNoEncontradaException("No se encontró el expediente");
        }

        expediente.ModificarCaratula(new Caratula(request.NuevaCaratula),  request.IdUsuario);

        _expedienteRepository.Modificar (expediente);

        return new ModificarCaratulaExpedienteResponse(expediente.Id);
    }
}