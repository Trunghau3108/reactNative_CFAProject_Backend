using CFAProject_Backend.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml;
namespace CFAProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CFAProjectContext _context;
        public CustomersController(CFAProjectContext ctx)
        {
            _context = ctx;
        }

        /*[HttpPost("GetListCustomers")]
        public IActionResult GetAll()
        {
            return Ok(_context.Customers.ToList());
        }*/


        public class LoginModel
        {
            public int Id { get; set; }
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginModel)
        {
            string email = loginModel.Email;
            string password = loginModel.Password;

            // Check the credentials against the database
            var customer = _context.Customers.FirstOrDefault(c => c.Email == email && c.Password == password);

            if (customer != null)
            {
                // Authentication successful
                return Ok("Login successful");
            }
            else
            {
                // Authentication failed
                return BadRequest("Invalid email or password");
            }
        }




       /* [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] LoginModel request)
        {
            // Kiểm tra thông tin đầy đủ
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Email and Password are required fields.");
            }

            // Tạo mới đối tượng Customer từ yêu cầu
            var customer = new Customer
            {
                Email = request.Email,
                Password = request.Password
            };

            // Thêm đối tượng Customer mới vào cơ sở dữ liệu
            _context.Customers.Add(customer);
            _context.SaveChanges();

            // Lấy giá trị ID ngay sau khi thêm bản ghi mới
            string sql = "SELECT SCOPE_IDENTITY() AS CustomerId";
            int newId = _context.Customers.FromSqlRaw(sql).Select(c => c.Id).FirstOrDefault();

            // Cập nhật giá trị ID cho đối tượng Customer
            customer.Id = newId;

            // Trả về đối tượng Customer đã được tạo mới
            return Ok(customer);
        }*/



    }
}
