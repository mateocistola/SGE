namespace SGE.Aplicacion.Autorizacion;

public class AutorizacionException : Exception
{
    // Anuncio que aparece si el id no tiene permiso.
    public AutorizacionException(string mensaje)
        : base(mensaje)
    {
    }
}