namespace SGE.Aplicacion.Expedientes;

public class ListarExpedientesUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;

    public ListarExpedientesUseCase(IExpedienteRepository expedienteRepository)
    {
        _expedienteRepository = expedienteRepository;
    }

    public ListarExpedientesResponse Ejecutar()
    {
        var expedientes = _expedienteRepository.ObtenerTodos();

        var expedientesResponse = expedientes.Select(expediente => new ExpedienteItemResponse(
                expediente.Id,
                expediente.Caratula.Valor,
                expediente.FechaCreacion,
                expediente.FechaUltimaModificacion,
                expediente.UsuarioUltimoCambio,
                expediente.Estado
            )
        );

        return new ListarExpedientesResponse(expedientesResponse);
    }
}