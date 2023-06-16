using BusinessLogic.DAL;
using BusinessLogic.DTOs;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using Xunit;

namespace UrlShortener.Tests;

public class AuthControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AuthController _authController;

    public AuthControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _authController = new AuthController(_unitOfWorkMock.Object);
    }

    [Fact]
    public void Post_SignInWithInvalidCredentials()
    {
        var invalidUserLogin = new UserLoginDto { Email = "invalid", Password = "invalid" };
        var user = new User();
        _unitOfWorkMock.Setup(u => u.UserAuthentication.ValidateUser(invalidUserLogin, out user))
            .Returns(false);

        var result = _authController.SignIn(invalidUserLogin);

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
