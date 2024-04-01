using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPlatform.Models.DatabaseModels
{
    [Table("users")]
    public class UserModel
    {
        [Column("u_id")]
        public int Id { get; set; }
        [Column("u_login")]
        public string Login { get; set; } = string.Empty;
        [Column("u_password")]
        public string Password { get; set; } = string.Empty;
    }
}
