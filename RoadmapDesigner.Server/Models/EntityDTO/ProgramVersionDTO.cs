namespace RoadmapDesigner.Server.Models.EntityDTO
{
    public class ProgramVersionDTO
    {
        public Guid ProgramVersionID { get; set; }
        public int AcademicYear { get; set; }
        public DateOnly CreatedAt { get; set; }
        public string ProgramCode { get; set; } = null!;
        public string ProgramName { get; set; } = null!;
        public string? Description { get; set; }
    }
}

