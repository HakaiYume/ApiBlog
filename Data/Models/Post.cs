using System;
using System.Collections.Generic;

namespace ApiBlog.Data.Models;

public partial class Post
{
    public int Id { get; set; }

    public int? Author { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? AuthorNavigation { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
