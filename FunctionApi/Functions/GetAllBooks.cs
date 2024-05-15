using FunctionApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FunctionApi.Functions
{
    public class GetAllBooks
    {
        private readonly ILogger<GetAllBooks> _logger;
        private readonly BookDbContext _context;

        public GetAllBooks(ILogger<GetAllBooks> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }


        //getting all books
        [Function("GetAllBooks")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "books")] HttpRequest req)
        {
            _logger.LogInformation("Get all books.");
            var books = await _context.Books.ToListAsync();

            return new OkObjectResult(books);
        }
    }
}
