using SGE.Aplicacion.Comun;
using SGE.Aplicacion.Tramites;

namespace SGE.Aplicacion.Expedientes;

public class ObtenerExpedienteUseCase
{
    private readonly IExpedienteRepository _expedienteRepository;
    private readonly ITramiteRepository _tramiteRepository;

    public ObtenerExpedienteUseCase(
        IExpedienteRepository expedienteRepository,
        ITramiteRepository tramiteRepository)
    {
        _expedienteRepository = expedienteRepository;
        _tramiteRepository = tramiteRepository;
    }

    public ObtenerExpedienteResponse Ejecutar(
        ObtenerExpedienteRequest request)
    {
        var expediente = _expedienteRepository.ObtenerPorId(request.ExpedienteId);

        if (expediente is null)
        {
            throw new EntidadNoEncontradaException(
                "No se encontró el expediente.");
        }

        var tramites = _tramiteRepository
            .ObtenerPorExpedienteId(request.ExpedienteId)
            .Select(t => new TramiteResponseDto
            {
                Id = t.Id,
                Etiqueta = t.Etiqueta,
                Contenido = t.Contenido.Valor,
                FechaCreacion = t.FechaCreacion
            });

        return new ObtenerExpedienteResponse(
            expediente.Id,
            expediente.Caratula.Valor,
            expediente.Estado,
            expediente.FechaCreacion,
            expediente.FechaUltimaModificacion,
            expediente.UsuarioUltimoCambio,
            tramites
        );
    }
}