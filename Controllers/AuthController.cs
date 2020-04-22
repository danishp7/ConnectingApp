using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ConnectingApp.API.Data;
using ConnectingApp.API.Dtos;
using ConnectingApp.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ConnectingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        // post register method
        [HttpPost("register")]
        public async Task<IActionResult> PostUser(/* in case [Apicontroller not using then we need to specify 'FromBody']*/
                                                    AuthDto creds)
        {
            // first check the state in case we are not using [apicontroller]
            // if (!ModelState.IsValid)
            //    return BadRequest("Username and password is required field!");

            // we do not have explicitly check the validate state as [apicontroller] do things for us.

            // for the irrespective of the case used by user
            creds.UserName = creds.UserName.ToLower();

            // if user already exist or not
            if (await _repo.UserExist(creds.UserName))
                return BadRequest("user already exist with this username");

            // now we create new user
            var newUser = new User
            {
                UserName = creds.UserName
            };
            var userCreated = await _repo.Register(newUser, creds.Password);

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDto loginCreds)
        {
            // first we check if user exist or not
            var user = await _repo.Login(loginCreds.UserName.ToLower(), loginCreds.Password);
            if (user == null)
                return Unauthorized();

            // now we create token
            // first create claim
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
            };

            // now create a key 
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            // now we need to set how to generate token using key so
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // now create token descriptor to write the token 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = creds
            };

            // now we need token handler
            var tokenHandler = new JwtSecurityTokenHandler();

            // now we create the token that have jwt token value
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // now we simply return this with ok status
            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            }); ;

        }
    }
}