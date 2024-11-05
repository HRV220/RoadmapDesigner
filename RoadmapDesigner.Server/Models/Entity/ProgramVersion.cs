using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class ProgramVersion
{
    public Guid ProgramVersionId { get; set; }

    public int AcademicYear { get; set; }

    public DateOnly CreateDate { get; set; }

    public string? ProgramCode { get; set; }

    public virtual Program? ProgramCodeNavigation { get; set; }

    public virtual ICollection<ProgramDiscipline> ProgramDisciplines { get; set; } = new List<ProgramDiscipline>();

    public virtual ICollection<ProgramEditor> ProgramEditors { get; set; } = new List<ProgramEditor>();

    public virtual ICollection<Roadmap> Roadmaps { get; set; } = new List<Roadmap>();

    public virtual ICollection<StudentProgram> StudentPrograms { get; set; } = new List<StudentProgram>();
}
