using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Category
{
    public int IdCategori { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
