using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RoadmapDesigner.Server.Models.Entity;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid userId);
}
