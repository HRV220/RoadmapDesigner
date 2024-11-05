using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class ProgramDiscipline
{
    public Guid ProgramDisciplineId { get; set; }

    public int Semester { get; set; }

    public Guid ProgramVersionId { get; set; }

    public Guid DisciplineId { get; set; }

    public virtual Discipline Discipline { get; set; } = null!;

    public virtual ProgramVersion ProgramVersion { get; set; } = null!;
}
