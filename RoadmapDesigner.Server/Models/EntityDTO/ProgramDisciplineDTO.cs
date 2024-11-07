namespace RoadmapDesigner.Server.Models.EntityDTO
{
    public class ProgramDisciplineDTO
    {
        public Guid ProgramDisciplineId { get; set; }
        public string DisciplineName { get; set; } = string.Empty; // Название дисциплины
        public string? Description { get; set; } // Описание дисциплины
        public int Semester { get; set; } // Семестр, в котором преподается дисциплина
    }
}
