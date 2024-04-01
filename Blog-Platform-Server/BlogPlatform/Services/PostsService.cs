#region Imports
using BlogPlatform.Interfaces.Helpers;
using BlogPlatform.Interfaces.Services;
using BlogPlatform.Models;
using BlogPlatform.Models.CommentsModels;
using BlogPlatform.Models.DatabaseModels;
using BlogPlatform.Models.PostsModels;
using Microsoft.EntityFrameworkCore;
using System.Net;
#endregion

namespace BlogPlatform.Services
{
    public class PostsService : IPostsService
    {
        #region Properties
        private readonly IDatabaseHelper _databaseHelper;
        #endregion

        #region Constructor
        public PostsService(IDatabaseHelper databaseHelper) 
        {
            _databaseHelper = databaseHelper;
        }
        #endregion

        #region Public methods
        public async Task<CreatePostResponseModel> CreatePostAsync(PostRequestModel requestModel) 
        {
            var responseModel = new CreatePostResponseModel();

            ValidatePostData(requestModel, responseModel);
            if (!responseModel.IsSuccess)
            {
                return responseModel;
            }

            var post = new PostModel() 
            {
                Title = requestModel.Title,
                Content = requestModel.Content,
                AuthorNickname = requestModel.AuthorNickname
            };

            try
            {
                var result = await _databaseHelper.PostsRepository.InsertAsync(post);
                responseModel.Post = result;
                if (result == null)
                {
                    responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                    return responseModel;
                }
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                return responseModel;
            }

            return responseModel;
        }

        public async Task<GetAllPostsResponseModel> GetAllPostsAsync()
        {
            var responseModel = new GetAllPostsResponseModel();

            try
            {
                responseModel.Posts = await _databaseHelper.PostsRepository.GetAllItemsAsync();
                return responseModel;
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database fetching error", HttpStatusCode.InternalServerError);
                return responseModel;
            }
        }

        public async Task<GetPostDetailsModel> GetPostDetailsAsync(string postIdString)
        {
            var responseModel = new GetPostDetailsModel();

            if (!int.TryParse(postIdString, out var postId)) 
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            try
            {
                var postData = await _databaseHelper.PostsRepository.GetPostByIdAsync(postId);
                if (postData == null)
                {
                    responseModel.AddErrorWithStatusCode("Post with this id does not exists", HttpStatusCode.NotFound);
                    return responseModel;
                }

                var comments = await _databaseHelper.CommentsRepository.GetPostCommentsByPostIdAsync(postId);

                responseModel.Post = postData;
                responseModel.Comments = comments;
                return responseModel;
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database fetching error", HttpStatusCode.InternalServerError);
                return responseModel;
            }
        }

        public async Task<BaseResponseModel> UpdatePostAsync(PostRequestModel requestModel, string postIdString)
        {
            var responseModel = new BaseResponseModel();

            if (!int.TryParse(postIdString, out var postId))
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            ValidatePostData(requestModel, responseModel);
            if (!responseModel.IsSuccess)
            {
                return responseModel;
            }

            var post = new PostModel()
            {
                Id = postId,
                Title = requestModel.Title,
                Content = requestModel.Content,
                AuthorNickname = requestModel.AuthorNickname
            };

            try
            {
                await _databaseHelper.PostsRepository.UpdateAsync(post);
            }
            catch (DbUpdateConcurrencyException e) 
            {
                responseModel.AddErrorWithStatusCode("Post with this id does not exists", HttpStatusCode.NotFound);
                return responseModel;
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                return responseModel;
            }

            return responseModel;
        }

        public async Task<BaseResponseModel> DeletePostAsync(string postIdString)
        {
            var responseModel = new BaseResponseModel();

            if (!int.TryParse(postIdString, out var postId))
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            try
            {
                var result = await _databaseHelper.PostsRepository.RemovePostByIdAsync(postId);
                if (result == 0)
                {
                    responseModel.AddErrorWithStatusCode("Post with this id does not exists", HttpStatusCode.NotFound);
                    return responseModel;
                }
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database fetching error", HttpStatusCode.InternalServerError);
                return responseModel;
            }

            return responseModel;
        }

