using System;
using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Data.Request
{
    public partial class CommentRequest
    {
        [Required(ErrorMessage = "El campo 'Post' es requerido.")]
        public int? Post { get; set; }

        [Required(ErrorMessage = "El campo 'Author' es requerido.")]
        public int? Author { get; set; }

        [Required(ErrorMessage = "El campo 'Content' es requerido.")]
        [MaxLength(500, ErrorMessage = "El campo 'Content' no puede exceder los 500 caracteres.")]
        public string Content { get; set; } = null!;
        
        [Required(ErrorMessage = "El campo 'CreatedAt' es requerido.")]
        public DateTime CreatedAt { get; set; }
    }
}
