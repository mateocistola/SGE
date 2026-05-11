using SGE.Dominio.Expedientes;

namespace SGE.Aplicacion.Expedientes;

public record CambiarEstadoExpedienteRequest(Guid ExpedienteId, EstadoExpediente NuevoEstado, Guid IdUsuario);