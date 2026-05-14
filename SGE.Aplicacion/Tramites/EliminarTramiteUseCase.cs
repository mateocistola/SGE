using System;
using System.Collections.Generic;
using System.Text;

using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.DTOs;

namespace SGE.Aplicacion.Tramites
{
    public class EliminarTramiteUseCase
    {
        private readonly ITramiteRepository _tramiteRepository;

        private readonly IExpedienteRepository _expedienteRepository;

        private readonly IAutorizacionService _autorizacionService;

        private readonly ActualizacionEstadoExpedienteService _actualizacionEstadoService;

        public EliminarTramiteUseCase(ITramiteRepository tramiteRepository, IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService, ActualizacionEstadoExpedienteService actualizacionEstadoService)
        {
            _tramiteRepository = tramiteRepository;

            _expedienteRepository = expedienteRepository;

            _autorizacionService = autorizacionService;

            _actualizacionEstadoService = actualizacionEstadoService;
            
        }

        public EliminarTramiteResponse Ejecutar(EliminarTramiteRequest request)           
        {
            if (!_autorizacionService.PoseeElPermiso(request.UsuarioId, Permiso.TramiteBaja))
            {
                throw new AutorizacionException("El usuario no posee permiso para eliminar trámites.");
            }
            var tramite = _tramiteRepository.ObtenerPorId(request.TramiteId);
            if (tramite is null)
            {
                throw new Exception("No existe un trámite con ese Id.");
            }
            _tramiteRepository.Eliminar(tramite.Id);
            _actualizacionEstadoService.Actualizar(tramite.ExpedienteId, request.UsuarioId);
            return new EliminarTramiteResponse
            {
                Eliminado = true
            };
        }
    }
}
