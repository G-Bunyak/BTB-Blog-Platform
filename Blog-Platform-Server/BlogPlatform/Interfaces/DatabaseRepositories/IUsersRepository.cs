using BlogPlatform.Models.DatabaseModels;

namespace BlogPlatform.Interfaces.DatabaseRepositories
{
    public interface IUsersRepository : IRepositoryBase<UserModel>
    {
        Task<UserModel?> GetUserByLoginAsync(string login);
    }
}
