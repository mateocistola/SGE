using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class RegistrarUsuarioUseCase
{
    private readonly IRepositorioUsuarios _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly IHashService _hashService;

    public RegistrarUsuarioUseCase(
        IRepositorioUsuarios usuarioRepository,
        IUnidadDeTrabajo unidadDeTrabajo,
        IHashService hashService)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
        _hashService = hashService;
    }

    public RegistrarUsuarioResponse Ejecutar(
        RegistrarUsuarioRequest request)
    {
        var existente = _usuarioRepository.ObtenerPorCorreo(
            request.CorreoElectronico);

        if (existente != null)
            throw new Exception("Ya existe un usuario con ese correo");

        var hash = _hashService.GenerarHash(request.Contrasena);

        var usuario = new Usuario(
            request.Nombre,
            request.CorreoElectronico,
            hash);

        _usuarioRepository.Agregar(usuario);
        _unidadDeTrabajo.Guardar();

        return new RegistrarUsuarioResponse(usuario.Id);
    }
}