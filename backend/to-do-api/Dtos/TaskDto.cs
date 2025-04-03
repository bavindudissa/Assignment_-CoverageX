using System;

namespace to_do_api.Dtos;

public class TaskDto
{
    public int? TaskId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsCompleted { get; set; }
}
