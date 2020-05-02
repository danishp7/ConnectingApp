using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ConnectingApp.API.Data;
using ConnectingApp.API.Dtos;
using ConnectingApp.API.Helpers;
using ConnectingApp.API.Models;
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
        // as we are sending pagination ifo in header so we need to pass params inside this method
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {
            // first we get the logged in user
            var loggedInUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);


            // we need to set the current logged in user id to params id
            // but first we get current logged in user
            var loggedInUser = await _repo.GetUser(loggedInUserId);

            // now we set the params id
            userParams.UserId = loggedInUserId;
                
            // now we set the filter for gender 
            // the opposite gender will be set based on the logged in user's gender
            // gender and id where clause add in repo

            if (string.IsNullOrEmpty(userParams.Gender))
            {
                userParams.Gender = loggedInUser.Gender == "male" ? "female" : "male";
            }

            var users = await _repo.GetUsers(userParams);

            // to map in dto
            var mappedUsers = _mapper.Map<IEnumerable<UserDto>>(users);

            // now we need to set the headers
            // and we just created this extension method to add pagination 
            Response.AddPagination(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(mappedUsers);
        }

        // get specific user
        [HttpGet("{id}", Name = "GetUser")]
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

        // update user
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDto model)
        {
            // we need to match the id in route with the id of token i.e the logged in user id
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                // if id doesn't match then it will be unauthorized
                return Unauthorized();
            }

            // else now first we get the user
            var user = await _repo.GetUser(id);

            // now map the updated values
            // 1st arg is source 2nd is destination
            _mapper.Map(model, user);

            // no we check the changes
            if (await _repo.SaveAll())
            {
                // if changes > 0
                // we simply return no content
                // we show pop up on front end that profile has been updated
                return NoContent(); 
            }
            // if not then we'll throw the exception
            throw new Exception($"Unable to update the user: {id}");
        }

        // to like other user
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id, int recipientId)
        {
            // first authorize the user
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                // if id doesn't match then it will be unauthorized
                return Unauthorized();
            }

            // now get the like
            var like = await _repo.GetLike(id, recipientId);

            // if like is not null then the loggedin user has already liked user with recipientId
            if (like != null)
                return BadRequest("You have already liked this user");

            // now check if the recipient user exist or not
            if (await _repo.GetUser(recipientId) == null)
                return NotFound("The user does not exist");

            // now the recipient exist, get like is null so we can create new like
            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            Response.AddLikeHeader();

            // now add this like
            //  it is not async, it will only store in memory and not add in db
            _repo.Add<Like>(like);

            // to save in db
            if (await _repo.SaveAll())
                return Ok("You have liked this user!");

            // else
            return BadRequest("Failed to like this user...");
        }



    }
}