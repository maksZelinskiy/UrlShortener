using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IAboutRepository
{
    Task<AboutModel> GetAboutAsync();
    Task<AboutModel> UpdateAboutAsync(AboutModel about);
}
