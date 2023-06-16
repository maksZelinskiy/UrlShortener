using DataAccess.Models;

namespace DataAccess.Repositories.Interfaces;

public interface IUrlsRepository : IDisposable
{
    Task<bool> CheckIfExists(string url);
    Task<IEnumerable<ShortUrl>> GetUrlsAsync();
    Task<ShortUrl?> GetUrlAsync(int id);
    Task InsertUrlAsync(ShortUrl url);
    Task UpdateUrlAsync(ShortUrl url);
    Task DeleteUrlAsync(ShortUrl url);
}
