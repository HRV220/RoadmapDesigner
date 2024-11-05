using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class Program
{
    public string ProgramCode { get; set; } = null!;

    public string ProgramName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<ProgramVersion> ProgramVersions { get; set; } = new List<ProgramVersion>();
}
