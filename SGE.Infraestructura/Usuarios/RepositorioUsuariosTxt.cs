using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Comun;

namespace SGE.Infraestructura.Usuarios;

public class RepositorioUsuariosTxt : IRepositorioUsuarios
{
    private readonly string _rutaArchivo;
    public RepositorioUsuariosTxt(string rutaArchivo)
    {
        _rutaArchivo = rutaArchivo;

        var carpeta = Path.GetDirectoryName(_rutaArchivo);

        if (!string.IsNullOrWhiteSpace(carpeta))
        {
            Directory.CreateDirectory(carpeta);
        }

        if (!File.Exists(_rutaArchivo))
        {
            File.Create(_rutaArchivo).Close();
        }
    }
    public Usuario? ObtenerPorCorreo(string correo)
    {
        return ObtenerTodos()
            .FirstOrDefault(u =>
                u.CorreoElectronico.Equals(
                    correo,
                    StringComparison.OrdinalIgnoreCase));
    }
    public void Agregar(Usuario usuario)
    {
        File.AppendAllLines(_rutaArchivo, new[] { Serializar(usuario) });
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return ObtenerTodos().FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        var lineas = File.ReadAllLines(_rutaArchivo);

        foreach (var linea in lineas)
        {
            if (string.IsNullOrWhiteSpace(linea))
            {
                continue;
            }

            yield return Deserializar(linea);
        }
    }

    public void Modificar(Usuario usuario)
    {
        var usuarios = ObtenerTodos().ToList();

        var indice = usuarios.FindIndex(e => e.Id == usuario.Id);

        if (indice == -1)
        {
            throw new RepositorioException("No se encontró el usuario a modificar.");
        }

        usuarios[indice] = usuario;

        SobrescribirArchivo(usuarios);
    }

    public void Eliminar(Guid id)
    {
        var usuarios = ObtenerTodos().ToList();

        var eliminado = usuarios.RemoveAll(e => e.Id == id);

        if (eliminado == 0)
        {
            throw new RepositorioException("No se encontró el usuario a eliminar.");
        }

        SobrescribirArchivo(usuarios);
    }

    private void SobrescribirArchivo(IEnumerable<Usuario> usuarios)
    {
        var lineas = usuarios.Select(Serializar);
        File.WriteAllLines(_rutaArchivo, lineas);
    }

    private string Serializar(Usuario usuario)
    {
        return string.Join('|',
            usuario.Id,
            usuario.Nombre,
            usuario.CorreoElectronico,
            usuario.ContrasenaHash,
            usuario.EsAdministrador
        );
    }

    private Usuario Deserializar(string linea)
    {
        var partes = linea.Split('|');

        return Usuario.Reconstruir(
            Guid.Parse(partes[0]),
            partes[1],
            partes[2],
            partes[3],
            bool.Parse(partes[4]),
            []
        );
    }
}