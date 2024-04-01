namespace BlogPlatform.Models.PostsModels
{
    public class PostRequestModel
    {
        public string AuthorNickname { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}
