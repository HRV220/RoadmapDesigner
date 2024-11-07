using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Models.Entity;
using RoadmapDesigner.Server.Models.EntityDTO;
using Microsoft.Extensions.Logging;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoadmapController : ControllerBase
    {

        private readonly RoadmapDesignerContext _context;
        private readonly ILogger<RoadmapController> _logger;

        public RoadmapController(RoadmapDesignerContext context, ILogger<RoadmapController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetListUsers()
        {
            var users = _context.Users.ToListAsync();
            return await users;
        }
       

        [HttpGet("login")]
        public IActionResult Login()
        {
            return File("~/index.html", "text/html");
        }

    }
}
