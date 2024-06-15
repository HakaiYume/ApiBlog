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
    public class UserProfileService
    {
        private readonly DwiApiBlogContext _context;

        public UserProfileService(DwiApiBlogContext context)
        {
            _context = context;
        }

        public async Task<MsgResponse<UserProfile>> Get(int userId, string? bio, string? profileImageUrl, int page, int pageSize)
        {
            var result = await _context.UserProfiles
            .Where(up =>
                (userId == default || up.UserId == userId) &&
                (string.IsNullOrEmpty(bio) || up.Bio.Contains(bio)) &&
                (string.IsNullOrEmpty(profileImageUrl) || up.ProfileImageUrl.Contains(profileImageUrl)))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(up => new UserProfileResponse
            {
                UserId = up.UserId,
                Bio = up.Bio,
                ProfileImageUrl = up.ProfileImageUrl
            }).ToListAsync();

            if (!result.Any()) return new MsgResponse<UserProfile> { type = "NotFound", message = $"No se encontraron perfiles de usuario con los siguientes parámetros de búsqueda: UserId ({userId}), Bio ({bio}), ProfileImageUrl ({profileImageUrl})" };
            return Messages<UserProfile>.Ok(result);
        }

        public async Task<UserProfile?> GetById(int id)
        {
            return await _context.UserProfiles.FindAsync(id);
        }

        public async Task<MsgResponse<UserProfile>> Create(int userId,string? bio, string? profileImageUrl)
        {
            var userProfile = new UserProfile
            {
                UserId = userId,
                Bio = bio,
                ProfileImageUrl = profileImageUrl
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return Messages<UserProfile>.Succeed("UserProfile", "creó");
        }

        public async Task<MsgResponse<UserProfile>> Update(int id, string? bio, string? profileImageUrl)
        {
            var userProfile = await GetById(id);
            if (userProfile is null) return Messages<UserProfile>.NotFound("UserProfile", "ID", id.ToString());

            userProfile.Bio = bio;
            userProfile.ProfileImageUrl = profileImageUrl;

            await _context.SaveChangesAsync();

            return Messages<UserProfile>.Succeed("UserProfile", "actualizó");
        }

        public async Task<MsgResponse<UserProfile>> Delete(int id)
        {
            var userProfile = await GetById(id);
            if (userProfile is null) return Messages<UserProfile>.NotFound("UserProfile", "ID", id.ToString());

            _context.UserProfiles.Remove(userProfile);
            await _context.SaveChangesAsync();

            return Messages<UserProfile>.Succeed("UserProfile", "eliminó");
        }
    }
}
