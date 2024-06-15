using System;
using System.ComponentModel.DataAnnotations;

namespace ApiBlog.Data.Request
{
    public partial class LoginRequest
    {

        [Required(ErrorMessage = "El campo 'email' es requerido.")]
        [MaxLength(255, ErrorMessage = "El campo 'email' no puede exceder los 255 caracteres.")]
        public string email { get; set; } = null!;

        [Required(ErrorMessage = "El campo 'Password' es requerido.")]
        [MaxLength(255, ErrorMessage = "El campo 'Password' no puede exceder los 255 caracteres.")]
        public string Password { get; set; } = null!;
    }
}
