using System;

namespace ApiBlog.Data.Response
{
    public partial class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public object? Profile { get; set; }
    }
}
