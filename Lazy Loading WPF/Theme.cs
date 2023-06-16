using System;
using System.Collections.Generic;

namespace Lazy_Loading_WPF;

public partial class Theme
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
