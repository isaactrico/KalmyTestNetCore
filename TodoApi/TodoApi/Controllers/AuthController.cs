using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly UserContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration, UserContext context)
        {
            _configuration = configuration;

            _context = context;

            if (_context.UserItems.Count() == 0)
            {
                _context.UserItems.Add(new UserItem { Name = "User", Password = "Pass"});
                _context.SaveChanges();
            }

        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(UserItem userIn)
        {
            
            
            var listUsers = await _context.UserItems.ToListAsync();

            foreach(UserItem user in listUsers)
            {
                if(user.Name == userIn.Name && user.Password == userIn.Password)
                {
                    // Read secretKey from appsettings
                    var secretKey = _configuration.GetValue<string>("SecretKey");
                    var key = Encoding.ASCII.GetBytes(secretKey);
                    var signingKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key);

                    // Create user's claims
                    var claims = new ClaimsIdentity(new[]
                    {
                    new Claim(ClaimTypes.NameIdentifier, user.Name),
                    new Claim(ClaimTypes.Email, user.Password)
                    });


                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256),
                        Subject = claims,
                        Expires = DateTime.UtcNow.AddDays(1),
                        NotBefore = DateTime.Now
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var createdToken = tokenHandler.CreateToken(tokenDescriptor);

                    return tokenHandler.WriteToken(createdToken);
                }
            }

            return BadRequest();
        }
    }
}
