using Microsoft.AspNetCore.Mvc;
using Task5.Models;

namespace Task5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {

        private readonly string filePath = "users.csv";
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {

            try
            {
                using (StreamReader streamReader = new(filePath,
                    new FileStreamOptions()
                    {
                        Mode = FileMode.Open,
                        Access = FileAccess.Read
                    }))
                {
                    string? line;
                    string? header = await streamReader.ReadLineAsync();

                    while ((line = await streamReader.ReadLineAsync()) != null)
                    {
                        var userData = line.Split(';');

                        var userId = Convert.ToInt32(userData[0]);

                        if (id.Equals(userId))
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
                    return NotFound("Пользователь с таким id не найден");
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
