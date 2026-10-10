using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3_no.Models;

public partial class Role
{
    [Key]
    public int Id { get; set; }

    [Column("Role")]
    [StringLength(255)]
    public string? Role1 { get; set; }

    [InverseProperty("RoleNavigation")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
