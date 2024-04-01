using BlogPlatform.Models;
using BlogPlatform.Models.CommentsModels;
using BlogPlatform.Models.PostsModels;

namespace BlogPlatform.Interfaces.Services
{
    public interface IPostsService
    {
        Task<CreatePostResponseModel> CreatePostAsync(PostRequestModel requestModel);
        Task<GetAllPostsResponseModel> GetAllPostsAsync();
        Task<GetPostDetailsModel> GetPostDetailsAsync(string postIdString);
        Task<BaseResponseModel> UpdatePostAsync(PostRequestModel requestModel, string postIdString);
        Task<BaseResponseModel> DeletePostAsync(string postIdString);
        Task<CreateCommentResponseModel> CreatePostCommentAsync(CommentRequestModel requestModel, string postIdString);
        Task<BaseResponseModel> UpdatePostCommentAsync(CommentRequestModel requestModel, string postIdString, string commentIdString);
        Task<BaseResponseModel> DeletePostCommentAsync(string postIdString, string commentIdString);
    }
}
