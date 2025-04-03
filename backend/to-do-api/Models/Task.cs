using System;
using System.Collections.Generic;

namespace to_do_api.Models;

public partial class Task
{
    public int TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }
}
