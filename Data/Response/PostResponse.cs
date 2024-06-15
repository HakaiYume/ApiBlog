using System;

namespace ApiBlog.Data.Response
{
    public partial class PostResponse
    {
        public int Id { get; set; }
        public int? Author { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
