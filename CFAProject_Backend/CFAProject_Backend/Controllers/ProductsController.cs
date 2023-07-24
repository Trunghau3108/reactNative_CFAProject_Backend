using CFAProject_Backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CFAProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CFAProjectContext _context;
        public ProductsController(CFAProjectContext ctx)
        {
            _context = ctx;
        }

        //GetListCategory
        [HttpPost("GetListProduct")]
        public IActionResult GetAll()
        {
            return Ok(_context.Products.ToList());
        }



        //GetCategory By ID
        [HttpPost("GetProductById")]
        public IActionResult GetProductById([FromBody] JsonElement requestBody)
        {
            if (requestBody.ValueKind != JsonValueKind.Object)
            {
                return BadRequest("Invalid request body");
            }

            if (!requestBody.TryGetProperty("id",
                out JsonElement idElement) || !idElement.TryGetInt32(out int id))
            {
                return BadRequest("Invalid category ID");
            }

            var product = _context.Products.FirstOrDefault(c => c.Id == id);

            if (product == null)
            {
                return NotFound(); // Return HTTP 404 Not Found if the category is not found
            }

            return Ok(product); // Return the found category as JSON
        }
    }
}
