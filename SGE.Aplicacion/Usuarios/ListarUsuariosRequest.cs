namespace SGE.Aplicacion.Usuarios;

public record ListarUsuariosRequest
{
    public Guid UsuarioAdministradorId { get; set; }
}