using SGE.Aplicacion.Autorizacion;
using SGE.Aplicacion.Expedientes;
using SGE.Aplicacion.Tramites.DTOs;
using SGE.Dominio.Expedientes;
using SGE.Dominio.Tramites;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SGE.Aplicacion.Tramites;

public class AgregarTramiteUseCase
{
    private readonly ITramiteRepository _tramiteRepository;

    private readonly IExpedienteRepository _expedienteRepository;

    private readonly IAutorizacionService _autorizacionService;

    private readonly ActualizacionEstadoExpedienteService _actualizacionEstadoService;

    public AgregarTramiteUseCase(
        ITramiteRepository tramiteRepository,
        IExpedienteRepository expedienteRepository,
        IAutorizacionService autorizacionService,
        ActualizacionEstadoExpedienteService actualizacionEstadoService)
    {
        _tramiteRepository = tramiteRepository;

        _expedienteRepository = expedienteRepository;

        _autorizacionService = autorizacionService;

        _actualizacionEstadoService = actualizacionEstadoService;
    }
    public AgregarTramiteResponse Ejecutar(AgregarTramiteRequest request)
    {
        if (!_autorizacionService.PoseeElPermiso(request.UsuarioId, Permiso.TramiteAlta))
        {
            throw new AutorizacionException("El usuario no posee permiso para agregar tramites.");
        }
        var expediente = _expedienteRepository.ObtenerPorId(request.ExpedienteId);
        if (expediente == null)
        {
            throw new Exception("No existe un expediente con ese id.");
        }
        var tramite = new Tramite(request.ExpedienteId, request.Etiqueta, new ContenidoTramite(request.Contenido), request.UsuarioId);
        _tramiteRepository.Agregar(tramite);
        var tramites = _tramiteRepository.ObtenerPorExpedienteId(request.ExpedienteId);
        _actualizacionEstadoService.Actualizar(request.ExpedienteId, request.UsuarioId);
        return new AgregarTramiteResponse
        {
            TramiteId = tramite.Id
        };
    }
}

