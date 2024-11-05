using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class StudentDiscipline
{
    public Guid StudentDisciplineId { get; set; }

    public string StatusDiscipline { get; set; } = null!;

    public string Grade { get; set; } = null!;

    public Guid StudentProgramId { get; set; }

    public Guid ProgramVersionId { get; set; }

    public virtual StudentProgram StudentProgram { get; set; } = null!;
}
