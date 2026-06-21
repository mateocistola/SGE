using SGE.Dominio.Comun;

namespace SGE.Dominio.Expedientes;

public record class Caratula
{
    public string Valor { get; private set; } = string.Empty;

    private Caratula()
    {
    }

    public Caratula(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            throw new DominioException("La carátula no puede estar vacía");
        }

        Valor = valor;
    }
}