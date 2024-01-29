using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Security.Core;
using Security.Core.SecurityEntites;
using Securtiy_api.Dtos;
using Securtiy_api.Errors;

namespace Securtiy_api.Controllers
{
    //step6
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ItokenServices _tokenServices;

        public SecurityController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ItokenServices tokenServices)
        {
            _userManager = userManager;
            _signInManager = signInManager;
           _tokenServices = tokenServices;
        }
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto Model)
        {
            #region account


            #endregion
            var user = await _userManager.FindByEmailAsync(Model.Email);
            if (user == null) return Ok(new ApiResponse(401));
            var result = await _signInManager.CheckPasswordSignInAsync(user, Model.Password, false);
            if (!result.Succeeded) return Ok(new ApiResponse(401));
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _tokenServices.CreateTokenAsync(user,_userManager),
                message= "success"

            });


        }
        [HttpPost("register")]
    
        
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {

            //if ( checkemailexists(model.Email).Result.Value)
            //{
            //    return BadRequest(new ApiValdtionErrorResponse() { Errors=new string[] {"This email is already in user"} });
            //}
            #region MyRegion
            //var user = new AppUser()
            //{
            //    DisplayName = model.DisplayName,
            //    Email = model.EmailAdress,
            //    UserName = model.EmailAdress.Split('@')[0],
            //    PhoneNumber = model.PhoneNumber,
            //};
            //var result = await _userManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            //return Ok(new UserDto()
            //{
            //    DisplayName = user.DisplayName,
            //    Email = user.Email,
            //    Token = await _service.CreateTokenAsync(user, _userManager)
            //});
            #endregion
            //create user in database 
            var user=new AppUser()
            {
             
                DisplayName= model.DisplayName,
                Email= model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber,
                
            };
            var result = await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded) return Ok(new ApiResponse(200));
            else
            {
                return Ok(new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = await _tokenServices.CreateTokenAsync(user, _userManager),
                    message = "success"
                });
            }
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> checkemailexists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }

}