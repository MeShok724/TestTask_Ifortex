using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService(ApplicationDbContext applicationDbContext) : IAuthorService
    {
        private readonly ApplicationDbContext _context = applicationDbContext;
        public Task<Author> GetAuthor()
        {
            var dbResp = _context.Authors
                .AsNoTracking()
                .Select(author => new
                {
                    Author = author,
                    LongestBookTitleLength = author.Books.Max(book => book.Title.Length)
                })
                .OrderByDescending(a => a.LongestBookTitleLength)
                .ThenBy(a => a.Author.Id)
                .FirstOrDefault()?.Author;

            return Task.FromResult(dbResp);
        }

        public Task<List<Author>> GetAuthors()
        {
            var dbResp = _context.Authors
                .AsNoTracking()
                .Where(author => author.Books
                    .Count(book => book.PublishDate >= new DateTime(2016, 1, 1)) % 2 == 0)
                .ToList();
            return Task.FromResult(dbResp);
        }
    }
}
