using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class Discipline
{
    public Guid DisciplineId { get; set; }

    public string DisciplineName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProgramDiscipline> ProgramDisciplines { get; set; } = new List<ProgramDiscipline>();
}
