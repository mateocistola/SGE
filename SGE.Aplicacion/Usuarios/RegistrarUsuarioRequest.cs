namespace SGE.Aplicacion.Usuarios;

public class RegistrarUsuarioRequest
{
    public string Nombre { get; set; } = string.Empty;

    public string CorreoElectronico { get; set; } = string.Empty;

    public string Contrasena { get; set; } = string.Empty;
}