namespace SGE.Aplicacion.Expedientes;

public record ModificarCaratulaExpedienteRequest(Guid ExpedienteId, string NuevaCaratula, Guid IdUsuario);