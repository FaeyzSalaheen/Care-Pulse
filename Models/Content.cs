using System;
using System.Collections.Generic;

namespace Care_Pulse.Models;

public partial class Content
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? FileUrl { get; set; }

    public string? ContentType { get; set; }

    public int UploadedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual User UploadedBy { get; set; } = null!;
}
