namespace BlogPlatform.Models.AuthModels
{
    public class AuthResponseModel : BaseResponseModel
    {
        public string AccessToken { get; set; } = string.Empty;
    }
}
