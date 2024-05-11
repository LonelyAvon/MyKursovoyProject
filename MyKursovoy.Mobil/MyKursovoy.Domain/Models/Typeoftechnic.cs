using System;
using System.Collections.Generic;

namespace MyKursovoy.Domain.Models;

public partial class Typeoftechnic
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? PhotoPath { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
