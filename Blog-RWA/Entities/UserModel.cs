using System.Text.Json.Serialization;

namespace Blog_RWA.Entities
{
    public class UserModel
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }

        [JsonIgnore]
        public ICollection<PostModel>? Posts { get; set; }
    }
}
