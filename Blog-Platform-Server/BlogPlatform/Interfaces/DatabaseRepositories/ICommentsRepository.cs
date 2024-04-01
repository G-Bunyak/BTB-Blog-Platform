using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Interfaces.DatabaseRepositories
{
    public interface ICommentsRepository : IRepositoryBase<CommentModel>
    {
        Task<List<CommentModel>> GetPostCommentsByPostIdAsync(int postId);
        Task<int> RemovePostCommentByPostIdAndIdAsync(int postId, int id);
    }
}
