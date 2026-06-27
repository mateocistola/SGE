using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class ModificarPermisosUsuarioUseCase
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarPermisosUsuarioUseCase(
        IRepositorioUsuarios repositorioUsuarios,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public ModificarPermisosUsuarioResponse Ejecutar(
        ModificarPermisosUsuarioRequest request)
    {
        var administrador = _repositorioUsuarios.ObtenerPorId(request.UsuarioAdministradorId);

        if (administrador is null || !administrador.EsAdministrador)
        {
            throw new AutorizacionException(
                "Solo un administrador puede modificar permisos.");
        }

        var usuario = _repositorioUsuarios.ObtenerPorId(request.UsuarioId);

        if (usuario is null)
        {
            throw new EntidadNoEncontradaException(
                "No se encontró el usuario.");
        }

        // Elimina todos los permisos actuales
        foreach (var permiso in Enum.GetValues<Permiso>())
        {
            usuario.QuitarPermiso(permiso);
        }

        // Asigna los nuevos permisos
        foreach (var permiso in request.Permisos)
        {
            usuario.AsignarPermiso(permiso);
        }

        _repositorioUsuarios.Modificar(usuario);

        _unidadDeTrabajo.Guardar();

        return new ModificarPermisosUsuarioResponse
        {
            Modificado = true
        };
    }
}