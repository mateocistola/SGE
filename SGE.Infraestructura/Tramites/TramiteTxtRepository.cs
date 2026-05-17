using SGE.Aplicacion.Tramites;
using SGE.Dominio.Tramites;
using SGE.Infraestructura.Comun;

namespace SGE.Infraestructura.Tramites;

public class TramiteTxtRepository : ITramiteRepository
{
    private readonly string _rutaArchivo;

    public TramiteTxtRepository(string rutaArchivo)
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

    public void Agregar(Tramite tramite)
    {
        File.AppendAllLines(_rutaArchivo, new[] { Serializar(tramite) });
    }

    public Tramite? ObtenerPorId(Guid id)
    {
        return ObtenerTodos().FirstOrDefault(t => t.Id == id);
    }

    public IEnumerable<Tramite> ObtenerTodos()
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

    public IEnumerable<Tramite> ObtenerPorExpedienteId(Guid expedienteId)
    {
        return ObtenerTodos().Where(t => t.ExpedienteId == expedienteId);
    }

    public void Modificar(Tramite tramite)
    {
        var tramites = ObtenerTodos().ToList();

        var indice = tramites.FindIndex(t => t.Id == tramite.Id);

        if (indice == -1)
        {
            throw new RepositorioException("No se encontró el trámite a modificar.");
        }

        tramites[indice] = tramite;

        SobrescribirArchivo(tramites);
    }

    public void Eliminar(Guid id)
    {
        var tramites = ObtenerTodos().ToList();

        var eliminado = tramites.RemoveAll(t => t.Id == id);

        if (eliminado == 0)
        {
            throw new RepositorioException("No se encontró el trámite a eliminar.");
        }

        SobrescribirArchivo(tramites);
    }

    public void EliminarPorExpedienteId(Guid expedienteId)
    {
        var tramites = ObtenerTodos().ToList();

        var restantes = tramites
            .Where(t => t.ExpedienteId != expedienteId)
            .ToList();

        File.WriteAllLines(_rutaArchivo, restantes.Select(Serializar));
    }

    private void SobrescribirArchivo(IEnumerable<Tramite> tramites)
    {
        var lineas = tramites.Select(Serializar);
        File.WriteAllLines(_rutaArchivo, lineas);
    }

    private string Serializar(Tramite tramite)
    {
        return string.Join('|',
            tramite.Id,
            tramite.ExpedienteId,
            tramite.Etiqueta,
            tramite.Contenido.Valor,
            tramite.FechaCreacion.ToString("O"),
            tramite.FechaUltimaModificacion.ToString("O"),
            tramite.UsuarioUltimoCambio
        );
    }

    private Tramite Deserializar(string linea)
    {
        var partes = linea.Split('|');

        var id = Guid.Parse(partes[0]);
        var expedienteId = Guid.Parse(partes[1]);
        var etiqueta = Enum.Parse<EtiquetaTramite>(partes[2]);
        var contenido = new ContenidoTramite(partes[3]);
        var fechaCreacion = DateTime.Parse(partes[4]);
        var fechaUltimaModificacion = DateTime.Parse(partes[5]);
        var usuarioUltimoCambio = Guid.Parse(partes[6]);

        return Tramite.Reconstruir(
            id,
            expedienteId,
            etiqueta,
            contenido,
            fechaCreacion,
            fechaUltimaModificacion,
            usuarioUltimoCambio
        );
    }
}