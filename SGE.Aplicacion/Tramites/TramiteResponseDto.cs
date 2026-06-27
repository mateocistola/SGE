using SGE.Dominio.Tramites;

namespace SGE.Aplicacion.Tramites
{
    public record TramiteResponseDto
    {
        public Guid Id { get; set; }

        public EtiquetaTramite Etiqueta { get; set; }

        public required string Contenido { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}