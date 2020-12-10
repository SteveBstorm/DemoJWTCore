using DemoApiJWT.Helpers;
using DemoApiJWT.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoApiJWT.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1,
                FirstName = "Termin",
                LastName = "Ator",
                UserName = "terminator",
                Password = "test"
            },

            new User
            {
                Id = 2,
                FirstName = "Chuck",
                LastName = "Norris",
                UserName = "chuck",
                Password ="test"
            }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> app)
        {
            _appSettings = app.Value;
        }

        public User Authenticate(string username, string password)
        {
            User user = _users.SingleOrDefault(x => x.UserName == username && x.Password == password);

            if (user == null) return null;

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user;
        }

        

        public IEnumerable<User> GetAll()
        {
            return _users;
        }
    }
}
