using CFAProject_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

    }

}
