using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogR.Entities
{
    public class PostBaseModel
    {
        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título não pode ter mais de 100 caracteres")]
        public required string Title { get; set; }

        [Required(ErrorMessage = "O conteúdo é obrigatório")]
        public required string Content { get; set; }
    }
}
