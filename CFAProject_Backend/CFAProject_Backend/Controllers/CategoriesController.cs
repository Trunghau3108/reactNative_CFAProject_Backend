using CFAProject_Backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Xml;
using System.Text.Json;
namespace CFAProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CFAProjectContext _context;
        public CategoriesController(CFAProjectContext ctx)
        {
            _context = ctx;
        }

        //GetListCategory
        [HttpPost("GetListCategory")]
        public IActionResult GetAll()
        {
            return Ok(_context.Categories.ToList());
        }



        //GetCategory By ID
        [HttpPost("GetCategoryById")]
        public IActionResult GetCategoryById([FromBody] JsonElement requestBody)
        {
            if (requestBody.ValueKind != JsonValueKind.Object)
            {
                return BadRequest("Invalid request body");
            }

            if (!requestBody.TryGetProperty("id", out JsonElement idElement) || !idElement.TryGetInt32(out int id))
            {
                return BadRequest("Invalid category ID");
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound(); // Return HTTP 404 Not Found if the category is not found
            }

            return Ok(category); // Return the found category as JSON
        }

        //Update Category
        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory([FromBody] Category model)
        {
            int id = model.Id;

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound(); // Return HTTP 404 Not Found if the category is not found
            }

            // Update the category properties with the values from the model
            category.NameVn = model.NameVn;
            category.Name = model.Name;

            // Save the changes to the database
            _context.SaveChanges();

            return Ok(category); // Return the updated category as JSON
        }

        //DeleteCategory

        /*[HttpPost("DeleteCategory")]
        public IActionResult DeleteCategory([FromBody] DeleteCategoryModel model)
        {
            int categoryId = model.CategoryId;

            var category = _context.Categories.FirstOrDefault(c => c.Id == categoryId);

            if (category == null)
            {
                return NotFound(); // Return HTTP 404 Not Found if the category is not found
            }

            var productsToUpdate = _context.Products.Where(p => p.CategoryId == categoryId);

            foreach (var product in productsToUpdate)
            {
                product.CategoryId = null;
            }

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return Ok(); // Return HTTP 200 OK if the category and associated product category IDs are successfully updated
        }*/










    }
}
