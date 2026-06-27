using SGE.Dominio.Usuarios;
using SGE.Dominio.Tramites;
using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Comun;

namespace SGE.Aplicacion.Tramites
{
    public class ModificarTramiteUseCase
    {
        private readonly ITramiteRepository _tramiteRepository;

        private readonly IAutorizacionService _autorizacionService;
   
        private readonly IUnidadDeTrabajo _unidadDeTrabajo;
        public ModificarTramiteUseCase(ITramiteRepository tramiteRepository, IAutorizacionService autorizacionService, IUnidadDeTrabajo unidadDeTrabajo)
        {
            _tramiteRepository = tramiteRepository;

            _autorizacionService = autorizacionService;

            _unidadDeTrabajo = unidadDeTrabajo;
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
                throw new EntidadNoEncontradaException("No existe un trámite con ese Id.");
            }
            tramite.ModificarContenido(new ContenidoTramite(request.Contenido), request.UsuarioId);
            _tramiteRepository.Modificar(tramite);
            _unidadDeTrabajo.Guardar();
            return new ModificarTramiteResponse
            {
                Modificado = true
            };
        }
    }
}