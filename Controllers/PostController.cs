using Microsoft.AspNetCore.Mvc;
using ApiBlog.Services;
using ApiBlog.Data.Request;
using ApiBlog.Data.Response;
using ApiBlog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ApiBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly PostService _service;

        public PostController(PostService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id = null, string? title = null, DateTime? createdAt = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await _service.Get(id, title, createdAt, page, pageSize);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<Post>.InternalServerError(ex));
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(PostRequest postRequest)
        {
            try
            {
                var result = await _service.Create(postRequest);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<Post>.InternalServerError(ex));
            }
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, PostRequest postRequest)
        {
            try
            {
                var result = await _service.Update(id, postRequest);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<Post>.InternalServerError(ex));
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
                return StatusCode(500, Messages<Post>.InternalServerError(ex));
            }
        }

        new private IActionResult Response(MsgResponse<Post> result)
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
