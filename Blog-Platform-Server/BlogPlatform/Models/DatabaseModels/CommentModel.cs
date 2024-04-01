using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogPlatform.Models.DatabaseModels
{
    [Table("comments")]
    public class CommentModel
    {
        [Column("c_id")]
        public int Id { get; set; }
        [Column("c_author_nickname")]
        public string AuthorNickname { get; set; } = string.Empty;
        [Column("c_content")]
        public string Content { get; set; } = string.Empty;
        [Column("c_post_id")]
        public int PostId { get; set; }

        [JsonIgnore]
        public PostModel Post { get; set; } = new PostModel();
    }
}
