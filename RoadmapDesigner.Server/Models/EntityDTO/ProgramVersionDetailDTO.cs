namespace RoadmapDesigner.Server.Models.EntityDTO
{
    public class ProgramVersionDetailDTO
    {
        public string ProgramCode { get; set; } = string.Empty; // Код программы
        public string ProgramName { get; set; } = string.Empty; // Название программы
        public string? Description { get; set; } // Описание программы
        public List<ProgramDisciplineDTO> Disciplines { get; set; } = new(); // Список дисциплин
    }
}
