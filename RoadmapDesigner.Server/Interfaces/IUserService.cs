using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoadmapDesigner.Server.Models.EntityDTO;

public interface IUserService
{
    Task<UserDTO?> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<UserDTO>> GetAllUsersAsync();
    Task<bool> UpdateUserAsync(UserDTO userDto);
    Task<bool> DeleteUserAsync(Guid userId);
}

