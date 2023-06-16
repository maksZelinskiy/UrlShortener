using BusinessLogic.DAL;
using BusinessLogic.Services;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using Xunit;

namespace UrlShortener.Tests;

public class UrlControllerTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UrlController _urlController;

    public UrlControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _urlController = new UrlController(_unitOfWorkMock.Object, new UrlConverter());
    }

    [Fact]
    public async Task Get_ReturnsUrlById()
    {
        var url = new ShortUrl { FullUrl = "some", ShortenedUrl = "some", CreatedAt = DateTime.Now };
        _unitOfWorkMock.Setup(u => u.UrlsRepository.GetUrlAsync(0)).ReturnsAsync(url);

        var result = await _urlController.GetUrlById(0);

        var jsonResult = Assert.IsType<JsonResult>(result);

        Assert.Equal(url, jsonResult.Value);
    }

    [Fact]
    public async Task Post_CreateNewUrl_AlreadyExists()
    {
        const string longUrl = "https://www.google.com/";
        _unitOfWorkMock.Setup(u => u.UrlsRepository.CheckIfExists(longUrl)).ReturnsAsync(true);

        var result = await _urlController.CreateUrl(longUrl);

        var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
        var badRequestResult = (string?)badRequestObjectResult.Value;

        Assert.Equal("Url already exists", badRequestResult);
    }

    [Fact]
    public async Task Post_CreateNewUrl()
    {
        const string longUrl = "https://www.google.com/";
        var newUrl = new ShortUrl
        {
            Id = 0, CreatedAt = DateTime.Now, FullUrl = longUrl, CreatedBy = new User(),
            ShortenedUrl = "https://localhost:44424/url/a"
        };

        _unitOfWorkMock.Setup(u => u.UrlsRepository.CheckIfExists(longUrl)).ReturnsAsync(false);
        _unitOfWorkMock.Setup(u => u.UserAuthentication.GetUserByName(null)).ReturnsAsync(new User());
        _unitOfWorkMock.Setup(u => u.UrlsRepository.InsertUrlAsync(new ShortUrl()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

        _urlController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        _urlController.ControllerContext.HttpContext.Request.Scheme = "https";
        _urlController.ControllerContext.HttpContext.Request.Host = new HostString("localhost:44424");

        var result = await _urlController.CreateUrl(longUrl);

        var jsonResult = Assert.IsType<JsonResult>(result);

        Assert.Equal(newUrl.ShortenedUrl,
            jsonResult.Value!.GetType().GetProperty("ShortenedUrl")!.GetValue(jsonResult.Value, null));

        Assert.Equal(newUrl.FullUrl,
            jsonResult.Value!.GetType().GetProperty("FullUrl")!.GetValue(jsonResult.Value, null));
    }
}
