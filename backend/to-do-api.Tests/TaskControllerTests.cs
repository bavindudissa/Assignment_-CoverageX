using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using to_do_api.Controllers;
using to_do_api.Dtos;
using to_do_api.IRepository;
using Xunit;

namespace to_do_api.Tests
{
    public class TaskControllerTests
    {
        private readonly Mock<ITaskRepository> _mockRepo;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mockRepo = new Mock<ITaskRepository>();
            _controller = new TaskController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetTasks_ReturnsOkResult_WhenTasksExist()
        {
            // Arrange
            var tasks = new List<TaskDto>
            {
                new TaskDto { TaskId = 1, Title = "Task 1", Description = "This is task one.", IsCompleted = false },
                new TaskDto { TaskId = 2, Title = "Task 2", Description = "This is task one.", IsCompleted = true }
            };

            _mockRepo.Setup(repo => repo.GetTasksAsync()).ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsAssignableFrom<List<TaskDto>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetTasks_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            _mockRepo.Setup(repo => repo.GetTasksAsync()).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _controller.GetTasks();

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error retrieving tasks: Database error", statusCodeResult.Value);
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedAtAction_WhenTaskIsCreated()
        {
            // Arrange
            var taskDto = new TaskDto { TaskId = 1, Title = "New Task", Description = "This is new task.", IsCompleted = false };
            _mockRepo.Setup(repo => repo.AddTaskAsync(It.IsAny<TaskDto>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<TaskDto>(createdAtActionResult.Value);
            Assert.Equal(taskDto.TaskId, returnValue.TaskId);
        }

        [Fact]
        public async Task CreateTask_ReturnsBadRequest_WhenTaskDtoIsNull()
        {
            // Arrange
            TaskDto taskDto = null;

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task CreateTask_ReturnsInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var taskDto = new TaskDto { TaskId = 1, Title = "New Task", Description = "This is new task.", IsCompleted = false };
            _mockRepo.Setup(repo => repo.AddTaskAsync(It.IsAny<TaskDto>())).ThrowsAsync(new Exception("Error creating task"));

            // Act
            var result = await _controller.CreateTask(taskDto);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Equal("Error creating task: Error creating task", statusCodeResult.Value);
        }

        [Fact]
        public async Task CompleteTask_ReturnsNoContent_WhenTaskIsCompleted()
        {
            // Arrange
            var taskId = 1;
            _mockRepo.Setup(repo => repo.MarkTaskAsCompletedAsync(taskId)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CompleteTask(taskId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task CompleteTask_ReturnsNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var taskId = 999;
            _mockRepo.Setup(repo => repo.MarkTaskAsCompletedAsync(taskId)).ThrowsAsync(new Exception("Task not found"));

            // Act
            var result = await _controller.CompleteTask(taskId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Task not found", notFoundResult.Value);
        }
    }
}
