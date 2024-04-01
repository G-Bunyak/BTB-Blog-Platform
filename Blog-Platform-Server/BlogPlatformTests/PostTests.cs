#region Imports
using BlogPlatform.Interfaces.Helpers;
using BlogPlatform.Models.DatabaseModels;
using BlogPlatform.Models.PostsModels;
using BlogPlatform.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Net;
#endregion

namespace BlogPlatformTests
{
    public class PostTests
    {
        #region Negative Tests
        [Fact]
        public async Task CreateInvalidPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.InsertAsync(new PostModel() { AuthorNickname = "Test", Title = "Test", Content = "Test" }, true))
                .ReturnsAsync(new PostModel() { Id = 1, AuthorNickname = "Test", Title = "Test", Content = "Test", Comments = new List<CommentModel>() });
            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.CreatePostAsync(new PostRequestModel() { AuthorNickname = "", Title = "", Content = "" });

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateInvalidPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.UpdateAsync(new PostModel() { Id = 5, AuthorNickname = "Test1", Title = "Test1", Content = "Test1" }, true, true))
                .ReturnsAsync(new PostModel() { Id = 5, AuthorNickname = "Test1", Title = "Test1", Content = "Test1", Comments = new List<CommentModel>() });

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.UpdatePostAsync(new PostRequestModel() { AuthorNickname = "", Title = "", Content = "" }, "5");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.BadRequest, result.HttpStatusCode);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateNotExistingPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.UpdateAsync(new PostModel() { Id = 10, AuthorNickname = "Test1", Title = "Test1", Content = "Test1" }, true, true))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.UpdatePostAsync(new PostRequestModel() { AuthorNickname = "Test1", Title = "Test1", Content = "Test1" }, "10");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteInvalidPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.RemovePostByIdAsync(10))
                .ReturnsAsync(0);

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.DeletePostAsync("10");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task TestGetNonExistingPostDetailsAsync()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.GetPostByIdAsync(10))
                .ReturnsAsync((PostModel?)null);
            mockDependency.Setup(
                d => d.CommentsRepository.GetPostCommentsByPostIdAsync(5))
                .ReturnsAsync(new List<CommentModel>());

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.GetPostDetailsAsync("10");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.NotFound, result.HttpStatusCode);
            Assert.False(result.IsSuccess);
            Assert.Null(result.Post);
        }
        #endregion

        #region Positive Tests
        [Fact]
        public async Task CreateValidPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.InsertAsync(new PostModel() { Id = 0, AuthorNickname = "Test", Title = "Test", Content = "Test" }, true))
                .ReturnsAsync(new PostModel() { Id = 5, AuthorNickname = "Test", Title = "Test", Content = "Test", Comments = new List<CommentModel>() });

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.CreatePostAsync(new PostRequestModel() { AuthorNickname = "Test", Title = "Test", Content = "Test" });

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Post);
            Assert.Equal(5, result?.Post?.Id);
        }

        [Fact]
        public async Task UpdateValidPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.UpdateAsync(new PostModel() { Id = 5, AuthorNickname = "Test1", Title = "Test1", Content = "Test1" }, true, true))
                .ReturnsAsync(new PostModel() { Id = 5, AuthorNickname = "Test1", Title = "Test1", Content = "Test1", Comments = new List<CommentModel>() });

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.UpdatePostAsync(new PostRequestModel() { AuthorNickname = "Test1", Title = "Test1", Content = "Test1" }, "5");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteValidPostTest()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.RemovePostByIdAsync(5))
                .ReturnsAsync(1);

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.DeletePostAsync("5");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task TestGetPostsAsync()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.GetAllItemsAsync())
                .ReturnsAsync(new List<PostModel>() 
                {
                    { new PostModel() { Id = 1, AuthorNickname = "Test1", Title = "Test1", Content = "Test1", Comments = new List<CommentModel>() } },
                    { new PostModel() { Id = 2, AuthorNickname = "Test2", Title = "Test2", Content = "Test2", Comments = new List<CommentModel>() } },
                    { new PostModel() { Id = 3, AuthorNickname = "Test3", Title = "Test3", Content = "Test3", Comments = new List<CommentModel>() } }
                });

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.GetAllPostsAsync();

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Posts);
            Assert.Equal(3, result.Posts.Count);
            Assert.Equal(2, result.Posts[1].Id);
        }

        [Fact]
        public async Task TestGetPostDetailsAsync()
        {
            // Init
            var mockDependency = new Mock<IDatabaseHelper>();
            mockDependency.Setup(
                d => d.PostsRepository.GetPostByIdAsync(5))
                .ReturnsAsync(new PostModel() { Id = 5, AuthorNickname = "Test1", Title = "Test1", Content = "Test1" });
            mockDependency.Setup(
                d => d.CommentsRepository.GetPostCommentsByPostIdAsync(5))
                .ReturnsAsync(new List<CommentModel>());

            var postsService = new PostsService(mockDependency.Object);

            //Exec
            var result = await postsService.GetPostDetailsAsync("5");

            //Validate
            Assert.NotNull(result);
            Assert.Equal(HttpStatusCode.OK, result.HttpStatusCode);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Post);
            Assert.Equal(5, result?.Post?.Id);
        }
        #endregion
    }
}
