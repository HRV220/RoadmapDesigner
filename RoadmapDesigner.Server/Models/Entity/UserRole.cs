using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class UserRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
