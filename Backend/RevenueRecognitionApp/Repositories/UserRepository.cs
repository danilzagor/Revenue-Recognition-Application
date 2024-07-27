using Microsoft.EntityFrameworkCore;
using RevenueRecognition.Contexts;
using RevenueRecognition.Models;

namespace RevenueRecognition.Repositories;

public interface IUserRepository : IGenericRepository<User>
{
    public Task<bool> DoesUserExistAsync(string userName);
    public Task<User?> GetUserByUserNameAsync(string userName); 
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);
}

public class UserRepository(DatabaseContext context) : GenericRepository<User>(context), IUserRepository
{
    public async Task<bool> DoesUserExistAsync(string userName)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.Login == userName)!=null;
    }
    

    public async Task<User?> GetUserByUserNameAsync(string userName)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.Login == userName);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.RefreshToken == refreshToken);
    }

    
}