using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class Editor
{
    public Guid EditorId { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<ProgramEditor> ProgramEditors { get; set; } = new List<ProgramEditor>();

    public virtual User User { get; set; } = null!;
}
