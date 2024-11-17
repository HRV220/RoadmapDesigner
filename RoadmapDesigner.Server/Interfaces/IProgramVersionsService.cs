using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoadmapDesigner.Server.Models.EntityDTO;

public interface IProgramVersionsService
{
    Task<IEnumerable<ProgramVersionDTO>> GetAllProgramVersionsAsync();
    Task<ProgramVersionDTO?> GetProgramVersionDetailsAsync(Guid programVersionId);
}
