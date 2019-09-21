using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using test_mongo_auth.Models;
using test_mongo_auth.Models.Requests;
using test_mongo_auth.Models.Responses;
using test_mongo_auth.Helpers;
using System.Collections.Generic;
using System.Security.Claims;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
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
                City = user.City,
                Email = user.Email
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


        

    }
}