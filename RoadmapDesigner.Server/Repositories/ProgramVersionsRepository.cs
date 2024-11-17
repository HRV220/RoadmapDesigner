using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Models.Entity;

namespace RoadmapDesigner.Server.Repositories
{
    public class ProgramVersionsRepository : IProgramVersionsRepository
    {
        private readonly RoadmapDesignerContext _context;

        public ProgramVersionsRepository(RoadmapDesignerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProgramVersion>> GetAllProgramVersionsAsync()
        {
            return await _context.ProgramVersions.ToListAsync();
        }
        public async Task<ProgramVersion?> GetProgramVersionDetailsAsync(Guid programVersionId)
        {
            return await _context.Set<ProgramVersion>()
                .Include(pv => pv.ProgramCodeNavigation) // Включаем связанные данные из Program
                .FirstOrDefaultAsync(pv => pv.ProgramVersionId == programVersionId);
        }
    }
}
