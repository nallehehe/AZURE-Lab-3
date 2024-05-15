using FunctionApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FunctionApi.Functions
{
    public class DeleteBook
    {
        private readonly ILogger<DeleteBook> _logger;

        private readonly BookDbContext _context;

        public DeleteBook(ILogger<DeleteBook> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        //deletes a book by its id
        [Function("DeleteBook")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "books/{id}")] HttpRequest req, int id)
        {
            _logger.LogInformation("Delete a book");
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            if (book == null)
            {
                return new NotFoundResult();
            }

            _context.Books.Remove(book);

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
