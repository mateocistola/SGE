using System;
using System.Collections.Generic;
using System.Text;

using SGE.Aplicacion.Expedientes;

namespace SGE.Aplicacion.Tramites
{
    public class ActualizacionEstadoExpedienteService
    {
        private readonly IExpedienteRepository _expedienteRepository;

        private readonly ITramiteRepository _tramiteRepository;

        public ActualizacionEstadoExpedienteService(
            IExpedienteRepository expedienteRepository,
            ITramiteRepository tramiteRepository)
        {
            _expedienteRepository = expedienteRepository;

            _tramiteRepository = tramiteRepository;
        }
        public void Actualizar(Guid expedienteId, Guid usuarioId)
        {
            var expediente = _expedienteRepository.ObtenerPorId(expedienteId);
            if (expediente is null)
            {
                throw new Exception($"No se encontró el expediente con ID: {expedienteId}");
            }
            var tramites = _tramiteRepository.ObtenerPorExpedienteId(expedienteId);
            var ultimoTramite = tramites.OrderByDescending(t => t.FechaCreacion).FirstOrDefault();
            bool cambioEstado = expediente.ActualizarEstado(ultimoTramite?.Etiqueta, usuarioId);
            if (cambioEstado)
            {
                _expedienteRepository.Modificar(expediente);
            }
        }
    }
}