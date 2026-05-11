using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;
// CON UseCase se ejecuta la accion, basicamente creo el expediente.
public class AgregarExpedienteUseCase
{
    // Sirve para guardar el expediente.
    private readonly IExpedienteRepository _expedienteRepository;

    //Sirve para preguntar si el usuario posee permiso.
    private readonly IAutorizacionService _autorizacionService;

    // Constructor.
    public AgregarExpedienteUseCase(IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService)
    {
        _expedienteRepository = expedienteRepository;
        _autorizacionService = autorizacionService;
    }

    public AgregarExpedienteResponse Ejecutar(AgregarExpedienteRequest request) // Traigo los datos que necesito de request.
    {
        // Me fijo si mi usuario tiene permiso de crear un expediente nuevo.
        if (!_autorizacionService.PoseeElPermiso(request.IdUsuario, Permiso.ExpedienteAlta))
        {
            // Si no lo tiene mando mensaje de Exception.
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