using Xunit;
using Moq;
using JonDou9000.TaskPlanner.Domain.Logic;
using JonDou9000.TaskPlanner.Domain.Models;
using JonDou9000.TaskPlanner.Domain.DataAccess.Abstractions;
using System.Linq;
using JonDou9000.TaskPlanner.DataAccess.Abstractions;
using JonDou9000.TaskPlanner.Domain.Models.Enums;

namespace JonDou9000.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        [Fact]
        public void CreatePlan_SortsTasksByPriorityAndDueDate()
        {
            var mockRepository = new Mock<IWorkItemsRepository>();
            mockRepository.Setup(repo => repo.GetAll()).Returns(new[]
            {
                new WorkItem { Id = 1, Title = "Task1", DueDate = new DateTime(2023, 1, 1), Priority = Priority.Low },
                new WorkItem { Id = 2, Title = "Task2", DueDate = new DateTime(2023, 1, 2), Priority = Priority.High },
                new WorkItem { Id = 3, Title = "Task3", DueDate = new DateTime(2023, 1, 3), Priority = Priority.Medium }
            });

            var taskPlanner = new SimpleTaskPlanner(mockRepository.Object);

            var plan = taskPlanner.CreatePlan();

            Assert.Collection(plan,
                item => Assert.Equal("Task2", item.Title),
                item => Assert.Equal("Task3", item.Title),
                item => Assert.Equal("Task1", item.Title)
            );
        }
    }
}
