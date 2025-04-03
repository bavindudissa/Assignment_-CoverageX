using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;
using to_do_api.Dtos;
using System.Net.Http.Json;

namespace to_do_api.Tests
{
    public class TaskControllerIntegrationTests
    {
        [Fact]
        public async Task GetTasks_ReturnsSuccessStatusCode()
        {
            //Arrange
            var application = new CustomWebApplicationFactory();

            var client = application.CreateClient();
            
            // Act
            var response = await client.GetAsync("/api/task");

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CreateTask_ReturnsCreatedStatusCode()
        {
            // Arrange
            var application = new CustomWebApplicationFactory();
            var taskDto = new TaskDto
            {
                Title = "Test Task",
                Description = "This is a test task",
                IsCompleted = false
            };

            var client = application.CreateClient(); 

            // Act
            var response = await client.PostAsJsonAsync("/api/task", taskDto);

            // Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task CompleteTask_ReturnsNoContent()
        {
            // Arrange
            var application = new CustomWebApplicationFactory();
            var taskDto = new TaskDto
            {
                TaskId = 2,
                Title = "Test Task to Complete",
                Description = "This task will be completed",
                IsCompleted = false
            };

            var client = application.CreateClient(); 

            await client.PostAsJsonAsync("/api/task", taskDto);

            // Act
            var response = await client.PutAsync("/api/task/2/complete", null);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
