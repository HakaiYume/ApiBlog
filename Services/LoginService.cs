using ApiBlog.Data;
using ApiBlog.Data.Models;
using ApiBlog.Data.Request;
using ApiBlog.Data.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiBlog.Services
{
    public class LoginService
    {
        private readonly DwiApiBlogContext _context;

        private IConfiguration config;

        public LoginService(DwiApiBlogContext context, IConfiguration configuration)
        {
            _context = context;
            config = configuration;
        }

        public async Task<MsgResponse<User>> Login(LoginRequest login){
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Email == login.email && u.Password == login.Password);
            if(user != default){

                string jwtToken = GenerateToken(user);
                return new MsgResponse<User> {type = "OK", message = jwtToken};

            }else{
                return new MsgResponse<User> {type = "Unauthorized", message = "inicio de sesion fallido"};
            }
        }

        private string GenerateToken(User user)
        {
            var Claims=new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Actor, user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var SecurityToken = new JwtSecurityToken(
                claims: Claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            string Token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return Token;
        }
    }    
}