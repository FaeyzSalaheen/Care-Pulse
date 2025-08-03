using System;
using System.Collections.Generic;

namespace Care_Pulse.Models;

public partial class Rating
{
    public int Id { get; set; }

    public int RateAmount { get; set; }

    public int UserId { get; set; }

    public int ContentId { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual Content Content { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
