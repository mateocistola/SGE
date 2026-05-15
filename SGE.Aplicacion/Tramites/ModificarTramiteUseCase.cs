using System;
using System.Collections.Generic;
using System.Text;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.DTOs;

namespace SGE.Aplicacion.Tramites
{
    public class ModificarTramiteUseCase
    {
        private readonly ITramiteRepository _tramiteRepository;

        private readonly IExpedienteRepository _expedienteRepository;

        private readonly IAutorizacionService _autorizacionService;

        private readonly ActualizacionEstadoExpedienteService _actualizacionEstadoService;    

        public ModificarTramiteUseCase(ITramiteRepository tramiteRepository, IExpedienteRepository expedienteRepository, IAutorizacionService autorizacionService, ActualizacionEstadoExpedienteService actualizacionEstadoService)
        {
            _tramiteRepository = tramiteRepository;

            _expedienteRepository = expedienteRepository;

            _autorizacionService = autorizacionService;

            _actualizacionEstadoService = actualizacionEstadoService;    
        }

        public ModificarTramiteResponse Ejecutar(ModificarTramiteRequest request)
        {
            if (!_autorizacionService.PoseeElPermiso(request.UsuarioId, Permiso.TramiteModificacion))
            {
                throw new AutorizacionException(
                    "El usuario no posee permiso para modificar trámites.");
            }
            var tramite = _tramiteRepository.ObtenerPorId(request.TramiteId);
            if (tramite is null)
            {
                throw new Exception("No existe un trámite con ese Id.");
            }
            tramite.ModificarContenido(new ContenidoTramite(request.Contenido), request.UsuarioId);
            _tramiteRepository.Modificar(tramite);
            return new ModificarTramiteResponse
            {
                Modificado = true
            };
        }
    }
}