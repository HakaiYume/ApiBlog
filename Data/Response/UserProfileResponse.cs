using System;

namespace ApiBlog.Data.Response
{
    public partial class UserProfileResponse
    {
        public int UserId { get; set; }
        public string? Bio { get; set; }
        public string? ProfileImageUrl { get; set; }
    }
}
