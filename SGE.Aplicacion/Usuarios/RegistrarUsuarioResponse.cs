namespace SGE.Aplicacion.Usuarios;

public class RegistrarUsuarioResponse
{
    public Guid UsuarioId { get; }

    public RegistrarUsuarioResponse(Guid usuarioId)
    {
        UsuarioId = usuarioId;
    }
}