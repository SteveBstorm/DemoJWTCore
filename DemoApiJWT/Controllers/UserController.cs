using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApiJWT.Models;
using DemoApiJWT.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoApiJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok("PIGNOUF !!!");
        }

        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult Auth([FromBody]LoginInfo model)
        {
            User user = _userService.Authenticate(model.UserName, model.Password);

            if (user == null)
            {
                return BadRequest(new { message = "Nom d'utilisateur ou Mot de passe invalide" });
            }

            return Ok(user);
        }

        [HttpGet("list")]
        public IActionResult GetAll()
        {
            return Ok(_userService.GetAll());
        }
    }
}