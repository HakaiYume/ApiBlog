using System;

namespace ApiBlog.Data.Response
{
    public partial class CommentResponse
    {
        public int Id { get; set; }
        public int? Post { get; set; }
        public int? Author { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}