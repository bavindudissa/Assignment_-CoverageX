using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using to_do_api.Dtos;
using to_do_api.IRepository;
using to_do_api.Models;

namespace to_do_api.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly TodoAppContext _context;
    private readonly IMapper _mapper;

    public TaskRepository(TodoAppContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<TaskDto>> GetTasksAsync()
    {
        try
        {
            var tasks = await _context.Tasks
                .Where(t => !t.IsCompleted)
                .OrderByDescending(t => t.CreatedAt)
                .Take(5)
                .ToListAsync();

            return _mapper.Map<List<TaskDto>>(tasks);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error retrieving tasks: {ex.Message}");
        }
    }

    public async Task<TaskDto> GetTaskByIdAsync(int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.TaskId == taskId);

        return _mapper.Map<TaskDto>(task);  
    }

    public async System.Threading.Tasks.Task AddTaskAsync(TaskDto taskDto)
    {
        try
        {
            var task = _mapper.Map<to_do_api.Models.Task>(taskDto);
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception($"Error adding task: {ex.Message}");
        }

    }

    public async System.Threading.Tasks.Task MarkTaskAsCompletedAsync(int taskId)
    {
        var task = await _context.Tasks.FindAsync(taskId);
        if (task != null)
        {
            task.IsCompleted = true;
            await _context.SaveChangesAsync();
        }
        else
        {
            throw new Exception($"Task with ID {taskId} not found");
        }        
    }

}
