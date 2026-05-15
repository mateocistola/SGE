using System;
using System.Collections.Generic;
using System.Text;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Tramites.DTOs;
using System.Linq;
using SGE.Dominio.Tramites;
namespace SGE.Aplicacion.Tramites
{
    public class ListarTramitesPorExpedienteUseCase
    {
        private readonly ITramiteRepository _tramiteRepository;

        private readonly IAutorizacionService _autorizacionService;

        public ListarTramitesPorExpedienteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService)
        {
            _tramiteRepository = tramiteRepository;

            _autorizacionService = autorizacionService;           
        }

        public ListarTramitesResponse Ejecutar(ListarTramitesRequest request)
        {
            var tramites = _tramiteRepository.ObtenerPorExpedienteId(request.ExpedienteId);
            var tramitesDto = tramites.Select(t => new TramiteResponseDto
            {
                Id = t.Id,
                Etiqueta = t.Etiqueta,
                Contenido = t.Contenido.Valor,
                FechaCreacion = t.FechaCreacion
            });
            return new ListarTramitesResponse
            {
                Tramites = tramitesDto
            };
        }
    }
}