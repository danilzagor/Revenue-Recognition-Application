using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.RequestModels.AuthRequestModels;
using RevenueRecognition.ResponseModels.AuthResponseModel;
using RevenueRecognition.Services;

namespace RevenueRecognition.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IConfiguration config, IUserService userService) : ControllerBase
{
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRegisterRequestModel model)
    {
        try
        {
            var user = await userService.GetUserAsync(model.Login);
            var passwordHashFromDb = user.Password;
            var curHashedPassword = SecurityHelper.GetHashedPasswordWithSalt(model.Password, user.Salt);
            
            if (passwordHashFromDb != curHashedPassword)
                return Unauthorized("Wrong password");

            var userClaim = new Claim [user.Roles.Count];
            {
                var i = 0;
                foreach (var role in user.Roles)
                {
                    userClaim[i++] = new Claim(ClaimTypes.Role, role);
                }
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescription = new JwtSecurityToken(
            
                issuer: config["JWT:Issuer"],
                audience:  config["JWT:Audience"],
                claims: userClaim,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            
            var refTokenDescription = new JwtSecurityToken(
            
                issuer: config["JWT:RefIssuer"],
                audience:  config["JWT:RefAudience"],
                claims: userClaim,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:RefKey"]!)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            
            var stringToken = tokenHandler.WriteToken(tokenDescription);
            var stringRefToken =  tokenHandler.WriteToken(refTokenDescription);
            await userService.UpdateRefreshTokenAsync(user, stringRefToken, 1);
            
            userService.SetTokensInsideCookie(stringToken, stringRefToken, HttpContext);

            return Ok(userClaim);
            
        }
        catch (NotFoundException e)
        {
            return Unauthorized(e.Message);
        }
        
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken()
    {
        
        try
        {
            
            var refreshToken = Request.Cookies["refreshToken"];
 
            if (refreshToken is null)
            {
                return Unauthorized("No refresh token provided");
            }
            
            var user = await userService.GetUserByRefreshTokenAsync(refreshToken);
            
            var userClaim = new Claim [user.Roles.Count];
            {
                var i = 0;
                foreach (var role in user.Roles)
                {
                    userClaim[i++] = new Claim(ClaimTypes.Role, role);
                }
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescription = new JwtSecurityToken(
            
                issuer: config["JWT:Issuer"],
                audience:  config["JWT:Audience"],
                claims: userClaim,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            
            var refTokenDescription = new JwtSecurityToken(
            
                issuer: config["JWT:RefIssuer"],
                audience:  config["JWT:RefAudience"],
                claims: userClaim,
                expires: DateTime.UtcNow.AddDays(3),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:RefKey"]!)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            
            var stringToken = tokenHandler.WriteToken(tokenDescription);
            var stringRefToken =  tokenHandler.WriteToken(refTokenDescription);
            await userService.UpdateRefreshTokenAsync(user, stringRefToken, 3);
            
            userService.SetTokensInsideCookie(stringToken, stringRefToken, HttpContext);    
            
            return Ok();
        }
        catch (SecurityTokenException e)
        {
            return Unauthorized(e.Message);
        }
        
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginRegisterRequestModel requestModel)
    {
        if (await userService.DoesUserExistAsync(requestModel.Login))
        {
            return Conflict(new { message = "User with this login already exists" });
        }
        
        var hashedPasswordAndSalt = SecurityHelper.GetHashedPasswordAndSalt(requestModel.Password);
        var user = new User
        {
            Login = requestModel.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            Roles = ["user"],
            RefreshToken = "",
            RefreshTokenExp = DateTime.UtcNow.AddDays(1)
        };
        await userService.AddUserAsync(user);
        
        var userClaim = new Claim [user.Roles.Count];
        {
            var i = 0;
            foreach (var role in user.Roles)
            {
                userClaim[i++] = new Claim(ClaimTypes.Role, role);
            }
        }
        
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var refTokenDescription = new JwtSecurityToken(
            
            issuer: config["JWT:RefIssuer"],
            audience:  config["JWT:RefAudience"],
            claims: userClaim,
            expires: DateTime.UtcNow.AddDays(3),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:RefKey"]!)),
                SecurityAlgorithms.HmacSha256
            )
        );
        
        var stringRefToken =  tokenHandler.WriteToken(refTokenDescription);
        await userService.UpdateRefreshTokenAsync(user, stringRefToken, 3);
        return Created();
    }
    
    

    
}