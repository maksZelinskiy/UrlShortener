using BusinessLogic.Interfaces;

namespace BusinessLogic.Services;

public class UrlConverter : IUrlService
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly int Base = Alphabet.Length;

    public string EncodeUrl(int id)
    {
        if (id == 0) return Alphabet[0].ToString();

        var s = string.Empty;

        while (id > 0)
        {
            s += Alphabet[id % Base];
            id /= Base;
        }

        return string.Join(string.Empty, s.Reverse());
    }

    public int DecodeUrl(string shortUrl)
    {
        return shortUrl.Aggregate(0, (current, c) => current * Base + Alphabet.IndexOf(c));
    }
}
