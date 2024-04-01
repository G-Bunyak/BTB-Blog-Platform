namespace BlogPlatform.Models.CommentsModels
{
    public class CommentRequestModel
    {
        public string AuthorNickname { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
