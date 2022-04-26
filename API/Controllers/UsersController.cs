using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
   // [Authorize]
   public class UsersController : BaseApiController
    {
        
        private readonly IMapper mapper;
        private readonly IPhotoInterface _photo;
        private readonly IUnitOfWork unitOfwork;

        public UsersController( IMapper mapper, IPhotoInterface _photo, IUnitOfWork unitOfwork)
        {
           
            this.mapper = mapper;
            this._photo = _photo;
            this.unitOfwork = unitOfwork;
        }

        
        // GET: api/<UsersController>
        [HttpGet]
        
        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetUsers([FromQuery]UserParams userParams)
        {
            

            var gender = await unitOfwork.UserRepository.GetUserGender(User.GetUsername());
            userParams.CurrentUsername = User.GetUsername();

            if (string.IsNullOrEmpty(userParams.Gender)) userParams.Gender = gender == "male" ? "female" : "male";
            

            var users = await unitOfwork.UserRepository.GetMembersAsync(userParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

            return Ok(users);
        }

        
        // GET api/<UsersController>/5
        [HttpGet("{username}", Name = "GetUser")]
        
        public async Task<ActionResult<MemberDTO>> GetUser(string username)
        {
            return  await unitOfwork.UserRepository.GetMemberAsync(username);
            
        }

        [HttpPut]
        
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDto)
        {
            
            var user = await unitOfwork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            mapper.Map(memberUpdateDto, user);

            unitOfwork.UserRepository.Update(user);

            if (await unitOfwork.Complete())  return NoContent();

            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await unitOfwork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var result = await _photo.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };


            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await unitOfwork.Complete())
            {
                

                return CreatedAtRoute("GetUser",new {username = user.UserName },
                    mapper.Map<PhotoDTO>(photo));
            }
                


            return BadRequest("Problem with adding photo!");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await unitOfwork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain)
                return BadRequest("This is already main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) 
                    currentMain.IsMain = false;

            photo.IsMain = true;

            if (await unitOfwork.Complete())
                return NoContent();

            return BadRequest("Faild to set main photo!");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await unitOfwork.UserRepository.GetUserByUsernameAsync(User.GetUsername());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null)
                return NotFound();

            if (photo.IsMain)
                return BadRequest("You can't delete main photo!");

            if(photo.PublicId != null)
            {
                var result = await _photo.DeletingPhotoAsync(photo.PublicId);
                if (result.Error != null)
                    return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await unitOfwork.Complete())
                return Ok();

            return BadRequest("Faild to delete photo");
        }
    }
}
