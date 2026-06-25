using SGE.Dominio.Comun;

namespace SGE.Aplicacion.Usuarios;

public class LoginUseCase
{
    private readonly IRepositorioUsuarios _repositorioUsuarios;
    private readonly IHashService _hashService;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IRepositorioUsuarios repositorioUsuarios,
        IHashService hashService,
        ITokenService tokenService)
    {
        _repositorioUsuarios = repositorioUsuarios;
        _hashService = hashService;
        _tokenService = tokenService;
    }

    public string Ejecutar(string correo, string contrasena)
    {
        var usuario = _repositorioUsuarios.ObtenerPorCorreo(correo);

        if (usuario == null ||
            usuario.ContrasenaHash != _hashService.GenerarHash(contrasena))
        {
            throw new DominioException("Correo o contraseña incorrectos");
        }

        return _tokenService.GenerarToken(usuario.Id);
    }
}