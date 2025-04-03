using System;
using to_do_api.Dtos;

namespace to_do_api.IRepository;

public interface ITaskRepository
{
    Task<List<TaskDto>> GetTasksAsync();
    Task<TaskDto> GetTaskByIdAsync(int taskId);
    Task AddTaskAsync(TaskDto taskDto);
    Task MarkTaskAsCompletedAsync(int taskId);
}
