namespace SGE.Aplicacion.Tramites
{
    public record ListarTramitesRequest
    {
        public Guid ExpedienteId { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
