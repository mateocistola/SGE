using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class RegistrarUsuarioUseCase
{
    private readonly IRepositorioUsuarios _usuarioRepository;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    public RegistrarUsuarioUseCase(
        IRepositorioUsuarios usuarioRepository, IUnidadDeTrabajo unidadDeTrabajo)
    {
        _usuarioRepository = usuarioRepository;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public RegistrarUsuarioResponse Ejecutar(
        RegistrarUsuarioRequest request)
    {
        var existente =
            _usuarioRepository.ObtenerPorCorreo(
                request.CorreoElectronico);

        if (existente != null)
        {
            throw new Exception(
                "Ya existe un usuario con ese correo");
        }

        var usuario = new Usuario(
            request.Nombre,
            request.CorreoElectronico,
            request.Contrasena);

        _usuarioRepository.Agregar(usuario);
        _unidadDeTrabajo.Guardar();
        return new RegistrarUsuarioResponse(
            usuario.Id);
    }
}