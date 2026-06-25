using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Autorizacion;

public class AutorizacionService : IAutorizacionService
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;

    public AutorizacionService(IRepositorioUsuarios repositorioUsuarios)
    {
        _repositorioUsuarios = repositorioUsuarios;
    }

    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        var usuario = _repositorioUsuarios.ObtenerPorId(idUsuario);

        if (usuario == null)
            return false;

        return usuario.PoseeElPermiso(permiso);
    }
}