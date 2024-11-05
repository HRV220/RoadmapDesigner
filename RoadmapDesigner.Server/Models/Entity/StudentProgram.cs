using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class StudentProgram
{
    public Guid StudentProgramId { get; set; }

    public DateOnly EnrollmentDate { get; set; }

    public Guid UserId { get; set; }

    public Guid ProgramVersionId { get; set; }

    public virtual ProgramVersion ProgramVersion { get; set; } = null!;

    public virtual ICollection<StudentDiscipline> StudentDisciplines { get; set; } = new List<StudentDiscipline>();

    public virtual User User { get; set; } = null!;
}
