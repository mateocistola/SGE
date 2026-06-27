using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites
{
    public record AgregarTramiteRequest
    {
        public Guid ExpedienteId { get; set; }

        public EtiquetaTramite Etiqueta { get; set; }

        public required string Contenido { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
