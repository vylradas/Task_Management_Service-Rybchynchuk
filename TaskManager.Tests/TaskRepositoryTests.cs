using System;
using Xunit;
using TaskManager.Api.Models;
using TaskManager.Api.Repositories;
using ModelsTaskStatus = TaskManager.Api.Models.TaskStatus;

namespace TaskManager.Tests
{
    public class TaskRepositoryTests
    {
        [Fact]
        public void CreateTask_ShouldAddNewTaskWithDefaultStatus()
        {
            // Arrange
            var repo = new TaskRepository();
            var newTask = new TaskItem { Title = "Test", Description = "Test task" };

            // Act
            var created = repo.Create(newTask);

            // Assert
            Assert.NotNull(created);
            Assert.Equal(ModelsTaskStatus.Backlog, created.Status);
            Assert.Single(repo.GetAll());
        }

        [Fact]
        public void UpdateStatus_ShouldChangeStatus_WhenTransitionIsAllowed()
        {
            // Arrange
            var repo = new TaskRepository();
            var task = repo.Create(new TaskItem { Title = "Task", Description = "Transition test" });

            // Act
            repo.UpdateStatus(task.Id, ModelsTaskStatus.InWork);
            repo.UpdateStatus(task.Id, ModelsTaskStatus.Testing);
            repo.UpdateStatus(task.Id, ModelsTaskStatus.Done);

            // Assert
            var updated = repo.GetById(task.Id);
            Assert.Equal(ModelsTaskStatus.Done, updated.Status); // повернення Done
        }

        [Fact]
        public void UpdateStatus_ShouldThrowError_WhenTransitionIsInvalid()
        {
            // Arrange
            var repo = new TaskRepository();
            var task = repo.Create(new TaskItem { Title = "Invalid transition", Description = "Should fail" });

            // Act + Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                repo.UpdateStatus(task.Id, ModelsTaskStatus.Done);
            });

            Assert.Equal("Перехід із Backlog до Done заборонений", ex.Message);
        }
    }
}
