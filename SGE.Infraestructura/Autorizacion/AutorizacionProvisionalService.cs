using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;

namespace SGE.Infraestructura.Autorizacion;

public class AutorizacionProvisionalService : IAutorizacionService
{
    public bool PoseeElPermiso(Guid idUsuario, Permiso permiso)
    {
        return true;
    }
}