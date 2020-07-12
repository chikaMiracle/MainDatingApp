using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MainDatingApp.Data;
using MainDatingApp.Dtos;
using MainDatingApp.Helpers;
using MainDatingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainDatingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/users/{userId}/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }


        [HttpGet("{id}", Name="GetMessage")]

        public async Task<IActionResult> GetMessage(int userId, int id)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //getting message from repo
            var messageFromRepo = _repo.GetMessage(id);

                //to check if we have message from repo
                if(messageFromRepo == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(messageFromRepo);
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
        {
            //check if user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            messageParams.UserId = userId;
            //getting our messages from repo

            var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);
            var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);

            Response.AddPagination(messagesFromRepo.CurrentPage, messagesFromRepo.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

            return Ok(messages);

        }

        [HttpGet("thread/{recipientId}")]

        public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
        {
            //checking if user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _repo.GetMessageThread(userId, recipientId);

            var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messageFromRepo);

            return Ok(messageThread);
        }

        [HttpPost]

        public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
        {

            var sender = await _repo.GetUser(userId);
            //checking if user is authorized
            if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            //setting our messageForCreationDto senderId to UserId

            messageForCreationDto.SenderId = userId;

            //checking if the recipient Id exist in the db

            var recipient = await _repo.GetUser(messageForCreationDto.RecipientId);

            //checking if the recipient id is null

            if (recipient == null)
            {
                return BadRequest("Could not find user");
            }
                //mapping our dto to our messages entity

                var message = _mapper.Map<Message>(messageForCreationDto);

            _repo.Add(message);

           

            if(await _repo.SaveAll())
            {
                var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
                return CreatedAtRoute("GetMessage", new {userId, id = message.Id }, messageToReturn);
            }
            else
            {
                throw new Exception("Creating the message failed on save");
            }

        }

        [HttpPost("{id}")]

        public async Task<IActionResult> DeleteMessage(int id, int userId)
        {
            //checking if user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var messageFromRepo = await _repo.GetMessage(id);

            if(messageFromRepo.SenderId == userId)
            {
                messageFromRepo.SenderDeleted = true;
            }
            if(messageFromRepo.RecipientId == userId)
            {
                messageFromRepo.RecipientDeleted = true;
            }
            if(messageFromRepo.SenderDeleted && messageFromRepo.RecipientDeleted)
            {
                _repo.Delete(messageFromRepo);
            }

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Error deleting the message");
        }

        [HttpPost("{id}/read")]

        public async Task<IActionResult> MarkMessageRead(int id, int userId)
        {
            //checking if user is authorized
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            {
                return Unauthorized();
            }

            var message = await _repo.GetMessage(id);

            if (message.RecipientId != userId)
                return Unauthorized();

            message.IsRead = true;
            message.DateRead = DateTime.Now;

            await _repo.SaveAll();

            return NoContent();

        }
    }
}