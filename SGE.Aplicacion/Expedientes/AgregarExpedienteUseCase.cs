using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;
public class AgregarExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly IAutorizacionService _autorizacionService;

    // Constructor.
    public AgregarExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request) // Traigo los datos que necesito de request.
    {
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            throw new AutorizacionException("El usuario no tiene permiso para crear expedientes");
        }
        // Creo un expediente nuevo.
        var expediente = new Expediente(new Caratula(request.Caratula), request.IdUsuario);
        // Guardo el expediente creado.
        _expedienteRepository.Agregar(expediente);
        // Retorno el ID del nuevo expediente.
        return new AgregarExpedienteResponse(expediente.Id);
    }
}