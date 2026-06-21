using Microsoft.EntityFrameworkCore;
using SGE.Aplicacion.Usuarios;
using SGE.Dominio.Usuarios;
using SGE.Infraestructura.Persistencia;

namespace SGE.Infraestructura.Usuarios;

public class UsuarioRepository : IRepositorioUsuarios
{
    private readonly SgeContext _context;

    public UsuarioRepository(SgeContext context)
    {
        _context = context;
    }

    public void Agregar(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
    }

    public void Modificar(Usuario usuario)
    {
        _context.Usuarios.Update(usuario);
    }

    public void Eliminar(Guid id)
    {
        var usuario = ObtenerPorId(id);

        if (usuario != null)
        {
            _context.Usuarios.Remove(usuario);
        }
    }

    public Usuario? ObtenerPorId(Guid id)
    {
        return _context.Usuarios
            .FirstOrDefault(u => u.Id == id);
    }

    public Usuario? ObtenerPorCorreo(string correo)
    {
        return _context.Usuarios
            .FirstOrDefault(u => u.CorreoElectronico == correo);
    }

    public IEnumerable<Usuario> ObtenerTodos()
    {
        return _context.Usuarios.ToList();
    }
}