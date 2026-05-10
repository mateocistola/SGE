using SGE.Dominio.Comun;

namespace SGE.Dominio.Tramites;

public record class ContenidoTramite
{
    public string Valor { get; }

    public ContenidoTramite(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DominioException("El contenido del trámite no puede estar vacío");
        }

        Valor = valor;
    }
}