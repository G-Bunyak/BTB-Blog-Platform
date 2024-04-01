#region Imports
using BlogPlatform.Interfaces.Services;
using BlogPlatform.Models;
using BlogPlatform.Models.CommentsModels;
using BlogPlatform.Models.PostsModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
#endregion

namespace BlogPlatform.Controllers
{
    [ApiController]
    public class PostsController : Controller
    {
        #region Properties
        private readonly IPostsService _postsService;
        #endregion

        #region Constructors
        public PostsController(IPostsService postsService)
        {
            _postsService = postsService;
        }
        #endregion

        #region Post controllers
        [HttpPost]
        [Authorize]
        [Route("api/post")]
        public async Task<CreatePostResponseModel> CreatePostAsync([FromBody] PostRequestModel requestModel)
        {
            var response = await _postsService.CreatePostAsync(requestModel);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/post")]
        public async Task<BaseResponseModel> GetAllPostsAsync()
        {
            var response = await _postsService.GetAllPostsAsync();
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/post/{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> GetPostDetailsAsync([FromRoute] string postId)
        {
            var response = await _postsService.GetPostDetailsAsync(postId);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }

        [HttpPut]
        [Authorize]
        [Route("api/post/{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> UpdatePostDetailsAsync([FromBody] PostRequestModel requestModel, [FromRoute] string postId)
        {
            var response = await _postsService.UpdatePostAsync(requestModel, postId);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("api/post/{postId:maxlength(9)}")]
        public async Task<BaseResponseModel> DeletePostAsync([FromRoute] string postId)
        {
            var response = await _postsService.DeletePostAsync(postId);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }
        #endregion

        #region Comment controllers
        [HttpPost]
        [Authorize]
        [Route("api/post/{postId:maxlength(9)}/comment")]
        public async Task<CreateCommentResponseModel> CreatePostCommentAsync([FromBody] CommentRequestModel requestModel, [FromRoute] string postId)
        {
            var response = await _postsService.CreatePostCommentAsync(requestModel, postId);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }

        [HttpPut]
        [Authorize]
        [Route("api/post/{postId:maxlength(9)}/comment/{commentId:maxlength(9)}")]
        public async Task<BaseResponseModel> UpdatePostCommentAsync([FromBody] CommentRequestModel requestModel, [FromRoute] string postId, [FromRoute] string commentId)
        {
            var response = await _postsService.UpdatePostCommentAsync(requestModel, postId, commentId);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }

        [HttpDelete]
        [Authorize]
        [Route("api/post/{postId:maxlength(9)}/comment/{commentId:maxlength(9)}")]
        public async Task<BaseResponseModel> DeletePostCommentAsync([FromRoute] string postId, [FromRoute] string commentId)
        {
            var response = await _postsService.DeletePostCommentAsync(postId, commentId);
            Response.StatusCode = (int)response.HttpStatusCode;
            return response;
        }
        #endregion
    }
}
