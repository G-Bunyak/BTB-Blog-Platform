using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Models.PostsModels
{
    public class GetPostDetailsModel : BaseResponseModel
    {
        public PostModel? Post { get; set; } = null;
        public List<CommentModel> Comments { get; set; } = new List<CommentModel>();
    }
}
