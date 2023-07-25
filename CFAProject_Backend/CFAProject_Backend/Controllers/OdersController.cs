﻿using CFAProject_Backend.Models;
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
    }

}
