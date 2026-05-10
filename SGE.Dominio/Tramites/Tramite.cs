using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public class Tramite
{
    public Guid Id { get; private set; }
    public Guid ExpedienteId { get; private set; }
    public EtiquetaTramite Etiqueta { get; private set; }
    public ContenidoTramite Contenido { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }

    public Tramite(
        Guid expedienteId,
        EtiquetaTramite etiqueta,
        ContenidoTramite contenido,
        Guid usuarioUltimoCambio)
    {
        if (expedienteId == Guid.Empty)
        {
            throw new DominioException("El expediente es obligatorio");
        }

        if (usuarioUltimoCambio == Guid.Empty)
        {
            throw new DominioException("El usuario es obligatorio");
        }

        Id = Guid.NewGuid();
        ExpedienteId = expedienteId;
        Etiqueta = etiqueta;
        Contenido = contenido;
        UsuarioUltimoCambio = usuarioUltimoCambio;

        FechaCreacion = DateTime.UtcNow;
        FechaUltimaModificacion = FechaCreacion;
    }
    public static Tramite Reconstruir(
        Guid id,
        Guid expedienteId,
        EtiquetaTramite etiqueta,
        ContenidoTramite contenido,
        DateTime fechaCreacion,
        DateTime fechaUltimaModificacion,
        Guid usuarioUltimoCambio)
    {
        if (id == Guid.Empty)
            throw new DominioException("Id inválido");

        if (fechaUltimaModificacion < fechaCreacion)
            throw new DominioException("Fecha inválida");

        var tramite = new Tramite(
            expedienteId,
            etiqueta,
            contenido,
            usuarioUltimoCambio
        );

        tramite.Id = id;
        tramite.FechaCreacion = fechaCreacion;
        tramite.FechaUltimaModificacion = fechaUltimaModificacion;

        return tramite;
    }
    public void ModificarContenido(
        ContenidoTramite nuevoContenido,
        Guid usuarioId)
    {
        if(usuarioId == Guid.Empty)
            throw new DominioException("Usuario inválido");

        Contenido = nuevoContenido;
        UsuarioUltimoCambio = usuarioId;
        FechaUltimaModificacion = DateTime.UtcNow;
    }
}