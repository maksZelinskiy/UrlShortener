using BusinessLogic.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace BusinessLogic.DAL;

public interface IUnitOfWork : IDisposable
{
    IAboutRepository AboutRepository { get; }
    IUrlsRepository UrlsRepository { get; }
    IUserAuthenticationRepository UserAuthentication { get; }
    Task SaveAsync();
}
