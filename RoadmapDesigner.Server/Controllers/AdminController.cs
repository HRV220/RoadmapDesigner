using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoadmapDesigner.Server.Models.Entity;
using RoadmapDesigner.Server.Models.EntityDTO;

namespace RoadmapDesigner.Server.Controllers
{
    public class AdminController : ControllerBase
    {

        private readonly RoadmapDesignerContext _context;
        private readonly ILogger<RoadmapController> _logger;

        public AdminController(RoadmapDesignerContext context, ILogger<RoadmapController> logger)
        {
            _context = context;
            _logger = logger;
        }


        // Метод, доступный только для пользователей с ролью "admin"
        [HttpGet("editUser/{userId}")]
        //[Authorize(Roles = "admin")] //Ограничиваем доступ к методу, разрешая только пользователям с ролью "admin"
        public async Task<ActionResult<UserDTO>> EditUser(Guid userId)
        {
            _logger.LogInformation("Request to retrieve user with ID {UserId}", userId);
            try
            {
                // Выполняем запрос к базе данных для поиска пользователя с заданным userId
                // Сразу проецируем данные в объект UserDto, чтобы выбрать только необходимые поля
                var userDto = await _context.Users
                    .Where(u => u.UserId == userId) // Фильтруем по userId
                    .Select(u => new UserDTO // Проецируем данные в UserDto
                    {
                        UserId = u.UserId,
                        Login = u.Login,
                        FirstName = u.FirstName,
                        SecondName = u.SecondName,
                        MiddleName = u.MiddleName,
                        Email = u.Email,
                        CreatedDate = u.CreatedDate
                    })
                    .FirstOrDefaultAsync(); // Выполняем запрос и получаем результат

                // Если пользователь не найден, возвращаем ответ с кодом 404 и сообщением
                if (userDto == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    return NotFound(new { Message = "User not found" });
                }

                // Возвращаем успешный результат с данными пользователя (UserDto) и статусом 200 (OK)
                _logger.LogInformation("Successfully retrieved user with ID {UserId}", userId);
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                // Логируем исключение для отслеживания ошибок (код логгирования закомментирован, но может быть добавлен)
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);

                // Возвращаем ответ с кодом 500 (Internal Server Error) и сообщением об ошибке
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }

        [HttpPost("editUser")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult> EditUser([FromBody] UserDTO userDto)
        {
            if (userDto == null)
            {
                _logger.LogWarning("Invalid user data received for update");
                return BadRequest(new { Message = "Invalid user data." });
            }

            try
            {
                // Поиск пользователя по UserId
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userDto.UserId);
                if (user == null)
                {
                    _logger.LogWarning("User with ID {UserId} not found", userDto.UserId);
                    return NotFound(new { Message = "User not found" });
                }

                // Обновление данных пользователя
                user.Login = userDto.Login;
                user.FirstName = userDto.FirstName;
                user.SecondName = userDto.SecondName;
                user.MiddleName = userDto.MiddleName;
                user.Email = userDto.Email;
                user.CreatedDate = userDto.CreatedDate;

                // Сохранение изменений
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} updated successfully", userDto.UserId);

                // Возвращаем успешный ответ
                return Ok(new { Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                _logger.LogError(ex, "Error updating user with ID {UserId}", userDto.UserId);

                // Возвращаем статус ошибки 500
                return StatusCode(500, new { Message = "An error occurred while updating the user." });
            }
        }

        [HttpGet("usersList")]
        //[Authorize(Roles = "admin")] // Ограничиваем доступ для admin
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Request to retrieve all users");

                // Извлекаем всех пользователей и проецируем их в UserDTO
                var users = await _context.Users
                    .Select(u => new UserDTO
                    {
                        UserId = u.UserId,
                        Login = u.Login,
                        FirstName = u.FirstName,
                        SecondName = u.SecondName,
                        MiddleName = u.MiddleName,
                        Email = u.Email,
                        CreatedDate = u.CreatedDate
                    })
                    .ToListAsync();

                // Проверяем, есть ли пользователи
                if (users == null || !users.Any())
                {
                    _logger.LogWarning("No users found");
                    return NotFound(new { Message = "No users found" });
                }

                _logger.LogInformation("Successfully retrieved list of users");
                return Ok(users); // Возвращаем список пользователей со статусом 200 OK
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving list of users");
                return StatusCode(500, new { Message = "An error occurred while retrieving users." });
            }
        }

