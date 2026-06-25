using System.Security.Cryptography;
using System.Text;
using SGE.Aplicacion.Usuarios;

namespace SGE.Infraestructura.Seguridad;

public class HashService : IHashService
{
    public string GenerarHash(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));

        return Convert.ToHexString(bytes);
    }
}