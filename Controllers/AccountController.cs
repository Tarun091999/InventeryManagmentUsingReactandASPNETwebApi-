using Inventory.Dal.Repository;
using Inventory.Modals.ModalDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        IUserRepo _userRepo;
        public AccountController( IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }


        [HttpGet]
        public string Login()
        {
            return "Welcome to Inventory System";
        }


        [HttpPost("login")]
        public async Task <IActionResult> Login( LogInDTO reqUser)
        {
            if ( await _userRepo.Login(reqUser) )
            {

                var token = await _userRepo.CreateJWT(reqUser.Email);
                //HttpContext.Session.SetString("token",token.Token.ToString());
                return Ok(token);
            }

            return Ok("Error Occured !!!");

        }


        //[HttpGet]
        //public string Signup()
        //{
        //    return "Signup";
        //}

        [HttpPost("Create")]
        public async Task<IActionResult>Signup(UserSignUpDTO reqUser)
        {
            var otp = await _userRepo.AddUser(reqUser);        
            return Ok(otp);    
        }

        [HttpPost("verified")]
        public async Task<IActionResult> Verified(UserSignUpDTO reqUser)
        {
            var result = await _userRepo.VerifiedUser(reqUser);
            
            return Ok(result);
        }



    }
}
