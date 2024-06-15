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
    public class PostService
    {
        private readonly DwiApiBlogContext _context;

        public PostService(DwiApiBlogContext context)
        {
            _context = context;
        }

        public async Task<MsgResponse<Post>> Get(int? id, string? title, DateTime? createdAt, int page, int pageSize)
        {
            var result = await _context.Posts
                .Where(p =>
                    (id == null || p.Id == id) &&
                    (string.IsNullOrEmpty(title) || p.Title.Contains(title)) &&
                    (createdAt == null || p.CreatedAt == createdAt))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new PostResponse
                {
                    Id = p.Id,
                    Author = p.Author,
                    Title = p.Title,
                    Content = p.Content,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                }).ToListAsync();

            if (!result.Any()) return new MsgResponse<Post> { type = "NotFound", message = $"No se encontraron posts con los siguientes parámetros de búsqueda: ID ({id}), Título ({title}), Fecha de Creación ({createdAt})" };
            return Messages<Post>.Ok(result);
        }

        public async Task<Post?> GetById(int id)
        {
            return await _context.Posts.FindAsync(id);
        }

        public async Task<MsgResponse<Post>> Create(PostRequest postRequest)
        {
            var post = new Post
            {
                Author = postRequest.Author,
                Title = postRequest.Title,
                Content = postRequest.Content,
                ImageUrl = postRequest.ImageUrl,
                CreatedAt = postRequest.CreatedAt,
                UpdatedAt = postRequest.UpdatedAt
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Messages<Post>.Succeed("Post", "creó");
        }

        public async Task<MsgResponse<Post>> Update(int id, PostRequest postRequest)
        {
            var post = await GetById(id);
            if (post is null) return Messages<Post>.NotFound("Post", "ID", id.ToString());
            
            post.Title = postRequest.Title;
            post.Content = postRequest.Content;
            post.ImageUrl = postRequest.ImageUrl;
            post.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return Messages<Post>.Succeed("Post", "actualizó");
        }

        public async Task<MsgResponse<Post>> Delete(int id)
        {
            var post = await GetById(id);
            if (post is null) return Messages<Post>.NotFound("Post", "ID", id.ToString());
            
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Messages<Post>.Succeed("Post", "eliminó");
        }
    }
}
