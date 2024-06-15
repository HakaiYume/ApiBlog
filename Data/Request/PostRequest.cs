using System;
using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Data.Request
{
    public partial class PostRequest
    {
        [Required(ErrorMessage = "El campo 'Autor' es requerido.")]
        public int Author { get; set; }

        [Required(ErrorMessage = "El campo 'Title' es requerido.")]
        [MaxLength(100, ErrorMessage = "El campo 'Title' no puede exceder los 100 caracteres.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Content' es requerido.")]
        public string Content { get; set; } = null!;

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }
    }
}
