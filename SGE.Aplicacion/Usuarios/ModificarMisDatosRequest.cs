namespace SGE.Aplicacion.Usuarios;

public record ModificarMisDatosRequest
{
    public Guid UsuarioId { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string NuevaContrasena { get; set; } = string.Empty;
}