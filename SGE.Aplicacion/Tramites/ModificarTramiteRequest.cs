namespace SGE.Aplicacion.Tramites
{
    public record ModificarTramiteRequest
    {
        public Guid TramiteId { get; set; }

        public required string Contenido { get; set; }

        public Guid UsuarioId { get; set; }
    }
}