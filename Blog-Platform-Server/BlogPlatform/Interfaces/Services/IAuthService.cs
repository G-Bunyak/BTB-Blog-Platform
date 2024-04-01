using BlogPlatform.Models.AuthModels;

namespace BlogPlatform.Interfaces.Services
{
    public interface IAuthService
    {
        Task<AuthResponseModel> GetTokenAsync(AuthRequestModel requestModel);
        Task<AuthResponseModel> CreateNewUserAsync(AuthRequestModel requestModel);
    }
}
