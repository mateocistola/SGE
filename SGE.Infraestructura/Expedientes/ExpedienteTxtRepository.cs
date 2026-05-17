using SGE.Aplicacion.Expedientes;
using SGE.Dominio.Expedientes;
using SGE.Infraestructura.Comun;

namespace SGE.Infraestructura.Expedientes;

public class ExpedienteTxtRepository : IExpedienteRepository
{
    private readonly string _rutaArchivo;

    public ExpedienteTxtRepository(string rutaArchivo)
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

    public void Agregar(Expediente expediente)
    {
        File.AppendAllLines(_rutaArchivo, new[] { Serializar(expediente) });
    }

    public Expediente? ObtenerPorId(Guid id)
    {
        return ObtenerTodos().FirstOrDefault(e => e.Id == id);
    }

    public IEnumerable<Expediente> ObtenerTodos()
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

    public void Modificar(Expediente expediente)
    {
        var expedientes = ObtenerTodos().ToList();

        var indice = expedientes.FindIndex(e => e.Id == expediente.Id);

        if (indice == -1)
        {
            throw new RepositorioException("No se encontró el expediente a modificar.");
        }

        expedientes[indice] = expediente;

        SobrescribirArchivo(expedientes);
    }

    public void Eliminar(Guid id)
    {
        var expedientes = ObtenerTodos().ToList();

        var eliminado = expedientes.RemoveAll(e => e.Id == id);

        if (eliminado == 0)
        {
            throw new RepositorioException("No se encontró el expediente a eliminar.");
        }

        SobrescribirArchivo(expedientes);
    }

    private void SobrescribirArchivo(IEnumerable<Expediente> expedientes)
    {
        var lineas = expedientes.Select(Serializar);
        File.WriteAllLines(_rutaArchivo, lineas);
    }

    private string Serializar(Expediente expediente)
    {
        return string.Join('|',
            expediente.Id,
            expediente.Caratula.Valor,
            expediente.FechaCreacion.ToString("O"),
            expediente.FechaUltimaModificacion.ToString("O"),
            expediente.UsuarioUltimoCambio,
            expediente.Estado
        );
    }

    private Expediente Deserializar(string linea)
    {
        var partes = linea.Split('|');

        var id = Guid.Parse(partes[0]);
        var caratula = new Caratula(partes[1]);
        var fechaCreacion = DateTime.Parse(partes[2]);
        var fechaUltimaModificacion = DateTime.Parse(partes[3]);
        var usuarioUltimoCambio = Guid.Parse(partes[4]);
        var estado = Enum.Parse<EstadoExpediente>(partes[5]);

        return Expediente.Reconstruir(
            id,
            caratula,
            fechaCreacion,
            fechaUltimaModificacion,
            usuarioUltimoCambio,
            estado
        );
    }
}