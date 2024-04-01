using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Models.CommentsModels
{
    public class CreateCommentResponseModel : BaseResponseModel
    {
        public CommentModel? Comment { get; set; } = null;
    }
}
