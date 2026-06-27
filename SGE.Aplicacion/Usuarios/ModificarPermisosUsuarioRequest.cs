using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public record ModificarPermisosUsuarioRequest
{
    public Guid UsuarioId { get; set; }

    public Guid UsuarioAdministradorId { get; set; }

    public IEnumerable<Permiso> Permisos { get; set; } = [];
}