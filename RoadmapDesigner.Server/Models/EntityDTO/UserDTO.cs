namespace RoadmapDesigner.Server.Models.EntityDTO
{
    public class UserDTO
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; } = null!;

        public string? SecondName { get; set; }

        public string MiddleName { get; set; } = null!;

        public string Login { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateOnly CreatedDate { get; set; }

        public int RoleId { get; set; }
    }
}
