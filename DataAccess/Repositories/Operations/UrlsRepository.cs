using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Operations;

public class UrlsRepository : IUrlsRepository
{
    private readonly ApplicationDbContext _context;

    public UrlsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckIfExists(string url)
    {
        return await _context.ShortUrls.AnyAsync(s => s.FullUrl == url);
    }

    public async Task<IEnumerable<ShortUrl>> GetUrlsAsync()
    {
        var urls = _context.ShortUrls.ToArrayAsync();

        return await urls;
    }

    public async Task<ShortUrl?> GetUrlAsync(int id)
    {
        var url = await _context.ShortUrls
            .Include(s => s.CreatedBy)
            .FirstOrDefaultAsync(s => s.Id == id);

        return url;
    }

    public async Task InsertUrlAsync(ShortUrl url)
    {
        await _context.ShortUrls.AddAsync(url);
    }

    public async Task UpdateUrlAsync(ShortUrl url)
    {
        await Task.Run(() => _context.ShortUrls.Update(url));
    }

    public async Task DeleteUrlAsync(ShortUrl url)
    {
        await Task.Run(() => _context.ShortUrls.Remove(url));
    }

    private bool _disposed;

    private void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
