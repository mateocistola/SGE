using SGE.Dominio.Comun;

namespace SGE.Dominio.Usuarios;

public class Usuario
{
    public Guid Id { get; private set; }
    public string Nombre { get; private set; }
    public string CorreoElectronico { get; private set; }
    public string ContrasenaHash { get; private set; }
    public bool EsAdministrador { get; private set; }

    private readonly List<Permiso> _permisos = [];
    public IReadOnlyCollection<Permiso> Permisos => _permisos.AsReadOnly();

    public Usuario(
        string nombre,
        string correoElectronico,
        string contrasenaHash)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DominioException("El nombre es obligatorio");

        if (string.IsNullOrWhiteSpace(correoElectronico))
            throw new DominioException("El correo es obligatorio");

        if (string.IsNullOrWhiteSpace(contrasenaHash))
            throw new DominioException("La contraseña es obligatoria");

        Id = Guid.NewGuid();
        Nombre = nombre;
        CorreoElectronico = correoElectronico;
        ContrasenaHash = contrasenaHash;
        EsAdministrador = false;
    }

    public static Usuario Reconstruir(
        Guid id,
        string nombre,
        string correoElectronico,
        string contrasenaHash,
        bool esAdministrador,
        IEnumerable<Permiso> permisos)
    {
        var usuario = new Usuario(
            nombre,
            correoElectronico,
            contrasenaHash);

        usuario.Id = id;
        usuario.EsAdministrador = esAdministrador;

        foreach (var permiso in permisos)
        {
            usuario._permisos.Add(permiso);
        }

        return usuario;
    }

    public void CambiarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new DominioException("El nombre es obligatorio");

        Nombre = nombre;
    }

    public void CambiarContrasena(string hash)
    {
        if (string.IsNullOrWhiteSpace(hash))
            throw new DominioException("La contraseña es obligatoria");

        ContrasenaHash = hash;
    }

    public void AsignarPermiso(Permiso permiso)
    {
        if (!_permisos.Contains(permiso))
            _permisos.Add(permiso);
    }

    public void QuitarPermiso(Permiso permiso)
    {
        _permisos.Remove(permiso);
    }

    public void ConvertirEnAdministrador()
    {
        EsAdministrador = true;
    }
    public bool PoseeElPermiso(Permiso permiso)
    {
        return EsAdministrador || _permisos.Contains(permiso);
    }
}