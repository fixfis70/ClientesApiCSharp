using System.Security.Claims;
using System.Text;
using ClientesApi.Data;
using ClientesApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace ClientesApi.controller;

[ApiController]
[Route("/")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("/register")]
    //REGISTRAR
    public async Task<IActionResult> Registrar([FromBody] RegistroRequest request)
    {
        if (await _context.Usuarios.AnyAsync(u=> u.Username == request.Username))
        {
            return Conflict(new { msg = "Usuario ya existe" });
        }

        if (request.Password.Length <6)
        {
            return Conflict(new { msg = "La contraseña debe de tener al menos 6 caracteres" });
        }
        var user = new Usuario
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Rol = "Usuario"
        };
        
        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();
        
        return Ok(new {msg = "Usuario registrado"});
    }

    [HttpPost("/login")]
    //Logear
    public async Task<ActionResult> Logear(LoginRequest request)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(u=>u.Username == request.Username);

        if (user == null ||
            !BCrypt.Net.BCrypt.Verify(request.Password,user.Password))
        {
            return Unauthorized(new {msg = "Usuario o contraseña no encontrado"});
        }

        var (token, expira) = GenerarToken(user);
        return Ok(new {token, expira});
    }

    private (string? token, DateTime? expira) GenerarToken(Usuario user)
    {
        var jwt = _configuration.GetSection("Jwt");
            
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
        var expira = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpireMinutes"]!));

        var descriptor = new SecurityTokenDescriptor
        {
            
            Subject = new ClaimsIdentity(new[]
            {
                new Claim( JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim( JwtRegisteredClaimNames.UniqueName, user.Username),
                new Claim( ClaimTypes.Role, user.Rol),
                new Claim( JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }),
            Issuer = jwt["Issuer"],
            Audience = jwt["Audience"],
            Expires = expira,
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = new JsonWebTokenHandler().CreateToken(descriptor);
        
        return (token,expira);
    }
}