        public async Task<CreateCommentResponseModel> CreatePostCommentAsync(CommentRequestModel requestModel, string postIdString)
        {
            var responseModel = new CreateCommentResponseModel();

            if (!int.TryParse(postIdString, out var postId))
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            ValidatePostCommentData(requestModel, responseModel);
            if (!responseModel.IsSuccess)
            {
                return responseModel;
            }

            var comment = new CommentModel()
            {
                PostId = postId,
                Content = requestModel.Content,
                AuthorNickname = requestModel.AuthorNickname,
                Post = null
            };

            try
            {
                var post = await _databaseHelper.PostsRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                    return responseModel;
                }

                comment.Post = post;

                var result = await _databaseHelper.CommentsRepository.InsertAsync(comment);            
                if (result == null)
                {
                    responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                    return responseModel;
                }

                responseModel.Comment = result;
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                return responseModel;
            }

            return responseModel;
        }

        public async Task<BaseResponseModel> UpdatePostCommentAsync(CommentRequestModel requestModel, string postIdString, string commentIdString)
        {
            var responseModel = new BaseResponseModel();

            if (!int.TryParse(postIdString, out var postId))
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            if (!int.TryParse(commentIdString, out var commentId))
            {
                responseModel.AddErrorWithStatusCode("Invalid comment id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            ValidatePostCommentData(requestModel, responseModel);
            if (!responseModel.IsSuccess)
            {
                return responseModel;
            }

            var comment = new CommentModel()
            {
                Id = commentId,
                PostId = postId,
                Content = requestModel.Content,
                AuthorNickname = requestModel.AuthorNickname
            };

            try
            {
                var post = await _databaseHelper.PostsRepository.GetPostByIdAsync(postId);
                if (post == null)
                {
                    responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                    return responseModel;
                }

                comment.Post = post;

                await _databaseHelper.CommentsRepository.UpdateAsync(comment);
            }
            catch (DbUpdateConcurrencyException e)
            {
                responseModel.AddErrorWithStatusCode("Post with this id does not exists", HttpStatusCode.NotFound);
                return responseModel;
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database saving error", HttpStatusCode.InternalServerError);
                return responseModel;
            }

            return responseModel;
        }

        public async Task<BaseResponseModel> DeletePostCommentAsync(string postIdString, string commentIdString)
        {
            var responseModel = new BaseResponseModel();

            if (!int.TryParse(postIdString, out var postId))
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            if (!int.TryParse(commentIdString, out var commentId))
            {
                responseModel.AddErrorWithStatusCode("Invalid post id", HttpStatusCode.BadRequest);
                return responseModel;
            }

            try
            {
                var result = await _databaseHelper.CommentsRepository.RemovePostCommentByPostIdAndIdAsync(postId, commentId);
                if (result == 0)
                {
                    responseModel.AddErrorWithStatusCode("Comment with this id does not exists", HttpStatusCode.NotFound);
                    return responseModel;
                }
            }
            catch
            {
                responseModel.AddErrorWithStatusCode("Database fetching error", HttpStatusCode.InternalServerError);
                return responseModel;
            }

            return responseModel;
        }
        #endregion

        #region Private methods
        private void ValidatePostData(PostRequestModel requestModel, BaseResponseModel responseModel) 
        {
            if (string.IsNullOrEmpty(requestModel?.AuthorNickname) || string.IsNullOrEmpty(requestModel?.Title) || string.IsNullOrEmpty(requestModel?.Content))
            {
                responseModel.AddErrorWithStatusCode("Invalid input model", HttpStatusCode.BadRequest);
                return;
            }

            if (requestModel.AuthorNickname.Length > 150)
            {
                responseModel.AddErrorWithStatusCode("Invalid author nickname", HttpStatusCode.BadRequest);
                return;
            }

            if (requestModel.Title.Length > 500)
            {
                responseModel.AddErrorWithStatusCode("Invalid blog title", HttpStatusCode.BadRequest);
                return;
            }

            if (requestModel.Content.Length > 65535)
            {
                responseModel.AddErrorWithStatusCode("Invalid blog content", HttpStatusCode.BadRequest);
                return;
            }
        }

        private void ValidatePostCommentData(CommentRequestModel requestModel, BaseResponseModel responseModel)
        {
            if (string.IsNullOrEmpty(requestModel?.AuthorNickname) || string.IsNullOrEmpty(requestModel?.Content))
            {
                responseModel.AddErrorWithStatusCode("Invalid input model", HttpStatusCode.BadRequest);
                return;
            }

            if (requestModel.AuthorNickname.Length > 150)
            {
                responseModel.AddErrorWithStatusCode("Invalid author nickname", HttpStatusCode.BadRequest);
                return;
            }

            if (requestModel.Content.Length > 65535)
            {
                responseModel.AddErrorWithStatusCode("Invalid blog content", HttpStatusCode.BadRequest);
                return;
            }
        }
        #endregion
    }
}
