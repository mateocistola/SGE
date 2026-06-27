namespace SGE.Aplicacion.Usuarios;

public record EliminarUsuarioRequest
{
    public Guid UsuarioId { get; set; }

    public Guid UsuarioAdministradorId { get; set; }
}