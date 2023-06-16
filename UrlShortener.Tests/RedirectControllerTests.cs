using BusinessLogic.DAL;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using UrlShortener.Controllers;
using Xunit;

namespace UrlShortener.Tests;

public class RedirectControllerTests
{
    private readonly Mock<IUrlService> _urlServiceMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly RedirectController _redirectController;

    public RedirectControllerTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _urlServiceMock = new Mock<IUrlService>();
        _redirectController = new RedirectController(_unitOfWorkMock.Object, _urlServiceMock.Object);
    }

    [Fact]
    public async Task Get_ReturnFullUrlByShort()
    {
        const string url = "https://www.google.com/";

        _urlServiceMock.Setup(u => u.DecodeUrl("a")).Returns(0);
        _unitOfWorkMock.Setup(u => u.UrlsRepository.GetUrlAsync(0)).ReturnsAsync(new ShortUrl { FullUrl = url });

        var result = await _redirectController.GetUrlByShortUrl("a");

        var redirectResult = Assert.IsType<RedirectResult>(result);

        Assert.Equal(url, redirectResult.Url);
    }
}
