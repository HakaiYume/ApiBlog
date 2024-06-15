using System;
using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Data.Request
{
    public partial class UserRequest
    {
        [Required(ErrorMessage = "El campo 'Username' es requerido.")]
        [MaxLength(50, ErrorMessage = "El campo 'Username' no puede exceder los 50 caracteres.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Email' es requerido.")]
        [EmailAddress(ErrorMessage = "El campo 'Email' debe ser una dirección de correo electrónico válida.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Password' es requerido.")]
        [MinLength(8, ErrorMessage = "El campo 'Password' debe tener al menos 8 caracteres.")]
        public string Password { get; set; } = null!;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [MaxLength(500, ErrorMessage = "El campo 'Bio' no puede exceder los 500 caracteres.")]
        public string? Bio { get; set; }
        
        [MaxLength(250, ErrorMessage = "El campo 'ProfileImageUrl' no puede exceder los 250 caracteres.")]
        public string? ProfileImageUrl { get; set; }
    }
}
