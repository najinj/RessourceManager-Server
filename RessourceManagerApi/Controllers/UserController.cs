using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.ViewModels.Authentication;
using RessourceManagerApi.Services;

namespace RessourceManagerApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly EmailSenderService _emailService;
        public UserController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, EmailSenderService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }


        [HttpGet]
        public List<UserDataResponse> Get() => _userManager.Users.Select(user => new UserDataResponse
        {
            Name = user.Name,
            Activated = user.Activated,
            Email = user.Email,
            LastName = user.LastName
        }).ToList();


        [HttpGet]
        public async Task<ActionResult> ActivateOrDeactivateUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();
            user.Activated = !user.Activated;
            var result = await _userManager.UpdateAsync(user);
            if (user.Activated)
            {
                if (result.Succeeded)
                {
                    try
                    {
                        await _emailService.SendEmailAsync(email, "Your Account Has Been Activated");
                    }

                    catch (Exception ex)
                    {
                        // TODO: handle exception
                        return BadRequest(ex.Message); // return activated but email not sent // do a quee for later ?
                    }
                }
                else
                {
                    ModelState.AddModelError("Activated", "Couldn't Update User");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }
                
            }
            return Ok(new UserDataResponse
            {
                Name = user.Name,
                Activated = user.Activated,
                Email = user.Email,
                LastName = user.LastName
            });
        }





    }
}