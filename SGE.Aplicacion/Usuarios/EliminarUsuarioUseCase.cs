using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class EliminarUsuarioUseCase
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public EliminarUsuarioUseCase(
        IRepositorioUsuarios repositorioUsuarios,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public EliminarUsuarioResponse Ejecutar(
        EliminarUsuarioRequest request)
    {
        var administrador = _repositorioUsuarios.ObtenerPorId(request.UsuarioAdministradorId);

        if (administrador is null || !administrador.EsAdministrador)
        {
            throw new AutorizacionException(
                "Solo un administrador puede eliminar usuarios.");
        }

        var usuario = _repositorioUsuarios.ObtenerPorId(request.UsuarioId);

        if (usuario is null)
        {
            throw new EntidadNoEncontradaException(
                "No se encontró el usuario.");
        }

        _repositorioUsuarios.Eliminar(request.UsuarioId);

        _unidadDeTrabajo.Guardar();

        return new EliminarUsuarioResponse
        {
            Eliminado = true
        };
    }
}