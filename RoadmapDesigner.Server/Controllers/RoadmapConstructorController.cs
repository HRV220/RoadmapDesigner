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
    public class RoadmapConstructorController : ControllerBase
    {

        private readonly RoadmapDesignerContext _context;
        private readonly ILogger<RoadmapConstructorController> _logger;

        public RoadmapConstructorController(RoadmapDesignerContext context, ILogger<RoadmapConstructorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetListDiscipline()
        {
            var listDiscipline  = await _context.Disciplines.Select (d => new Discipline
            {
                DisciplineName = d.DisciplineName,
                Description = d.Description,
                
            })
            .ToListAsync();

            return Ok(listDiscipline);
        }

    }
}
