using BusinessLogic.DAL;
using BusinessLogic.Interfaces;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrlController : Controller
{
    public UrlController(IUnitOfWork workModel, IUrlService urlService)
    {
        UrlService = urlService;
        WorkModel = workModel;
    }

    private IUrlService UrlService { get; }
    private IUnitOfWork WorkModel { get; }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var urls = await WorkModel.UrlsRepository.GetUrlsAsync();
        var user = await WorkModel.UserAuthentication.GetUserByName(User?.Identity?.Name);
        var auth = user is not null;

        return Json(new
        {
            urls = urls.Select(url => new
                { url, canEdit = auth && (url.UserId == user!.Id || User!.IsInRole("Admin")) }),
            canAdd = auth
        });
    }

    [HttpGet("{id:int}")]
    [Authorize]
    public async Task<IActionResult> GetUrlById(int id)
    {
        var url = await WorkModel.UrlsRepository.GetUrlAsync(id);
        return Json(url);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUrl([FromBody] string longUrl)
    {
        if (await WorkModel.UrlsRepository.CheckIfExists(longUrl))
            return BadRequest("Url already exists");

        var user = await WorkModel.UserAuthentication.GetUserByName(User?.Identity?.Name);
        var url = new ShortUrl
        {
            CreatedAt = DateTime.Now,
            FullUrl = longUrl,
            CreatedBy = user
        };

        await WorkModel.UrlsRepository.InsertUrlAsync(url);
        await WorkModel.SaveAsync();

        var shortUrl = UrlService.EncodeUrl(url.Id);
        url.ShortenedUrl = $"{Request.Scheme}://{Request.Host.Value}/url/{shortUrl}";

        await WorkModel.SaveAsync();

        return Json(url);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateUrl(int id, [FromBody] string longUrl)
    {
        var url = await WorkModel.UrlsRepository.GetUrlAsync(id);
        if (url is null) return NotFound();

        url.FullUrl = longUrl;
        url.LastUpdatedAt = DateTime.Now;

        await WorkModel.UrlsRepository.UpdateUrlAsync(url);
        await WorkModel.SaveAsync();

        return Json(url);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteUrl(int id)
    {
        var url = await WorkModel.UrlsRepository.GetUrlAsync(id);
        if (url is null)
            return NotFound();

        var user = (await WorkModel.UserAuthentication.GetUserByName(User.Identity?.Name))!;
        if (url.UserId != user.Id && !User.IsInRole("Admin"))
            return BadRequest("You are not allowed to delete this url");

        await WorkModel.UrlsRepository.DeleteUrlAsync(url);
        await WorkModel.SaveAsync();

        return Ok();
    }
}
