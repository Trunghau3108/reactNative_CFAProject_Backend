using CFAProject_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace CFAProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OdersController : ControllerBase
    {
        private readonly CFAProjectContext _context;
        public OdersController(CFAProjectContext ctx)
        {
            _context = ctx;
        }

        [HttpPost("GetListOrder")]
        public IActionResult GetAll()
        {
            return Ok(_context.Orders.ToList());
        }
       

        [HttpPost("CreateOrder")]
        public IActionResult CreateNewOrder([FromBody] Order order)
        {
            Order newOrder = new Order
            {
                ProductId = order.ProductId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                ReturnDate = order.ReturnDate,
                Receipt = order.Receipt,
                Address = order.Address,
                Description = order.Description,
                Amount = order.Amount,
                PaymentMethod = order.PaymentMethod,
                StatusRent = true,
            };

            // Add the new Order to the Orders DbSet
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            // Lấy ID của bản ghi vừa insert
            int orderId = newOrder.Id;
            int productId = newOrder.ProductId;
            int customerId = newOrder.CustomerId;

            // Fetch the related Product and Customer data based on their IDs
            Product product = _context.Products.FirstOrDefault(p => p.Id == productId);
            Customer customer = _context.Customers.FirstOrDefault(c => c.Id == customerId);

            if (product == null || customer == null)
            {
                return BadRequest("Invalid ProductId or CustomerId.");
            }

            // Create a new OrderDetail item based on the fetched data
            OrderDetail orderDetail = new OrderDetail
            {
                OrderId = orderId,
                ProductName = product.Name,
                Image = product.Image,
                UnitPrice = product.UnitPrice,
                Quantity = product.Quantity,
                Discount = product.Discount,
                OrderDate = newOrder.OrderDate,
                ReturnDate = newOrder.ReturnDate,
                SupplierId = product.SupplierId,
                PaymentMethod = newOrder.PaymentMethod,
                CustomerName = customer.Fullname,
                CustomerEmail = customer.Email
            };

            // Insert the newly created OrderDetail into the OrderDetails table
            _context.OrderDetails.Add(orderDetail);
            _context.SaveChanges();

            return Ok("Tạo hóa đơn thành công");
        }

        [HttpPost("OrderHistory")]
        public IActionResult GetOrderHistory([FromBody] JsonElement customerIdElement)
        {
            if (customerIdElement.ValueKind != JsonValueKind.Object)
            {
                return BadRequest("Invalid request body");
            }

            if (!customerIdElement.TryGetProperty("id", out JsonElement idElement) || !idElement.TryGetInt32(out int id))
            {
                return BadRequest("Invalid customerId" + customerIdElement);
            }

            var orders = _context.Orders
            .Where(o => o.CustomerId == id && o.StatusRent == false)
            .ToList();

            if (orders.Count == 0)
            {
                return NotFound("Không tìm thấy đơn hàng cho khách hàng có mã: " + id);
            }

            var productIds = orders.Select(o => o.ProductId).ToList();

            var products = _context.Products.Include(p => p.Automotives)
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            var result = products.Select(p => new
            {
                p.Id,
                p.Name,
                p.Description,
                p.Image,
                p.UnitPrice,
                p.SupplierId,
                p.Views,
                p.Discount,


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


        [HttpPost("OrderPayment")]
        public IActionResult GetOrderPayment([FromBody] JsonElement customerIdElement)
        {
            if (customerIdElement.ValueKind != JsonValueKind.Object)
            {
                return BadRequest("Invalid request body");
            }

            if (!customerIdElement.TryGetProperty("id", out JsonElement idElement) || !idElement.TryGetInt32(out int id))
            {
                return BadRequest("Invalid customerId" + customerIdElement);
            }

            var orders = _context.Orders
            .Where(o => o.CustomerId == id && o.StatusRent == true)
            .ToList();

            if (orders.Count == 0)
            {
                return NotFound("Không tìm thấy đơn hàng cho khách hàng có mã: " + id);
            }

            var productIds = orders.Select(o => o.ProductId).ToList();

            var products = _context.Products.Include(p => p.Automotives)
                .Where(p => productIds.Contains(p.Id))
                .ToList();

            var result = orders.Select(o => new
            {
                o.Id,
                o.OrderDate,
                o.ReturnDate,
                o.PaymentMethod,
                o.Address,
                o.Description,

                Products = products.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.Image,
                    p.UnitPrice,
                    p.SupplierId,
                    p.Views,
                    p.Discount,


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
                    }).ToList()
                }).ToList();
   
                return Ok(result);
            }


        [HttpPost("UpdateStatusRent")]
        public IActionResult UpdateStatusRent([FromBody] JsonElement requestData)
        {
            if (requestData.ValueKind != JsonValueKind.Object)
            {
                return BadRequest("Invalid request body");
            }

            if (!requestData.TryGetProperty("customerId", out JsonElement customerIdElement) ||
                !customerIdElement.TryGetInt32(out int customerId))
            {
                return BadRequest("Invalid customerId");
            }

            if (!requestData.TryGetProperty("productId", out JsonElement productIdElement) ||
                !productIdElement.TryGetInt32(out int productId))
            {
                return BadRequest("Invalid productId");
            }

            var order = _context.Orders
                .FirstOrDefault(o => o.CustomerId == customerId && o.ProductId == productId);

            if (order == null)
            {
                return NotFound("Không tìm thấy đơn hàng cho khách hàng và sản phẩm có mã: " + customerId + ", " + productId);
            }

            // Update the StatusRent here
            order.StatusRent = !order.StatusRent; // Set to the opposite value (true -> false, false -> true)

            try
            {
                _context.SaveChanges(); // Save the changes to the database
            }
            catch (Exception ex)
            {
                // Handle exception if there's an error while saving changes
                return BadRequest("An error occurred while updating the StatusRent: " + ex.Message);
            }

            return Ok("StatusRent updated successfully for order with CustomerId: " + customerId + " and ProductId: " + productId);
        }


    }
}
