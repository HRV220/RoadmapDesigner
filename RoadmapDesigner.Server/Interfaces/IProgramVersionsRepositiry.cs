using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoadmapDesigner.Server.Models.Entity;
using RoadmapDesigner.Server.Models.EntityDTO;

public interface IProgramVersionsRepository
{
    Task<IEnumerable<ProgramVersion>> GetAllProgramVersionsAsync();
    Task<ProgramVersion?> GetProgramVersionDetailsAsync(Guid programVersionId);
}
