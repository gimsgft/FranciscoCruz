using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Questao5.Infrastructure.Services.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class LoginController : ControllerBase
{
    private readonly ILogger<LoginController> _logger;
    private readonly IConfiguration _configuration;

    public LoginController(
        ILogger<LoginController> logger,
        IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [HttpPost, Route("login")]
    public IActionResult Login(LoginDTO loginDTO)
    {
        _logger.LogInformation("Requisição método: {0}", nameof(Login));

        try
        {
            if (string.IsNullOrEmpty(loginDTO.Usuario) || string.IsNullOrEmpty(loginDTO.Senha))
                return BadRequest("Usuário e/ou senha não informada");

            if (loginDTO.Usuario.ToLower().Equals("gft") && loginDTO.Senha.Equals("teste123"))
            {
                var chaveSecreta = _configuration["JWT:ChaveSecreta"];
                var expiracaoHoras = int.Parse(_configuration["JWT:ExpiracaoHoras"]);
                var dataExpiracao = DateTime.UtcNow.AddHours(expiracaoHoras);

                var key = Encoding.UTF8.GetBytes(chaveSecreta);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new List<Claim>()),
                    Expires = dataExpiracao,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                
                var token = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

                _logger.LogInformation("Sucesso Resposta método: {0}", nameof(Login));
                return Ok(new JwtSecurityTokenHandler().WriteToken(token));
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation("Erro método: {0} | Exception.Message {1}", nameof(Login), ex.Message);
            return BadRequest("Falha ao gerar token");
        }

        return Unauthorized();
    }
}

public class LoginDTO
{
    public string Usuario { get; set; }
    public string Senha { get; set; }
}