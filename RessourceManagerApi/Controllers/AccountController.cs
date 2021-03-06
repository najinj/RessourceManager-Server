﻿using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RessourceManagerApi.Helpers;
using Microsoft.AspNetCore.Cors;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.ViewModels.Authentication;
using RessourceManager.Core.Services.Interfaces;

namespace RessourceManagerApi.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService _emailService;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<ApplicationRole> roleManager,
            IEmailSenderService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<ActionResult> UserData()
        {
            var user = await _userManager.GetUserAsync(User);
            var userData = new UserDataResponse
            {
                Name = user.UserName,
                LastName = user.LastName,
                Email = user.Email,
                Activated = user.Activated
            };
            return Ok(userData);
        }


        // POST api/Account/register
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody]RegisterEntity model)
        {
            if (ModelState.IsValid)
            {
                var userRole = await _roleManager.FindByNameAsync("User");
                if (userRole == null)
                {
                    userRole = new ApplicationRole("User");
                    await _roleManager.CreateAsync(userRole);
                }

                var user = new ApplicationUser { Name = model.Name, LastName = model.LastName, City = model.City, UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                await _userManager.AddToRoleAsync(user, userRole.Name);

                if (result.Succeeded)
                {
                    
                    await _signInManager.SignInAsync(user, false);
                    var token = await AuthenticationHelper.GenerateJwtToken(model.Email, user, _configuration, _userManager);

                    var rootData = new SignUpResponse(token, user.UserName, user.Email);
                    return Created("api/authentication/register", rootData);
                }
                return Ok(string.Join(",", result.Errors?.Select(error => error.Description)));
            }
            
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        // api/user/login
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody]LoginEntity model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    if (appUser.Activated)
                    {
                        var token = await AuthenticationHelper.GenerateJwtToken(model.Email, appUser, _configuration, _userManager);

                        var rootData = new LoginResponse(token, appUser.UserName, appUser.Email);
                        return Ok(rootData);
                    }
                    ModelState.TryAddModelError("Activated", "Account is not Activated");
                    return StatusCode((int)HttpStatusCode.Unauthorized, new ValidationProblemDetails(ModelState));
                }
                return StatusCode((int)HttpStatusCode.Unauthorized, "Bad Credentials");
            }
            return BadRequest(new ValidationProblemDetails(ModelState));
        }

        [HttpGet]
        public async Task<ActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if(user != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _emailService.SendResetPasswordEmailAsync(email, resetToken);
                return Ok(resetToken);
            }
            return null;
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user,model.ResetToken, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                return BadRequest(result.Errors);

            }
            return BadRequest("User not found");
        }




    }
}