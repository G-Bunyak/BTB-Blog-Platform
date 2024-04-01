#region Imports
using BlogPlatform.Interfaces.DatabaseRepositories;
using BlogPlatform.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BlogPlatform.Helpers.DatabaseRepositories
{
    public class UsersRepository : RepositoryBase<UserModel>, IUsersRepository
    {
        #region Properties
        private readonly ApplicationContext context;
        #endregion

        #region Constructors
        public UsersRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
        #endregion

        #region Public Methods
        public async Task<UserModel?> GetUserByLoginAsync(string login)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            return user;
        }
        #endregion
    }
}
