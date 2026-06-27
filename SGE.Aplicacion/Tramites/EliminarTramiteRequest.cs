namespace SGE.Aplicacion.Tramites
{
    public record EliminarTramiteRequest
    {
        public Guid TramiteId { get; set; }

        public Guid UsuarioId { get; set; }
    }
}
