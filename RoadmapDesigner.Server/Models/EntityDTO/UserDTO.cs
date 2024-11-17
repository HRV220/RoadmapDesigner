using RoadmapDesigner.Server.Models.Entity;

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

        public UserDTO() { }

        public UserDTO(User user)
        {
            UserId = user.UserId;
            FirstName = user.FirstName;
            SecondName = user.SecondName;
            MiddleName = user.MiddleName;
            Login = user.Login;
            Email = user.Email;
            CreatedDate = user.CreatedDate;
            RoleId = user.RoleId;
        }
    }
}
