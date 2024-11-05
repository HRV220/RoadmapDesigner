using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class Roadmap
{
    public Guid RoadmapId { get; set; }

    public DateOnly CreatedDate { get; set; }

    public Guid ProgramVersionId { get; set; }

    public virtual ProgramVersion ProgramVersion { get; set; } = null!;
}
