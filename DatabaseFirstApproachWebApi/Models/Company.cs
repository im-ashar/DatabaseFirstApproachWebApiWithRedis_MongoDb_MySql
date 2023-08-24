using System;
using System.Collections.Generic;

namespace DatabaseFirstApproachWebApi.Models;

public partial class Company
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
