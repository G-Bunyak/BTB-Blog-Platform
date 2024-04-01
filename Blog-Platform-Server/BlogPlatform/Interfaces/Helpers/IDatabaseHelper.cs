using BlogPlatform.Interfaces.DatabaseRepositories;

namespace BlogPlatform.Interfaces.Helpers
{
    public interface IDatabaseHelper
    {
        IUsersRepository Users { get; }
        IPostsRepository PostsRepository { get; }
        ICommentsRepository CommentsRepository { get; }
        Task SaveAsync();
    }
}
