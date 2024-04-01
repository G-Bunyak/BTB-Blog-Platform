#region Imports
using BlogPlatform.Interfaces.Services;
using BlogPlatform.Models.AuthModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#endregion

namespace BlogPlatform.Controllers
{
    [ApiController]
    [AllowAnonymous]
    public class AuthController : Controller
    {
        #region Properties
        private readonly IAuthService _authService;
        #endregion

        #region Constructors
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        #endregion

        #region Controllers
        [HttpPost]
        [Route("api/auth")]
        public async Task<AuthResponseModel> GetTokenAsync([FromBody] AuthRequestModel requestModel)
        {
            return await _authService.GetTokenAsync(requestModel);
        }

        [HttpPost]
        [Route("api/register")]
        public async Task<AuthResponseModel> CreateNewUserAsync([FromBody] AuthRequestModel requestModel)
        {
            return await _authService.CreateNewUserAsync(requestModel);
        }
        #endregion
    }
}
