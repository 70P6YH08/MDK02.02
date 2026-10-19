using Microsoft.AspNetCore.Mvc;
using Task5.Models;
using Task5.Services;

namespace Task5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly string filePath = "users.csv";

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            try
            {
                if (!int.TryParse(id, out int userId))
                    return BadRequest(new
                    {
                        error = "Invalid ID format",
                        statusCode = 400
                    });

                using (var streamReader = new StreamReader(filePath))
                {
                    string? line;
                    string? header = await streamReader.ReadLineAsync();

                    while ((line = await streamReader.ReadLineAsync()) != null)
                    {
                        var userData = line.Split(';');
                        var convertedUserId = Convert.ToInt32(userData[0]);

                        if (userId.Equals(convertedUserId))
                        {
                            return Ok(new User
                            {
                                Id = userId,
                                FirstName = userData[1],
                                SecondName = userData[2],
                                Login = userData[3],
                                Age = Convert.ToInt32(userData[4])
                            });
                        }
                    }
                    return NotFound($"Пользователь с id = {userId} не найден");
                }
            }
            catch (FileNotFoundException ex)
            {
                LoggerException.LogExceptionToFile(ex.Message);
                return NotFound();
            }
            catch (Exception ex)
            {
                LoggerException.LogExceptionToFile(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
