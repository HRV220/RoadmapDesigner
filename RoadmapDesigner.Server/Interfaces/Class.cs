using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoadmapDesigner.Server.Models.EntityDTO;

public interface IProgramVersionService
{
    Task<IEnumerable<ProgramVersionDTO>> GetAllProgramVersionsAsync();
    Task<ProgramVersionDetailDTO?> GetProgramVersionDetailsAsync(Guid programVersionId);
}
