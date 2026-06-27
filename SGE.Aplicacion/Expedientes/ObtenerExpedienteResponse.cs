using SGE.Aplicacion.Tramites;
using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public record ObtenerExpedienteResponse(
    Guid Id,
    string Caratula,
    EstadoExpediente Estado,
    DateTime FechaCreacion,
    DateTime FechaUltimaModificacion,
    Guid UsuarioUltimoCambio,
    IEnumerable<TramiteResponseDto> Tramites
);