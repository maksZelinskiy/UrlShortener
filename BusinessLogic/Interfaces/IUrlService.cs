namespace BusinessLogic.Interfaces;

public interface IUrlService
{
    string EncodeUrl(int id);
    int DecodeUrl(string shortUrl);
}
