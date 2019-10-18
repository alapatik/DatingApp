using System.Threading.Tasks;
using DatingApp.Api.Data;
using DatingApp.Api.DTO;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.Api.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo;
        private IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserAuthRegisterDTO registerDTO)
        {
            if(await _repo.UserExists(registerDTO.username))
                return BadRequest("User already exists");
            var user = new User(){ Username = registerDTO.username};
            await _repo.Register(user, registerDTO.password);
            return StatusCode(201);   
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserAuthLoginDTO loginDTO)
        {
            var userFromRepo = await _repo.Login(loginDTO.Username, loginDTO.Password);
            if(userFromRepo == null)
                return Unauthorized();
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });            
        }
    }
}