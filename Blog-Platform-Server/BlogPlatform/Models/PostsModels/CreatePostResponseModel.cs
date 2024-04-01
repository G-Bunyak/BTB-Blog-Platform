using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Models.PostsModels
{
    public class CreatePostResponseModel : BaseResponseModel
    {
        public PostModel? Post { get; set; } = null;
    }
}
