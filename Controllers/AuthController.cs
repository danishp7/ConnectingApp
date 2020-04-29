using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
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
                return BadRequest("User already exist with this username");

            // now we create new user
            var newUser = _mapper.Map<User>(creds);


            var userCreated = await _repo.Register(newUser, creds.Password);

            var userToReturn = _mapper.Map<UserDetailDto>(userCreated);


            // we'll return the created user with route
            // getuser in string is name of the method of GetUser in usercontroller [http({id}, name="GetUser")]
            // to pass id we use GetUser method
            // we have to return detaildto object so we first mapped user to detail and then pass it as 3rd arg
            return CreatedAtRoute("GetUser", new { Controller = "User", id = userCreated.Id }, userToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDto loginCreds)
        {
            // for demo purpose we thro exception here
            // throw new Exception("computer says no!!!!");

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
            }); 

        }
    }
}