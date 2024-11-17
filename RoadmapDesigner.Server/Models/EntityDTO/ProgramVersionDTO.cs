using RoadmapDesigner.Server.Models.Entity;

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

        public ProgramVersionDTO() { }

        // Конструктор, принимающий объект ProgramVersion
        public ProgramVersionDTO(ProgramVersion programVersion)
        {
            ProgramVersionID = programVersion.ProgramVersionId;
            AcademicYear = programVersion.AcademicYear;
            CreatedAt = programVersion.CreateDate;
            ProgramCode = programVersion.ProgramCode ?? string.Empty;

            // Извлекаем ProgramName и Description из ProgramCodeNavigation, если он не null
            ProgramName = programVersion.ProgramCodeNavigation?.ProgramName ?? string.Empty;
            Description = programVersion.ProgramCodeNavigation?.Description;
        }
    }


}

