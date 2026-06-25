using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Expedientes;

// Modificar caratula, ejemplo: Expediente mal hecho => Expediente bien hecho.
public class ModificarCaratulaExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarCaratulaExpedienteUseCase(
        IExpedienteRepository expedienteRepository,
        IAutorizacionService autorizacionService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
        _unidadDeTrabajo = unidadDeTrabajo;
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
        _unidadDeTrabajo.Guardar();

        return new ModificarCaratulaExpedienteResponse(expediente.Id);
    }
}