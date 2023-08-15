using ProjectService.Domain.Event;

namespace ProjectService.Domain.UnitTest.BaseEntityTest;

public class BaseEntityTests
    {
        [Fact]
        public void BaseEntity_AddDomainEvent_AddsEventToCollection()
        {        // Arrange
            var project = CreateTestProject();

            // Assert
            Assert.Single(project.DomainEvents);
            Assert.IsType<ProjectCreatedEvent>(project.DomainEvents.First());
        }

        [Fact]
        public void BaseEntity_RemoveDomainEvent_RemovesEventFromCollection()
        {
            // Arrange
            var project = CreateTestProject();
            var domainEvent = project.DomainEvents.First();

            // Act
            project.RemoveDomainEvent(domainEvent);

            // Assert
            Assert.Empty(project.DomainEvents);
        }

        [Fact]
        public void BaseEntity_ClearDomainEvents_ClearsEventCollection()
        {
            // Arrange
            var project = CreateTestProject();

            // Act
            project.ClearDomainEvents();

            // Assert
            Assert.Empty(project.DomainEvents);
        }

        private Project CreateTestProject()
        {
            var projectDetails = new Details("Test Project", "This is a test project");
            var ownerId = Guid.NewGuid();
            var workflowId = Guid.NewGuid();
            return new Project(projectDetails, ownerId, workflowId);
        }
    }
