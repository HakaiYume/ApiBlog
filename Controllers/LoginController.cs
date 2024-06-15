using Microsoft.AspNetCore.Mvc;
using ApiBlog.Services;
using ApiBlog.Data.Request;
using ApiBlog.Data.Response;
using System.Threading.Tasks;
using ApiBlog.Data.Models;

namespace ApiBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _service;

        public LoginController(LoginService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await _service.Login(loginRequest);
                return Response(result);
            }
            catch (Exception ex)
            {
                return error(ex);
            }
        }

        new private IActionResult Response(MsgResponse<User> result)
        {
            switch (result.type)
            {
                case "OK":
                    return Ok(result.message);
                case "Unauthorized":
                    return Unauthorized(result.message);
                case "NotFound":
                    return NotFound(result.message);
                default:
                    return NoContent();
            }
        }

        private IActionResult error(Exception ex)
        {
            return StatusCode(500, new { msg = "Ocurrió un error inesperado", Error = ex });
        }
    }
}
