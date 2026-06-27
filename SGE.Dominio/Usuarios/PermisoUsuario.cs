namespace SGE.Dominio.Usuarios;

public class PermisoUsuario
{
    private PermisoUsuario()
    {
    }

    public int Id { get; private set; }

    public Permiso Permiso { get; private set; }

    public PermisoUsuario(Permiso permiso)
    {
        Permiso = permiso;
    }
}