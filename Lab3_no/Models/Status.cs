using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Lab3_no.Models;

public partial class Status
{
    [Key]
    public int Id { get; set; }

    [Column("Status")]
    [StringLength(255)]
    public string? Status1 { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
