using System;
using System.Collections.Generic;

namespace DatabaseFirstApproachWebApi.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CompanyId { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Company? Company { get; set; }
}
