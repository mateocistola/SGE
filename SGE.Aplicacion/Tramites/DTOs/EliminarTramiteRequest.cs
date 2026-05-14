using System;
using System.Collections.Generic;
using System.Text;

namespace SGE.Aplicacion.Tramites.DTOs
{
    public record EliminarTramiteRequest
    {
        public Guid TramiteId { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
