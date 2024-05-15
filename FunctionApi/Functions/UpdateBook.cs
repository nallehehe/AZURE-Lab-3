using FunctionApi.Data;
using FunctionApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApi.Functions
{
    public class UpdateBook
    {
        private readonly ILogger<UpdateBook> _logger;
        private readonly BookDbContext _context;

        public UpdateBook(ILogger<UpdateBook> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        //overwrites an existing book already with a new title and author by id
        [Function("UpdateBook")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "put", Route = "books/{id}")] HttpRequest req, int id)
        {
            var currentBook = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);

            _logger.LogInformation("Update a book.");

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var updatedBook = JsonConvert.DeserializeObject<Book>(requestData);

            currentBook.Title = updatedBook.Title;
            currentBook.Author = updatedBook.Author;

            await _context.SaveChangesAsync();

            return new OkObjectResult(currentBook);
        }
    }
}
