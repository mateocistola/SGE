using SGE.Dominio.Usuarios;

namespace SGE.Aplicacion.Usuarios;

public record UsuarioItemResponse(
    Guid Id,
    string Nombre,
    string CorreoElectronico,
    bool EsAdministrador,
    IEnumerable<Permiso> Permisos
);

public record ListarUsuariosResponse(
    IEnumerable<UsuarioItemResponse> Usuarios
);