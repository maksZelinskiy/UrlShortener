using DataAccess.Models;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Operations;

public class AboutRepository : IAboutRepository
{
    private readonly ApplicationDbContext _context;

    public AboutRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AboutModel> GetAboutAsync()
    {
        var about = await _context.AboutModels.FirstOrDefaultAsync();
        if (about is not null) return about;

        about = new AboutModel
        {
            Topic = "About Url Shortener Algorithm",
            Content =
                @"A URL shortener algorithm is a mechanism designed to convert long, unwieldy URLs into shorter and more manageable ones. This algorithm typically involves the following steps:
            1.When a new URL is added to the system,
            it is assigned a unique identifier (ID) by the MSSQL server using the Entity Framework.This ID serves as a
            reference to the original URL in the database.

            2.After obtaining the ID,
            the algorithm employs a bijective function to calculate the shortened URL.A bijective function is a function
            that establishes a one-to-one correspondence between two sets,
            ensuring that each element from the first set corresponds to a unique element in the second set,
            and vice versa.

            The specific details of the bijective function may vary depending on the implementation,
            but its purpose is to convert the ID into a shorter representation for the shortened URL.This function
            typically operates on a numerical input (the ID) and produces a shortened URL string as its output.

            For example, a simple bijective function could be based on base conversion.It might convert the ID from its numerical
            representation into a shorter string representation using a set of characters chosen specifically for the
            purpose of creating the shortened URLs.The function would take the ID as input,
            perform the necessary calculations or transformations, and generate the corresponding shortened URL.

            The advantage of using a bijective function is that it ensures the uniqueness of the shortened URLs.By
            establishing a one-to-one mapping between the IDs and the shortened URLs,
            the algorithm can accurately retrieve the original URL associated with a given shortened URL.

            Overall, the URL shortener algorithm combines the process of generating a unique ID for each URL and applying a
            bijective function to create a shorter,
            more concise URL representation.This allows for efficient storage and sharing of URLs while maintaining the
            ability to retrieve the original long URL when needed."
        };
        await _context.AboutModels.AddAsync(about);
        await _context.SaveChangesAsync();

        return about;
    }

    public async Task<AboutModel> UpdateAboutAsync(AboutModel about)
    {
        await Task.Run(() => _context.AboutModels.Update(about));
        return about;
    }
}
