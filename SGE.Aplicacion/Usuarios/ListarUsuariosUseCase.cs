using SGE.Aplicacion.Autorizacion;
using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public class ListarUsuariosUseCase
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;

    public ListarUsuariosUseCase(
        IRepositorioUsuarios repositorioUsuarios)
    {
        _repositorioUsuarios = repositorioUsuarios;
    }

    public ListarUsuariosResponse Ejecutar(
        ListarUsuariosRequest request)
    {
        var administrador =
            _repositorioUsuarios.ObtenerPorId(request.UsuarioAdministradorId);

        if (administrador is null || !administrador.EsAdministrador)
        {
            throw new AutorizacionException(
                "Solo un administrador puede listar usuarios.");
        }

        var usuarios = _repositorioUsuarios.ObtenerTodos();

        var response = usuarios.Select(u =>
            new UsuarioItemResponse(
                u.Id,
                u.Nombre,
                u.CorreoElectronico,
                u.EsAdministrador,
                u.Permisos));

        return new ListarUsuariosResponse(response);
    }
}
