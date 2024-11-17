using RoadmapDesigner.Server.Models.EntityDTO;
using RoadmapDesigner.Server.Repositories;

namespace RoadmapDesigner.Server.Services
{
    public class ProgramVersionsService : IProgramVersionsService
    {

        private readonly IProgramVersionsRepository _programVersions;
        private readonly ILogger<ProgramVersionsService> _logger;

        public ProgramVersionsService(IProgramVersionsRepository programVersions, ILogger<ProgramVersionsService> logger) 
        {
            _programVersions = programVersions;
            _logger = logger;
        }

        public async Task<IEnumerable<ProgramVersionDTO>> GetAllProgramVersionsAsync()
        {
            var listProgramVersions = await _programVersions.GetAllProgramVersionsAsync();
            return listProgramVersions.Select(l => new ProgramVersionDTO(l)).ToList();
        }

        public async Task<ProgramVersionDTO?> GetProgramVersionDetailsAsync(Guid programVersionId)
        {
            var programVersion = await _programVersions.GetProgramVersionDetailsAsync(programVersionId);

            if (programVersion == null) return null;

            // Маппинг ProgramVersion в ProgramVersionDTO
            return new ProgramVersionDTO
            {
                ProgramVersionID = programVersion.ProgramVersionId,
                AcademicYear = programVersion.AcademicYear,
                CreatedAt = programVersion.CreateDate,
                ProgramCode = programVersion.ProgramCode ?? string.Empty,
                ProgramName = programVersion.ProgramCodeNavigation?.ProgramName ?? string.Empty, // Получаем ProgramName
                Description = programVersion.ProgramCodeNavigation?.Description // Получаем Description
            };
        }
    }
}
