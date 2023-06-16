using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using DataAccess.Repositories.Operations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.DAL;

public class WorkModel : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public WorkModel(ApplicationDbContext context, UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _context = context;
        UserAuthentication = new UserAuthenticationRepository(userManager, roleManager, configuration);
        UrlsRepository = new UrlsRepository(_context);
        AboutRepository = new AboutRepository(_context);
    }

    public IAboutRepository AboutRepository { get; }
    public IUrlsRepository UrlsRepository { get; }
    public IUserAuthenticationRepository UserAuthentication { get; }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
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
