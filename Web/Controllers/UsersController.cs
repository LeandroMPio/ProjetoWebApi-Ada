using Application.Services;
using Domain.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var users = await _service.List();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var user = await _service.GetById(id);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpGet("getByEmail/{email}")]
        public async Task<IActionResult> GetByEmail([FromRoute] string email)
        {
            var user = await _service.FindByEmail(email);
            return user is null ? NotFound() : Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] BaseUserRequest user)
        {
            var newUser = await _service.Create(user);
            return Ok(newUser);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] UpdateUserRequest user)
        {
            user.Id = id;
            var updateUser = await _service.Update(user);
            return Ok(updateUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
