using CFAProject_Backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Xml;

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
        public IActionResult GetAll()
        {
            return Ok(_context.Categories.ToList());
        }

        [HttpPost]
        /*public IActionResult GetCategoryById()
        {
            int id;
            if (!int.TryParse(Request.Body.ToString(), out id))
            {
                return BadRequest("Invalid ID"); // Trả về HTTP 400 Bad Request nếu ID không hợp lệ
            }

            var category = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (category == null)
            {
                return NotFound(); // Trả về HTTP 404 Not Found nếu không tìm thấy danh mục với ID tương ứng
            }

            return Ok(category); // Trả về danh mục tìm thấy dưới dạng JSON
        }*/
    }
}
