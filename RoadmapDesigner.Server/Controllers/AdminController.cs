using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RoadmapDesigner.Server.Models.EntityDTO;

namespace RoadmapDesigner.Server.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IProgramVersionsService _programVersionService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IUserService userService, IProgramVersionsService programVersionService, ILogger<AdminController> logger)
        {
            _userService = userService;
            _programVersionService = programVersionService;
            _logger = logger;
        }

        [HttpGet("editUser/{userId}")]
        public async Task<ActionResult<UserDTO>> EditUser(Guid userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return user == null ? NotFound(new { Message = "User not found" }) : Ok(user);
        }

        [HttpPost("editUser")]
        public async Task<ActionResult> EditUser([FromBody] UserDTO userDto)
        {
            var result = await _userService.UpdateUserAsync(userDto);
            return result ? Ok(new { Message = "User updated successfully." }) : NotFound(new { Message = "User not found" });
        }

        [HttpGet("usersList")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var result = await _userService.DeleteUserAsync(userId);
            return result ? Ok(new { Message = "User successfully deleted" }) : NotFound(new { Message = "User not found" });
        }

        [HttpGet("GetProgramVersions")]
        public async Task<ActionResult<IEnumerable<ProgramVersionDTO>>> GetProgramVersions()
        {
            var programVersions = await _programVersionService.GetAllProgramVersionsAsync();
            return Ok(programVersions);
        }

        [HttpGet("program-version/{programVersionId}")]
        public async Task<ActionResult<ProgramVersionDetailDTO>> GetProgramVersionDetails(Guid programVersionId)
        {
            var programDetails = await _programVersionService.GetProgramVersionDetailsAsync(programVersionId);
            return programDetails == null ? NotFound(new { Message = "Program version not found" }) : Ok(programDetails);
        }
    }
}

