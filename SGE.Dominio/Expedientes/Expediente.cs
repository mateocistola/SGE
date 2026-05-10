using SGE.Dominio.Comun;
using SGE.Dominio.Tramites;

namespace SGE.Dominio.Expedientes;

public class Expediente
{
    public Guid Id { get; private set; }
    public Caratula Caratula { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime FechaUltimaModificacion { get; private set; }
    public Guid UsuarioUltimoCambio { get; private set; }
    public EstadoExpediente Estado { get; private set; }

    public Expediente(
        Caratula caratula,
        Guid usuarioUltimoCambio)
    {
        if (usuarioUltimoCambio == Guid.Empty)
        {
            throw new DominioException("El usuario es obligatorio");
        }

        Id = Guid.NewGuid();
        Caratula = caratula;
        UsuarioUltimoCambio = usuarioUltimoCambio;

        FechaCreacion = DateTime.UtcNow;
        FechaUltimaModificacion = FechaCreacion;

        Estado = EstadoExpediente.RecienIniciado;
    }
    public static Expediente Reconstruir(
        Guid id,
        Caratula caratula,
        DateTime fechaCreacion,
        DateTime fechaUltimaModificacion,
        Guid usuarioUltimoCambio,
        EstadoExpediente estado)
    {
        if (id == Guid.Empty)
            throw new DominioException("Id inválido");

        if (usuarioUltimoCambio == Guid.Empty)
            throw new DominioException("Usuario inválido");

        if (fechaUltimaModificacion < fechaCreacion)
            throw new DominioException("Fecha inválida");

        return new Expediente(caratula, usuarioUltimoCambio)
        {
            Id = id,
            FechaCreacion = fechaCreacion,
            FechaUltimaModificacion = fechaUltimaModificacion,
            Estado = estado
        };
    }
    public void ModificarCaratula(
        Caratula nuevaCaratula,
        Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new DominioException("El usuario es obligatorio");
        }

        Caratula = nuevaCaratula;
        UsuarioUltimoCambio = usuarioId;
        FechaUltimaModificacion = DateTime.UtcNow;
    }
    public bool ActualizarEstado(
        EtiquetaTramite? ultimaEtiqueta,
        Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new DominioException("El usuario es obligatorio");
        }

        EstadoExpediente nuevoEstado = Estado;

        if (ultimaEtiqueta == null)
        {
            nuevoEstado = EstadoExpediente.RecienIniciado;
        }
        else if (ultimaEtiqueta == EtiquetaTramite.Resolucion)
        {
            nuevoEstado = EstadoExpediente.ConResolucion;
        }
        else if (ultimaEtiqueta == EtiquetaTramite.PaseAEstudio)
        {
            nuevoEstado = EstadoExpediente.ParaResolver;
        }
        else if (ultimaEtiqueta == EtiquetaTramite.PaseAlArchivo)
        {
            nuevoEstado = EstadoExpediente.Finalizado;
        }

        if (nuevoEstado == Estado)
        {
            return false;
        }

        Estado = nuevoEstado;
        UsuarioUltimoCambio = usuarioId;
        FechaUltimaModificacion = DateTime.UtcNow;

        return true;
    }
    public void CambiarEstado(
        EstadoExpediente nuevoEstado,
        Guid usuarioId)
    {
        if (usuarioId == Guid.Empty)
        {
            throw new DominioException("El usuario es obligatorio");
        }

        Estado = nuevoEstado;
        UsuarioUltimoCambio = usuarioId;
        FechaUltimaModificacion = DateTime.UtcNow;
    }
}