using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Models.EntityDTO;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<UserDTO?> GetUserByIdAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        return user == null ? null : new UserDTO(user);
    }

    public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(u => new UserDTO(u)).ToList();
    }

    public async Task<bool> UpdateUserAsync(UserDTO userDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userDto.UserId);
        if (user == null) return false;

        user.UpdateFromDTO(userDto);
        await _userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        await _userRepository.DeleteUserAsync(userId);
        return true;
    }
}
