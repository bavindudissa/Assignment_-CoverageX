using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using to_do_api.Dtos;
using to_do_api.IRepository;

namespace to_do_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }   

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var taskDtos = await _taskRepository.GetTasksAsync();
                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving tasks: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] TaskDto taskDto)
        {
            if (taskDto == null)
            {
                return BadRequest();
            }

            try
            {
                await _taskRepository.AddTaskAsync(taskDto);
                return CreatedAtAction(nameof(GetTasks), new { id = taskDto.TaskId }, taskDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error creating task: {ex.Message}");
            }
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteTask(int id)
        {
            try
            {
                await _taskRepository.MarkTaskAsCompletedAsync(id);
                return NoContent(); // Return 204 No Content if successful
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Return 404 if task not found
            }
        }
    }
}
