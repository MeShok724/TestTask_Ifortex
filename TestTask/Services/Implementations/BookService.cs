using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService(ApplicationDbContext applicationDbContext) : IBookService
    {
        private readonly ApplicationDbContext _context = applicationDbContext;
        public Task<Book> GetBook()
        {
            var dbResp = _context.Books
                .AsNoTracking()
                .OrderByDescending(book => book.Price * book.QuantityPublished)
                .FirstOrDefault();
            return Task.FromResult(dbResp);
        }

        public Task<List<Book>> GetBooks()
        {
            var carolusRexReleaseDate = new DateTime(2012, 5, 25);
            var dbResp = _context.Books
                .AsNoTracking()
                .Where(book => book.PublishDate >  carolusRexReleaseDate 
                    && book.Title.Contains("Red"))
                .ToList();
            return Task.FromResult(dbResp);
        }
    }
}
