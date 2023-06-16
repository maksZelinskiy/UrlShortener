using BusinessLogic.DAL;
using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers;

[ApiController]
[Route("url")]
public class RedirectController : Controller
{
    public RedirectController(IUnitOfWork workModel, IUrlService urlService)
    {
        WorkModel = workModel;
        UrlService = urlService;
    }

    private IUrlService UrlService { get; }
    private IUnitOfWork WorkModel { get; }

    [HttpGet("{shortUrl}")]
    public async Task<IActionResult> GetUrlByShortUrl(string shortUrl)
    {
        var id = UrlService.DecodeUrl(shortUrl);
        var url = await WorkModel.UrlsRepository.GetUrlAsync(id);
        return url is null ? NotFound() : Redirect(url.FullUrl);
    }
}
