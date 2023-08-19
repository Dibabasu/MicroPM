
namespace ProjectService.Domain.UnitTest.ComponentTest;
public class ComponentTests
{
    [Fact]
    public void Component_Constructor_CreatesNewComponentWithDetails()
    {
        // Arrange
        var details = new Details("Test Component", "This is a test component");

        // Act
        var component = new Component(details);

        // Assert
        Assert.NotNull(component.Id);
        Assert.Equal(details, component.ComponentDetails);
    }

    [Fact]
    public void Component_UpdateDetails_UpdatesComponentDetailsAndProjectId()
    {
        // Arrange
        var details = new Details("Test Component", "This is a test component");
        var component = new Component(details);
        var newDetails = new Details("Updated Component", "This is an updated component");
        var newProjectId = Guid.NewGuid();

        // Act
        component.UpdateDetails(newDetails, newProjectId, Guid.NewGuid());

        // Assert
        Assert.Equal(newDetails, component.ComponentDetails);
        Assert.Equal(newProjectId, component.ProjectId);
    }
}