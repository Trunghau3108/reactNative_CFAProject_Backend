using CFAProject_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Xml;

namespace CFAProject_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly CFAProjectContext _context;
        public SupplierController(CFAProjectContext ctx)
        {
            _context = ctx;
        }

        [HttpPost("GetListSuppliers")]
        public IActionResult GetAll()
        {
            var suppliers = _context.Suppliers.ToList();
            return Ok(suppliers);
        }

    }
}