        // Метод для удаления пользователя по его ID (доступен только для admin)
        [HttpDelete("delete/{userId}")]
        //[Authorize(Roles = "admin")] // Ограничиваем доступ для admin
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            _logger.LogInformation("Request to delete user with ID {UserId}", userId);

            try
            {
                // Ищем пользователя по userId
                var user = await _context.Users.FindAsync(userId);

                if (user == null)
                {
                    // Если пользователь не найден, возвращаем статус 404 NotFound
                    _logger.LogWarning("User with ID {UserId} not found", userId);
                    return NotFound(new { Message = "User not found" });
                }

                // Удаляем пользователя из контекста и сохраняем изменения
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                _logger.LogInformation("User with ID {UserId} successfully deleted", userId);
                return Ok(new { Message = "User successfully deleted" });
            }
            catch (Exception ex)
            {
                // Логируем исключение и возвращаем ошибку
                _logger.LogError(ex, "Error deleting user with ID {UserId}", userId);
                return StatusCode(500, new { Message = "An error occurred while deleting the user." });
            }
        }

        // Метод для получения ProgramVersionDTO
        [HttpGet("GetProgramVersions")]
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<ProgramVersionDTO>>> GetProgramVersions()
        {
            // Загружаем ProgramVersion вместе с Program
            var programVersions = await _context.ProgramVersions
                .Include(pv => pv.ProgramCodeNavigation) // Включаем связанные данные Program
                .Select(pv => new ProgramVersionDTO // Преобразуем данные в DTO
                {
                    AcademicYear = pv.AcademicYear,
                    ProgramCode = pv.ProgramCode ?? string.Empty,
                    ProgramName = pv.ProgramCodeNavigation.ProgramName ?? string.Empty,
                })
                .ToListAsync();

            return Ok(programVersions);
        }

        [HttpGet("program-version/{programVersionId}")]
        public async Task<ActionResult<ProgramVersionDetailDTO>> GetProgramVersionDetails(Guid programVersionId)
        {
            // Ищем нужную версию программы вместе с дисциплинами и данными по каждой дисциплине
            var programVersion = await _context.ProgramVersions
                .Include(pv => pv.ProgramCodeNavigation) // Включаем данные о программе
                .Include(pv => pv.ProgramDisciplines) // Включаем дисциплины
                    .ThenInclude(pd => pd.Discipline) // Включаем данные по каждой дисциплине
                .FirstOrDefaultAsync(pv => pv.ProgramVersionId == programVersionId);

            // Проверка, если версия программы не найдена
            if (programVersion == null)
            {
                return NotFound(new { Message = "Program version not found" });
            }

            // Маппим данные в DTO
            var programDetailDto = new ProgramVersionDetailDTO
            {
                ProgramCode = programVersion.ProgramCodeNavigation.ProgramCode,
                ProgramName = programVersion.ProgramCodeNavigation.ProgramName,
                Description = programVersion.ProgramCodeNavigation.Description,
                Disciplines = programVersion.ProgramDisciplines
                    .Select(pd => new ProgramDisciplineDTO
                    {
                        ProgramDisciplineId = pd.ProgramDisciplineId,
                        DisciplineName = pd.Discipline.DisciplineName, // Название дисциплины
                        Semester = pd.Semester, // Семестр
                        Description = pd.Discipline.Description // Описание дисциплины
                    })
                    .ToList()
            };

            return Ok(programDetailDto); // Возвращаем DTO с данными программы и дисциплинами
        }

    }
}

