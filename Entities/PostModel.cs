using System.Text.Json.Serialization;

namespace Blog_RWA.Entities
{
    public class PostModel : PostBaseModel
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int UserId { get; set; }

        [JsonIgnore]
        public UserModel? User { get; set; }
    }
}
