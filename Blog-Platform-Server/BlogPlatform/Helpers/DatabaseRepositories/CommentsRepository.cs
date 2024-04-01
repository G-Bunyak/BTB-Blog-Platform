#region Imports
using BlogPlatform.Interfaces.DatabaseRepositories;
using BlogPlatform.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BlogPlatform.Helpers.DatabaseRepositories
{
    public class CommentsRepository : RepositoryBase<CommentModel>, ICommentsRepository
    {
        #region Properties
        private readonly ApplicationContext context;
        #endregion

        #region Constructors
        public CommentsRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
        #endregion

        #region Public methods
        public async Task<List<CommentModel>> GetPostCommentsByPostIdAsync(int postId)
        {
            var comments = await context.Comments.Where(c => c.PostId == postId).ToListAsync();
            return comments;
        }

        public async Task<int> RemovePostCommentByPostIdAndIdAsync(int postId, int id)
        {
            return await context.Comments.Where(c => c.Id == id && c.PostId == postId).ExecuteDeleteAsync();
        }
        #endregion
    }
}
