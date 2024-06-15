using Microsoft.AspNetCore.Mvc;
using ApiBlog.Services;
using ApiBlog.Data.Request;
using ApiBlog.Data.Response;
using ApiBlog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace ApiBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;

        public UserController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int id = 0, string? username = null, string? email = null, DateTime? createdAt = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.Get(id, username, email, createdAt, page, pageSize);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<User>.InternalServerError(ex));
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserRequest user)
        {
            try
            {
                var result = await _service.Create(user);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<User>.InternalServerError(ex));
            }
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UserRequest user)
        {
            try
            {
                var result = await _service.Update(id, user);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<User>.InternalServerError(ex));
            }
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<User>.InternalServerError(ex));
            }
        }

        new private IActionResult Response(MsgResponse<User> result)
        {
            switch (result.type)
            {
                case "Ok":
                    return Ok(result.message);
                case "Unauthorized":
                    return Unauthorized(result.message);
                case "NotFound":
                    return NotFound(result.message);
                default:
                    return NoContent();
            }
        }
    }
}
