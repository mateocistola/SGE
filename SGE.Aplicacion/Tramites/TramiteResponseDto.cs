using SGE.Dominio.Tramites;
using System;
using System.Collections.Generic;
using System.Text;

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