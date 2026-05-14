using System;
using System.Collections.Generic;
using System.Text;

namespace SGE.Aplicacion.Tramites.DTOs
{
    public record AgregarTramiteResponse
    {
        public Guid TramiteId { get; set; }
    }
}
