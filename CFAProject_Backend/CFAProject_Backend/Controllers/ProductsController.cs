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
            public string? SearchIdProduct { get; set; }
            public int? Seats { get; set; } // Chỉ số ghế, kiểu int?, cho phép truyền null nếu không cần tìm theo điều kiện này
            public string? TypeCar { get; set; }
            public string? Id { get; set; } // Loại xe, kiểu string
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
                p.Discount,


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
                    a.Location

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

            /*int searchQueryInt;
            bool isSearchQueryInt = int.TryParse(searchModel.SearchQuery, out searchQueryInt);*/
            // Thực hiện tìm kiếm nếu có thông tin tìm kiếm từ người dùng
            if (searchModel != null && !string.IsNullOrEmpty(searchModel.SearchIdProduct))
            {
                query = query.Where(p =>
                    p.Id.ToString().Contains(searchModel.SearchIdProduct) /*||
                    (isSearchQueryInt && p.Automotives.Any(a => a.Seats == searchQueryInt)) ||
                    p.Category.TypeCar.Contains(searchModel.SearchQuery)*/
                );
            }
            // Nếu cả đều có value
            else if(searchModel.Seats.HasValue && !string.IsNullOrEmpty(searchModel.TypeCar) && !string.IsNullOrEmpty(searchModel.Id))
            {
                query = query.Where(p =>
                    p.Automotives.Any(a => a.Seats == searchModel.Seats.Value) &&
                    p.Category.TypeCar.Contains(searchModel.TypeCar) &&
                    p.Supplier.Id.Contains(searchModel.Id)
                );
            }
            //Nếu Seats == null và ID == "" thì lọc TypeCar
            else if (!searchModel.Seats.HasValue && !string.IsNullOrEmpty(searchModel.TypeCar) && string.IsNullOrEmpty(searchModel.Id))
            {
                query = query.Where(p =>
                    p.Category.TypeCar.Contains(searchModel.TypeCar)
                );
            }
            //Nếu Seats có value nhưng Id == "" thì lọc theo Seats và typecar
            else if (searchModel.Seats.HasValue && !string.IsNullOrEmpty(searchModel.TypeCar) && string.IsNullOrEmpty(searchModel.Id))
            {
                query = query.Where(p =>
                    p.Category.TypeCar.Contains(searchModel.TypeCar) &&
                    p.Automotives.Any(a => a.Seats == searchModel.Seats.Value)
                );
            }
            //Trường hợp còn lại
            else if (!searchModel.Seats.HasValue && !string.IsNullOrEmpty(searchModel.TypeCar) && !string.IsNullOrEmpty(searchModel.Id))
            {
                query = query.Where(p =>
                    p.Category.TypeCar.Contains(searchModel.TypeCar) &&
                    p.Supplier.Id.Contains(searchModel.Id)
                );
            }
            else
            {
                return BadRequest("Lỗi rồi");
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
                   
                }).ToList()
            }).ToList();

            if (result.Any())
            {
                return Ok(result);
            }
            else
            {
                return NotFound("Không có dữ liệu trong hệ thống");
            }
        }
    }
}
