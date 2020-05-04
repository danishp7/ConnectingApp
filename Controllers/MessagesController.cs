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

namespace ConnectingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/user/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        // inject the services
        public MessagesController(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // to get the message
        // id is the recipientid thats why it is passed in route
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(int userId/*from url*/, int id /*msg id*/)
        {
            // first check if user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // now get the msg from repo
            var message = await _repo.GetMessage(id);
            // check if it is null or not
            if (message == null)
                return NotFound("Message not found!");

            return Ok(message);
        }

        // to get all the messages
        [HttpGet]
        public async Task<IActionResult> GetMessagesForUser(int userId/*from url*/, [FromQuery]MessageParams messageParams)
        {
            // authorize user
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // set the userid to params.userid
            messageParams.UserId = userId;

            // now get all the messages
            var messagesFromRepo = await _repo.GetMessagesForUsers(messageParams);

            // now map them to message
            var messages = _mapper.Map<IEnumerable<MessageToReturn>>(messagesFromRepo);

            // now add the pagination in header
            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize,
                messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            // noe return
            return Ok(messages);
        }

        // get the conversation between 2 users
        // we add thread as to distinguish between 2 get methods
        [HttpGet("thread/{recipientId}")]
        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            // if user authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // get the conversation from repo
            var messagesFromRepo = await _repo.GetMessageThread(userId, recipientId);

            // map them to message dto
            var conversation = _mapper.Map<IEnumerable<MessageToReturn>>(messagesFromRepo);

            return Ok(conversation);
        }


        // post message
        [HttpPost]
        public async Task<IActionResult> CreateMessage(int userId, MessageCreatingDto messageCreatingModel)
        {
            // check if user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // set the userId to model's senderid
            messageCreatingModel.SenderId = userId;

            // now get the user
            // this will be the user, to him/her we'll send the msg
            // recipientid will be passed in body of request
            var recipient = await _repo.GetUser(messageCreatingModel.RecipientId);

            // check if recipient exists or not
            if (recipient == null)
                return BadRequest("Recipient not found!");

            // now map the msg to model message
            var message = _mapper.Map<Message>(messageCreatingModel);

            // now add this msg in entity
            _repo.Add(message);

            // now save this
            // we do want to return messagedto rather than message which contains the sender
            // which eventually has passwordhash and salt etc
            // so we map back to model from our models
            var messageToReturn = _mapper.Map<MessageCreatingDto>(message);

            if (await _repo.SaveAll())
                return Created($"api/user/{userId}/messages/{message.Id}", messageToReturn);

            // else
            throw new Exception("Creating message failed on save...");

        }
    }
}