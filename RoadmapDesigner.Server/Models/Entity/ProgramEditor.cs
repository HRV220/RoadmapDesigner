using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class ProgramEditor
{
    public Guid ProgramEditorId { get; set; }

    public Guid EditorId { get; set; }

    public Guid UserId { get; set; }

    public Guid ProgramVersionId { get; set; }

    public virtual Editor Editor { get; set; } = null!;

    public virtual ProgramVersion ProgramVersion { get; set; } = null!;
}
