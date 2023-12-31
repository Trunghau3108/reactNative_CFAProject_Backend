﻿using CFAProject_Backend.Models;
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

        [HttpPost("GetListCustomers")]
        public IActionResult GetAll()
        {
            var customers = _context.Customers.ToList();
            return Ok(customers);
        }

        [HttpPost("CheckMail")]
        public IActionResult CheckMail([FromBody] Customer req)
        {
            var checkMail = _context.Customers.FirstOrDefault(c =>
            c.Email == req.Email);
            if (checkMail != null)
            {
                return Ok(checkMail);
            }
            else
            {
                // Không tìm thấy khách hàng phù hợp hoặc thông tin đăng nhập không hợp lệ
                return BadRequest("Không tìm thấy email");
            }
        }


        public class LoginModel
        {
            public string Email { get; set; } = null!;
            public string Password { get; set; } = null!;
            public string Fullname { get; set; }

        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] Customer loginModel)
        {
            var customer = _context.Customers.FirstOrDefault(c =>
            c.Email == loginModel.Email && c.Password == loginModel.Password);

            if (customer != null)
            {
                return Ok(customer);
            }
            else
            {
                // Không tìm thấy khách hàng phù hợp hoặc thông tin đăng nhập không hợp lệ
                return BadRequest("Invalid email or password");
            }
        }




        [HttpPost("CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] LoginModel request)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(c => c.Email == request.Email);
            // Kiểm tra thông tin đầy đủ
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Fullname))
            {
                return BadRequest("Email and Password or Fullname are required fields.");
            }
            else if (existingCustomer != null)
            {
                return BadRequest("Email already exists.");
            }
            else
            {
                // Tạo mới đối tượng Customer từ yêu cầu
                var customer = new Customer
                {
                    Email = request.Email,
                    Password = request.Password,
                    Fullname = request.Fullname

                };

                // Thêm đối tượng Customer mới vào cơ sở dữ liệu
                _context.Customers.Add(customer);
                _context.SaveChanges();

                // Trả về đối tượng Customer đã được tạo mới
                return Ok(customer);
            }

         
        }


        [HttpPost("UpdateCustomer")]
        public IActionResult UpdateCategory([FromBody] Customer req)
        {
            int id = req.Id;
            string email = req.Email;

            var customer = _context.Customers.FirstOrDefault(c => c.Id == id || c.Email == email);

            if (customer == null)
            {
                return NotFound(); // Return HTTP 404 Not Found if the category is not found
            }
            else if(!string.IsNullOrEmpty(req.Fullname) && !string.IsNullOrEmpty(req.Password))
            {
                customer.Fullname = req.Fullname;
                customer.Password = req.Password;
            }
            else if (!string.IsNullOrEmpty(req.Fullname))
            {
                customer.Fullname = req.Fullname;
            }
            else if (!string.IsNullOrEmpty(req.Password))
            {
                customer.Password = req.Password;
            }
            else
            {
                return BadRequest("Lỗi rồi mấy chế ơi");
            }


            // Save the changes to the database
            _context.SaveChanges();
            return Ok(customer); // Return the updated category as JSON
        }
    }
}
