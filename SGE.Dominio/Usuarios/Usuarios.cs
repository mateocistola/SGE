using SGE.Dominio.Comun;

namespace SGE.Dominio.Usuarios;

public class Usuario
{
protected Usuario()
{
}

public Guid Id { get; private set; }

public string Nombre { get; private set; } = string.Empty;

public string CorreoElectronico { get; private set; } = string.Empty;

public string ContrasenaHash { get; private set; } = string.Empty;

public bool EsAdministrador { get; private set; }

public List<PermisoUsuario> Permisos { get; private set; } = [];

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
    if (!Permisos.Any(p => p.Permiso == permiso))
        Permisos.Add(new PermisoUsuario(permiso));
}

    public void QuitarPermiso(Permiso permiso)
    {
        var existente = Permisos.FirstOrDefault(p => p.Permiso == permiso);

        if (existente != null)
            Permisos.Remove(existente);
    }

    public bool PoseeElPermiso(Permiso permiso)
    {
        return EsAdministrador ||
            Permisos.Any(p => p.Permiso == permiso);
    }

    public void ConvertirEnAdministrador()
    {
        EsAdministrador = true;
    }
}