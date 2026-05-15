using SGE.Dominio.Tramites;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGE.Aplicacion.Tramites.DTOs
{
    public record TramiteResponseDto
    {
        public Guid Id { get; set; }

        public EtiquetaTramite Etiqueta { get; set; }

        public string Contenido { get; set; }

        public DateTime FechaCreacion { get; set; }
    }
}