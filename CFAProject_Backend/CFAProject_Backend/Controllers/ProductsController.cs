using CFAProject_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public class ProductSearchModel
        {
            public string SearchQuery { get; set; }
            // Các thuộc tính tìm kiếm khác nếu cần
        }

        [HttpPost("GetListProduct")]
        public IActionResult GetListProductWithAutomotive()
        {
            var productsWithAutomotives = _context.Products
               .Include(p => p.Automotives).Include(z => z.Category) // Include the 'Automotives' navigation property
               .ToList();

            // Project the results to the desired format
            var result = productsWithAutomotives.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Image,
                p.UnitPrice,
                p.SupplierId,
                p.Views,


                Category = new
                {
                    p.Category.TypeCar,
                    // Include other category properties you want in the result
                },

                // Include other product properties you want in the result

                Automotives = p.Automotives.Select(a => new
                {
                    a.Id,
                    a.Fuel,
                    a.Ac,
                    a.Gps,
                    a.Usb,
                    a.Seats,
                    a.Engine,
                    a.Bluetooth,
                    a.Capacity,
                    a.Driver,

                    // Include other automotive properties you want in the result
                }).ToList()
            }).ToList();

            return Ok(result);
        }

        //Search Product
        [HttpPost("SearchProduct")]
        public IActionResult SearchProductWithAutomotive([FromBody] ProductSearchModel searchModel)
        {
            var query = _context.Products
              .Include(p => p.Automotives)
              .Include(p => p.Category)
              .AsQueryable();

            // Thực hiện tìm kiếm nếu có thông tin tìm kiếm từ người dùng
            if (searchModel != null && !string.IsNullOrEmpty(searchModel.SearchQuery))
            {
                query = query.Where(p =>
                    p.Id.Equals(searchModel.SearchQuery) ||
                    p.Name.Contains(searchModel.SearchQuery) ||
                    p.Description.Contains(searchModel.SearchQuery) ||
                    p.Category.TypeCar.Contains(searchModel.SearchQuery)
                // Thêm các tiêu chí tìm kiếm khác tại đây
                // Ví dụ: p.Automotives.Any(a => a.Fuel.Contains(searchModel.SearchQuery))
                );
            }
            else
            {
                return BadRequest("error");
            }

            var result = query.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Image,
                p.UnitPrice,
                p.SupplierId,
                p.Views,
                p.Discount,
                p.ProductDate,
                p.UnitBrief,
                p.Quantity,

                Category = new
                {
                    p.Category.TypeCar,
                    // Include other category properties you want in the result
                },
                Automotives = p.Automotives.Select(a => new
                {
                    a.Id,
                    a.Fuel,
                    a.Ac,
                    a.Gps,
                    a.Usb,
                    a.Seats,
                    a.Engine,
                    a.Bluetooth,
                    a.Capacity,
                    a.Driver,
                    // Include other automotive properties you want in the result
                }).ToList()
            }).ToList();

            return Ok(result);
        }


        //GetCategory By ID
        [HttpPost("GetProductById")]
        public IActionResult GetProductById([FromBody] JsonElement requestBody)
        {
            try
            {
                if (requestBody.ValueKind != JsonValueKind.Object)
                {
                    return BadRequest("Invalid request body");
                }

                if (!requestBody.TryGetProperty("id",
                    out JsonElement idElement) || !idElement.TryGetInt32(out int id))
                {
                    return BadRequest("Invalid product ID");
                }

                var product = _context.Products.Include(c => c.Automotives).FirstOrDefault(c => c.Id == id);

                if (product == null)
                {
                    return NotFound();
                }
              


                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
