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
    public class UserService
    {
        private readonly DwiApiBlogContext _context;
        private readonly UserProfileService _service;

        public UserService(DwiApiBlogContext context, UserProfileService service)
        {
            _context = context;
            _service = service;
        }

        public async Task<MsgResponse<User>> Get(int id, string? username, string? email, DateTime? createdAt, int page, int pageSize)
        {
            var result = await _context.Users
                .Where(u =>
                    (id == 0 || u.Id == id) &&
                    (string.IsNullOrEmpty(username) || u.Username.Contains(username)) &&
                    (string.IsNullOrEmpty(email) || u.Email.Contains(email)) &&
                    (createdAt == default || u.CreatedAt == createdAt))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt,
                    Profile = u.UserProfile,
                }).ToListAsync();

            if (!result.Any()) return new MsgResponse<User> { type = "NotFound", message = $"No se encontraron usuarios con los siguientes parámetros de búsqueda: ID ({id}), Username ({username}), Email ({email}), CreatedAt ({createdAt})" };
            return Messages<User>.Ok(result);
        }

        public async Task<User?> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<MsgResponse<User>> Create(UserRequest userRequest)
        {
            var user = new User
            {
                Username = userRequest.Username,
                Email = userRequest.Email,
                Password = userRequest.Password,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await _service.Create(user.Id, userRequest.Bio, userRequest.ProfileImageUrl);

            return Messages<User>.Succeed("Usuario", "creó");
        }

        public async Task<MsgResponse<User>> Update(int id, UserRequest userRequest)
        {
            var user = await GetById(id);
            if (user is null) return Messages<User>.NotFound("Usuario", "ID", id.ToString());

            user.Username = userRequest.Username;
            user.Email = userRequest.Email;
            user.Password = userRequest.Password;
            user.CreatedAt = userRequest.CreatedAt;

            await _context.SaveChangesAsync();

            await _service.Update(user.UserProfile.UserId, userRequest.Bio, userRequest.ProfileImageUrl);

            return Messages<User>.Succeed("Usuario", "actualizó");
        }

        public async Task<MsgResponse<User>> Delete(int id)
        {
            var user = await GetById(id);
            if (user is null) return Messages<User>.NotFound("Usuario", "ID", id.ToString());

            await _service.Delete(user.UserProfile.UserId);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Messages<User>.Succeed("Usuario", "eliminó");
        }
    }
}
