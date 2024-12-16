using Application.Services.AppServices;
using Application.Users.DTOs;
using Domain.Exceptions;
using Domain.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TT_API.Controllers;

/// <summary>
/// Contrôleur pour la gestion de l'authentification.
/// </summary>
public class AuthController(IUserService userService, SignInManager<User> signInManager, IConfiguration configuration) : ControllerBase
{

    /// <summary>
    /// Enregistre un nouvel utilisateur.
    /// </summary>
    /// <param name="registerDto">Les informations de l'utilisateur à enregistrer.</param>
    /// <returns>Un message indiquant que l'utilisateur a été enregistré avec succès.</returns>
    /// <response code="200">Retourne un message de succès.</response>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var user = new User { UserName = registerDto.UserName, Email = registerDto.Email };
        var result = await userService.AddUser(registerDto.UserName, registerDto.Email, registerDto.Password, registerDto.Role);

        return Ok(new { Message = "User registered successfully" });
    }

    /// <summary>
    /// Authentifie un utilisateur et génère un token JWT.
    /// </summary>
    /// <param name="loginDto">Les informations de connexion de l'utilisateur.</param>
    /// <returns>Un token JWT si l'authentification est réussie.</returns>
    /// <response code="200">Retourne le token JWT.</response>
    /// <response code="401">Retourne une erreur si l'authentification échoue.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        var user = await userService.GetUserByEmail(loginDto.Email) ?? throw new NotFoundException("user", loginDto.Email);

        var result = await signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);

        if (!result.Succeeded)
        {
            return Unauthorized();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return Ok(new { Token = tokenString });
    }
}
