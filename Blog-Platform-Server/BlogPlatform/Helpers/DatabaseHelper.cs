#region Imports
using BlogPlatform.Helpers.DatabaseRepositories;
using BlogPlatform.Interfaces.DatabaseRepositories;
using BlogPlatform.Interfaces.Helpers;
#endregion

namespace BlogPlatform.Helpers
{
    public class DatabaseHelper : IDisposable, IDatabaseHelper
    {
        #region Properties
        private ApplicationContext context;
        private IPostsRepository postsRepository;
        private ICommentsRepository commentsRepository;
        private IUsersRepository usersRepository;
        private bool disposed = false;

        public IPostsRepository PostsRepository => postsRepository = postsRepository ?? new PostsRepository(context);
        public ICommentsRepository CommentsRepository => commentsRepository = commentsRepository ?? new CommentsRepository(context);
        public IUsersRepository Users => usersRepository = usersRepository ?? new UsersRepository(context);
        #endregion

        #region Constructors
        public DatabaseHelper(ApplicationContext context)
        {
            this.context = context;
        }
        #endregion

        #region Public Methods
        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Private Methods
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }
        #endregion
    }
}
