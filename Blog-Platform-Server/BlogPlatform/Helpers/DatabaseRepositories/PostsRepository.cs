#region Imports
using BlogPlatform.Interfaces.DatabaseRepositories;
using BlogPlatform.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BlogPlatform.Helpers.DatabaseRepositories
{
    public class PostsRepository : RepositoryBase<PostModel>, IPostsRepository
    {
        #region Properties
        private readonly ApplicationContext context;
        #endregion

        #region Constructors
        public PostsRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
        #endregion

        #region Public methods
        public async Task<PostModel?> GetPostByIdAsync(int id)
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
            return post;
        }

        public async Task<int> RemovePostByIdAsync(int id)
        {
            return await context.Posts.Where(p => p.Id == id).ExecuteDeleteAsync();
        }
        #endregion
    }
}
