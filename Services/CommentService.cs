using ApiBlog.Data;
using ApiBlog.Data.Models;
using ApiBlog.Data.Request;
using ApiBlog.Data.Response;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiBlog.Services
{
    public class CommentService
    {
        private readonly DwiApiBlogContext _context;

        public CommentService(DwiApiBlogContext context)
        {
            _context = context;
        }

        public async Task<MsgResponse<Comment>> Get(int id, int? post, int? author, DateTime? createdAt, int page, int pageSize)
        {
            var result = await _context.Comments
                .Where(c =>
                    (id == default || c.Id == id) &&
                    (post == null || c.Post == post) &&
                    (author == null || c.Author == author) &&
                    (createdAt == default || c.CreatedAt == createdAt))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CommentResponse
                {
                    Id = c.Id,
                    Post = c.Post,
                    Author = c.Author,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt
                }).ToListAsync();

            if (!result.Any()) return new MsgResponse<Comment> { type = "NotFound", message = $"No se encontraron comentarios con los siguientes parámetros de búsqueda: ID ({id}), Post ({post}), Author ({author}), Created At ({createdAt})" };
            return Messages<Comment>.Ok(result);
        }

        public async Task<Comment?> GetById(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task<MsgResponse<Comment>> Create(CommentRequest commentRequest)
        {
            var comment = new Comment
            {
                Post = commentRequest.Post,
                Author = commentRequest.Author,
                Content = commentRequest.Content,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Messages<Comment>.Succeed("Comentario", "creó");
        }

        public async Task<MsgResponse<Comment>> Update(int id, CommentRequest commentRequest)
        {
            var comment = await GetById(id);
            if (comment is null) return Messages<Comment>.NotFound("Comentario", "ID", id.ToString());

            comment.Post = commentRequest.Post;
            comment.Author = commentRequest.Author;
            comment.Content = commentRequest.Content;
            comment.CreatedAt = commentRequest.CreatedAt;

            await _context.SaveChangesAsync();

            return Messages<Comment>.Succeed("Comentario", "actualizó");
        }

        public async Task<MsgResponse<Comment>> Delete(int id)
        {
            var comment = await GetById(id);
            if (comment is null) return Messages<Comment>.NotFound("Comentario", "ID", id.ToString());

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Messages<Comment>.Succeed("Comentario", "eliminó");
        }
    }
}
