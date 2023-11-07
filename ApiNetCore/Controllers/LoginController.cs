using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ApiNetCore.Models;

namespace ApiNetCore.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LoginController : ControllerBase
  {
    private readonly IConfiguration _configuration;

    public LoginController(IConfiguration configuration)
    {
      _configuration = configuration;
    }


    [HttpPost()]
    public async Task<IActionResult> Login([FromBody] Login model)
    {
      //TODO -> VALIDAR CREDENCIALES CONTRA EL REPOSITORIO
      var clave = _configuration["Jwt:Key"];
      if (model.UserName == "usuario" && model.Contraseña == "contraseña")
      {
        // Clave secreta para firmar el token
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave));

        // Crear las afirmaciones (claims) para el usuario autenticado
        var claims = new[]
        {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        // Crear el token JWT
        var token = new JwtSecurityToken(
            issuer: "tu_issuer",
            audience: "tu_audience",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        // Serializar el token a una cadena
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { token = tokenString });
      }

      return Unauthorized();
    }
  }
}
