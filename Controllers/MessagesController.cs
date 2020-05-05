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
            // to set and send the full sender object
            var sender = await _repo.GetUser(userId);
            // check if user is authorized
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
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

            
            if (await _repo.SaveAll())
            {
                // now save this
                // we do want to return messagedto rather than message which contains the sender
                // which eventually has passwordhash and salt etc
                // so we map back to model from our models

                // we place this line inside if cz we are sending complete object of sender as well
                // and with correct sender id
                // it was originally placed just above the if statement
                var messageToReturn = _mapper.Map<MessageToReturn>(message);

                return Created($"api/user/{userId}/messages/{message.Id}", messageToReturn);
            }
                

            // else
            throw new Exception("Creating message failed on save...");

        }


        // to delete a msg
        // we are not using httpdelete here

        [HttpPost("{id}")]
        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            // user authorization
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // get a message that matches the id
            var message = await _repo.GetMessage(id);

            // now check if sender deleted a msg or not
            if (message.SenderId == userId)
                message.SenderDelted = true;

            // now check if recipient deleted a msg or not
            if (message.RecipientId == userId)
                message.RecipientDeleted = true;

            // now check if both deletes the msg then delete that msg
            if (message.SenderDelted == true && message.RecipientDeleted == true)
                _repo.Delete(message);

            // now save all
            if (await _repo.SaveAll())
                return NoContent();

            // else
            throw new Exception("Error deleting a msg...");
        }

        // to set the mark as read to now
        [HttpPost("{id}/read")]
        public async Task<IActionResult> MarkMessageAsRead(int userId, int id /*message id*/)
        {
            // user authorization
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            // get a message
            var message = await _repo.GetMessage(id);

            // check if recipient ids matches
            if (message.RecipientId != userId)
                return Unauthorized();

            // set the markas read prop
            message.IsRead = true;
            message.DateRead = DateTime.Now;

            // save all
            await _repo.SaveAll();

            return NoContent();
        }
    }
}