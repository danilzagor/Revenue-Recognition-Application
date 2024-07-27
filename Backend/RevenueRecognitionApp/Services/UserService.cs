using Microsoft.IdentityModel.Tokens;
using RevenueRecognition.Controllers;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;

namespace RevenueRecognition.Services;

public interface IUserService
{
    public Task AddUserAsync(User user);
    public Task UpdateRefreshTokenAsync(User user, string refToken, int days);
    public Task<User> GetUserAsync(string userName);
    public Task<bool> DoesUserExistAsync(string userName);
    public Task<User> GetUserByRefreshTokenAsync(string refreshToken);
    public void SetTokensInsideCookie(string accessToken, string refreshToken, HttpContext context);
}
public class UserService(IUserRepository userRepository):IUserService
{
    public async Task AddUserAsync(User user)
    {
        await userRepository.AddAsync(user);
        await userRepository.SaveAsync();
    }

    public async Task UpdateRefreshTokenAsync(User user, string refToken, int days)
    {
        var userInDb = await userRepository.GetByIdAsync(user.Id);

        userInDb.RefreshToken = refToken;
        userInDb.RefreshTokenExp = DateTime.UtcNow.AddDays(days);

        await userRepository.SaveAsync();
    }

    public async Task<User> GetUserAsync(string userName)
    {
        var res = await userRepository.GetUserByUserNameAsync(userName);
        if (res is null)
        {
            throw new NotFoundException($"User with username:{userName} doesn't exist");
        }
        return res;
    }
    public async Task<bool> DoesUserExistAsync(string userName)
    {
        var res = await userRepository.DoesUserExistAsync(userName);
        return res;
    }

    public async Task<User> GetUserByRefreshTokenAsync(string refreshToken)
    {
        var userInDb = await userRepository.GetUserByRefreshTokenAsync(refreshToken);
        
        if (userInDb is null)
        {
            throw new SecurityTokenException($"Invalid refresh token");
        }
        
        return userInDb;
        
    }

    public void SetTokensInsideCookie(string accessToken, string refreshToken, HttpContext context)
    {
        context.Response.Cookies.Append("accessToken", accessToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(15),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
        
        context.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(3),
            HttpOnly = true,
            IsEssential = true,
            Secure = true,
            SameSite = SameSiteMode.None
        });
    }
}