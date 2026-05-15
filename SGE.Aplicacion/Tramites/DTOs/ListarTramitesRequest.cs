using System;
using System.Collections.Generic;
using System.Text;

namespace SGE.Aplicacion.Tramites.DTOs
{
    public record ListarTramitesRequest
    {
        public Guid ExpedienteId { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
