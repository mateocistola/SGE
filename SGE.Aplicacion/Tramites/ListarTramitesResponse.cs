namespace SGE.Aplicacion.Tramites;

public record ListarTramitesResponse
{
    public required IEnumerable<TramiteResponseDto> Tramites { get; set; }
}