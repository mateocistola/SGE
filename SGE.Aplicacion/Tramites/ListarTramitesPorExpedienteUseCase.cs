namespace SGE.Aplicacion.Tramites
{
    public class ListarTramitesPorExpedienteUseCase
    {
        private readonly ITramiteRepository _tramiteRepository;


        public ListarTramitesPorExpedienteUseCase(ITramiteRepository tramiteRepository)
        {
            _tramiteRepository = tramiteRepository;
         
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