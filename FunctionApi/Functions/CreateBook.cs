using FunctionApi.Data;
using FunctionApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApi.Functions
{
    public class CreateBook
    {
        private readonly ILogger<CreateBook> _logger;

        private readonly BookDbContext _context;

        public CreateBook(ILogger<CreateBook> logger, BookDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        //input a title and author into the body to create a book
        [Function("CreateBook")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "books")] HttpRequest req)
        {
            _logger.LogInformation("Create a book.");
            string requestData = await new StreamReader(req.Body).ReadToEndAsync();
            var book = JsonConvert.DeserializeObject<Book>(requestData);

            _context.Books.Add(book);
            await _context.SaveChangesAsync();
            return new OkObjectResult(book);
        }
    }
}
