﻿using System;
using System.Collections.Generic;

namespace RoadmapDesigner.Server.Models.Entity;

public partial class User
{
    public Guid UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string? SecondName { get; set; }

    public string MiddleName { get; set; } = null!;

    public string Login { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly CreatedDate { get; set; }

    public int RoleId { get; set; }

    public virtual ICollection<Editor> Editors { get; set; } = new List<Editor>();

    public virtual UserRole Role { get; set; } = null!;

    public virtual ICollection<StudentProgram> StudentPrograms { get; set; } = new List<StudentProgram>();
}