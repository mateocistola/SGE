using SGE.Aplicacion.Comun;
namespace SGE.Aplicacion.Usuarios;

public class ModificarMisDatosUseCase
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IHashService _hashService;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ModificarMisDatosUseCase(
        IRepositorioUsuarios repositorioUsuarios,
        IHashService hashService,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _hashService = hashService;
        _unidadDeTrabajo = unidadDeTrabajo;
    }
    public ModificarMisDatosResponse Ejecutar(
    ModificarMisDatosRequest request)
    {
        var usuario = _repositorioUsuarios.ObtenerPorId(request.UsuarioId);

        if (usuario is null)
        {
            throw new EntidadNoEncontradaException(
                "No se encontró el usuario.");
        }

        usuario.CambiarNombre(request.Nombre);

        if (!string.IsNullOrWhiteSpace(request.NuevaContrasena))
        {
            usuario.CambiarContrasena(
                _hashService.GenerarHash(request.NuevaContrasena));
        }

        _repositorioUsuarios.Modificar(usuario);
        _unidadDeTrabajo.Guardar();

        return new ModificarMisDatosResponse
        {
            Modificado = true
        };
    }
}
