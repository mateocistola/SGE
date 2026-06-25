using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SGE.Aplicacion.Usuarios;

namespace SGE.Infraestructura.Seguridad;

public class JwtTokenService : ITokenService
{
    private const string Clave =
        "clave-super-secreta-sge-tp2-2026-minimo-32-caracteres";

    public string GenerarToken(Guid usuarioId)
    {
        var clave = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Clave));

        var credenciales = new SigningCredentials(
            clave,
            SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}