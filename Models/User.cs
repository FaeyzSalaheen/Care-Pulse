using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Care_Pulse.Models;
[Index(nameof(Email), IsUnique = true)]
public partial class User
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? Image { get; set; }

    public string? Bio { get; set; }

    public int Role { get; set; }

    public string? Gender { get; set; }

    public string? Specialty { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<Content> Contents { get; set; } = new List<Content>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();
}

