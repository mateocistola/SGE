namespace SGE.Infraestructura.Comun;

// Se usa para encapsular errores relacionados con el repositorio, como problemas de conexión a la base
// de datos o errores al ejecutar consultas.
public class RepositorioException : Exception
{
    public RepositorioException(string mensaje) : base(mensaje)
    {
    }
}