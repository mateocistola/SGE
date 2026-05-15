using System;
using System.Collections.Generic;
using System.Text;

namespace SGE.Aplicacion.Tramites.DTOs
{
    public record ListarTramitesResponse
    {
        public IEnumerable<TramiteResponseDto>
            Tramites { get; set; }
    }
}