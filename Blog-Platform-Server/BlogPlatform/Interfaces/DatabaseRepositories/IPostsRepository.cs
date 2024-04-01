using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Interfaces.DatabaseRepositories
{
    public interface IPostsRepository : IRepositoryBase<PostModel>
    {
        Task<PostModel?> GetPostByIdAsync(int id);
        Task<int> RemovePostByIdAsync(int id);
    }
}
