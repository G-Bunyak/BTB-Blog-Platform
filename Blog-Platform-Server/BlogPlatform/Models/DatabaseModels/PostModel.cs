using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogPlatform.Models.DatabaseModels
{
    [Table("posts")]
    public class PostModel
    {
        [Column("p_id")]
        public int Id { get; set; }
        [Column("p_author_nickname")]
        public string AuthorNickname { get; set; } = string.Empty;
        [Column("p_title")]
        public string Title { get; set; } = string.Empty;
        [Column("p_content")]
        public string Content { get; set; } = string.Empty;

        [JsonIgnore]
        public ICollection<CommentModel> Comments { get; set; } = new List<CommentModel>();

        public override bool Equals(object? obj)
        {
            try
            {
                var secondObj = obj as PostModel;
                if (secondObj != null && secondObj.Id == Id && secondObj.AuthorNickname == AuthorNickname && secondObj.Title == Title)
                {
                    return true;
                }
                else
                { 
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
