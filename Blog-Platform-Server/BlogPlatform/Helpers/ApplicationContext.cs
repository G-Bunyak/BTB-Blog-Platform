#region Imports
using BlogPlatform.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;
#endregion

namespace BlogPlatform.Helpers
{
    public class ApplicationContext : DbContext
    {
        #region Properties
        public DbSet<UserModel> Users { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public bool IsDisposed { get; set; }
        #endregion

        #region Constructors
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(1800);
        }
        #endregion

        #region Override methods
        public override void Dispose()
        {
            IsDisposed = true;
            base.Dispose();
        }
        #endregion
    }
}
