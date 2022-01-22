using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using TweetBook2.Contracts.V1;
using TweetBook2.Contracts.V1.Requests;
using TweetBook2.Contracts.V1.Responses;
using TweetBook2.Services;

namespace TweetBook2.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identity)
        {
            _identityService = identity;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized(new AuthenFailedResponse {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var registrationResult = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!registrationResult.Success)
            {
                return Unauthorized(new AuthenFailedResponse
                {
                    Errors = registrationResult.Errors
                });
            }
            return Ok(new AuthenSuccessResponse { Token = registrationResult.Token});
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Unauthorized(new AuthenFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }

            var registrationResult = await _identityService.Login(request.Email, request.Password);

            if (!registrationResult.Success)
            {
                return Unauthorized(new AuthenFailedResponse
                {
                    Errors = registrationResult.Errors
                });
            }
            return Ok(new AuthenSuccessResponse { Token = registrationResult.Token });
        }
    }
}