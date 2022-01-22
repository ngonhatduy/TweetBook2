using Microsoft.AspNetCore.Mvc;
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
            var registrationResult = await _identityService.RegisterAsync(request.Email, request.Password);

            if (!registrationResult.Success)
            {
                return BadRequest(new AuthenFailedResponse
                {
                    Errors = registrationResult.Errors
                });
            }
            return Ok(new AuthenSuccessResponse { Token = registrationResult.Token});
        }
    }
}