﻿using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController : BaseApiController
    {
        private readonly DataContext context;

        public BuggyController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet("auth")]
        [Authorize]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        
        public ActionResult<AppUser> GetNotFound()
        {
            var ting = context.Users.Find(-1);

            if (ting == null)
            {
                return NotFound();
            }

            return Ok(ting);
        }

        [HttpGet("server-error")]
        
        public ActionResult<string> GetServerError()
        {
            
                var thing = context.Users.Find(-1);

                var thingToReturn = thing.ToString();

                return thingToReturn;
            
           
        }

        [HttpGet("bad-request")]
        
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest();
        }


    }
}
