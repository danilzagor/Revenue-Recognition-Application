using Microsoft.IdentityModel.Tokens;
using Moq;
using RevenueRecognition.Exceptions;
using RevenueRecognition.Models;
using RevenueRecognition.Repositories;
using RevenueRecognition.Services;

namespace RevenueRecognitionTest.UnitTests.Services;

public class UserServiceTests
{

    [Fact]
    public async void AddUserAsync_Successful()
    {
        var user = new User
        {
            Login = "123",
            Password = "123",
            RefreshToken = "asd",
            RefreshTokenExp = DateTime.Today,
            Roles = ["asd"],
            Salt = "asd"
        };
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        
        mockRepository.Setup(r => r.SaveAsync())
            .Returns(Task.CompletedTask);

        var userService = new UserService(mockRepository.Object);

        await userService.AddUserAsync(user);
        
        mockRepository.Verify(r => r.AddAsync(It.IsAny<User>()));
        mockRepository.Verify(r => r.SaveAsync());
    }

    [Fact]
    public async void UpdateRefreshTokenAsync_Successful()
    {
        var user = new User
        {
            Login = "123",
            Password = "123",
            RefreshToken = "asd",
            RefreshTokenExp = DateTime.Today,
            Roles = ["asd"],
            Salt = "asd"
        };

        var refToken = "asdasd";
        var days = 5;
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(user);
        
        mockRepository.Setup(r => r.SaveAsync())
            .Returns(Task.CompletedTask);

        var userService = new UserService(mockRepository.Object);

        await userService.UpdateRefreshTokenAsync(user, refToken, days);
        
        Assert.Equal(refToken, user.RefreshToken);
        Assert.Equal(days, double.Round((user.RefreshTokenExp-DateTime.UtcNow).TotalDays));
        mockRepository.Verify(r => r.SaveAsync());
    }

    [Fact]
    public async void DoesUserExistAsync_DoesExist()
    {
        var userName = "123";
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.DoesUserExistAsync(It.IsAny<string>()))
            .ReturnsAsync(true);
        
        var userService = new UserService(mockRepository.Object);

        var res = await userService.DoesUserExistAsync(userName);
        
        Assert.True(res);
    }
    
    [Fact]
    public async void DoesUserExistAsync_DoesNotExist()
    {
        var userName = "123";
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.DoesUserExistAsync(It.IsAny<string>()))
            .ReturnsAsync(false);
        
        var userService = new UserService(mockRepository.Object);

        var res = await userService.DoesUserExistAsync(userName);
        
        Assert.False(res);
    }

    [Fact]
    public async void GetUserAsync_Successful()
    {
        var userName = "123";
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.GetUserByUserNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                Login = "123"
            });
        
        var userService = new UserService(mockRepository.Object);

        var res = await userService.GetUserAsync(userName);
        
        Assert.Equal(userName, res.Login);
    }
    
    [Fact]
    public async void GetUserAsync_ThrowsNotFoundException()
    {
        var userName = "123";
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.GetUserByUserNameAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        
        var userService = new UserService(mockRepository.Object);
        
        await Assert.ThrowsAsync<NotFoundException>(()=> userService.GetUserAsync(userName));
    }
    
    [Fact]
    public async void GetUserByRefreshTokenAsync_Successful()
    {
        var refToken = "123";
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.GetUserByRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync(new User
            {
                RefreshToken = "123"
            });
        
        var userService = new UserService(mockRepository.Object);

        var res = await userService.GetUserByRefreshTokenAsync(refToken);
        
        Assert.Equal(refToken, res.RefreshToken);
    }

    [Fact]
    public async void GetUserByRefreshTokenAsync_ThrowsSecurityTokenException()
    {
        var refToken = "123";
        
        var mockRepository = new Mock<IUserRepository>();

        mockRepository.Setup(r => r.GetUserByRefreshTokenAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);
        
        var userService = new UserService(mockRepository.Object);
        
        await Assert.ThrowsAsync<SecurityTokenException>(()=> userService.GetUserByRefreshTokenAsync(refToken));
    }

}