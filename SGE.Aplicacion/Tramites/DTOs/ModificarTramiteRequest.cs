using SGE.Dominio.Tramites;
using System;
using System.Collections.Generic;
using System.Text;
namespace SGE.Aplicacion.Tramites.DTOs
{
    public record ModificarTramiteRequest
    {
        public Guid TramiteId { get; set; }

        public string Contenido { get; set; }

        public Guid UsuarioId { get; set; }
    }
}