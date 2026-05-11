namespace SGE.Aplicacion.Expedientes;

// Datos necesarios para crear el expediente.
public record AgregarExpedienteRequest(string Caratula, Guid IdUsuario);