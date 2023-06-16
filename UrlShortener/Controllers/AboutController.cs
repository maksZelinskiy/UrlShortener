using BusinessLogic.DAL;
using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UrlShortener.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : Controller
{
    public AboutController(IUnitOfWork workModel)
    {
        WorkModel = workModel;
    }

    private IUnitOfWork WorkModel { get; }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var about = await WorkModel.AboutRepository.GetAboutAsync();

        return Json(new { about, canEdit = User?.IsInRole("Admin") });
    }

    [HttpPut]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Put([FromBody] AboutModel about)
    {
        await WorkModel.AboutRepository.UpdateAboutAsync(about);
        await WorkModel.SaveAsync();

        return Ok(new { message = "About page updated successfully" });
    }
}
