using Microsoft.AspNetCore.Mvc;
using ApiBlog.Services;
using ApiBlog.Data.Request;
using ApiBlog.Data.Response;
using ApiBlog.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System;

namespace ApiBlog.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;

        public CommentController(CommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int Id = default, int? Post = null, int? Author = null, DateTime? CreatedAt = null, int Page = 1, int PageSize = 10)
        {
            try
            {
                var result = await _service.Get(Id, Post, Author, CreatedAt, Page, PageSize);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<Comment>.InternalServerError(ex));
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create(CommentRequest comment)
        {
            try
            {
                var result = await _service.Create(comment);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<Comment>.InternalServerError(ex));
            }
        }

        [HttpPut("update/{id}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, CommentRequest comment)
        {
            try
            {
                var result = await _service.Update(id, comment);
                return Response(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, Messages<Comment>.InternalServerError(ex));
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
                return StatusCode(500, Messages<Comment>.InternalServerError(ex));
            }
        }

        new private IActionResult Response(MsgResponse<Comment> result)
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
