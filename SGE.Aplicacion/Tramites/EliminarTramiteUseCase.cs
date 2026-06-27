using SGE.Dominio.Usuarios;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Tramites
{
    public class EliminarTramiteUseCase
    {
        private readonly ITramiteRepository _tramiteRepository;


        private readonly IAutorizacionService _autorizacionService;

        private readonly ActualizacionEstadoExpedienteService _actualizacionEstadoService;
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;

        public EliminarTramiteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService, ActualizacionEstadoExpedienteService actualizacionEstadoService, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _tramiteRepository = tramiteRepository;


            _autorizacionService = autorizacionService;

            _actualizacionEstadoService = actualizacionEstadoService;
            _unidadDeTrabajo = unidadDeTrabajo;
            
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
                throw new EntidadNoEncontradaException("No existe un trámite con ese Id.");
            }
            _tramiteRepository.Eliminar(tramite.Id);
            _actualizacionEstadoService.Actualizar(tramite.ExpedienteId, request.UsuarioId);
            _unidadDeTrabajo.Guardar();
            return new EliminarTramiteResponse
            {
                Eliminado = true
            };
        }
    }
}
