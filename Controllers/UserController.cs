using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ConnectingApp.API.Data;
using ConnectingApp.API.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ConnectingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserRepository repo, IMapper mapper, ILogger<UserController> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        // get all users
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            
            // to map in dto
            var mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(mappedUsers);
        }

        // get specific user
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            if (user == null)
            {
                return BadRequest();
            }

            // to map
            var mappedUser = _mapper.Map<UserDetailDto>(user);
            
            return Ok(mappedUser);
        }
    }
}