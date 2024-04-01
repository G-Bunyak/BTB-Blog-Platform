using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Models.PostsModels
{
    public class GetAllPostsResponseModel : BaseResponseModel
    {
        public List<PostModel> Posts { get; set; } = new List<PostModel>();
    }
}
