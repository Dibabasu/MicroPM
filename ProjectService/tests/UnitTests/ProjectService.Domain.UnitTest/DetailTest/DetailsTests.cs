namespace ProjectService.Domain.UnitTest.DetailTest;
public class DetailsTests
{
    [Fact]
    public void Details_Constructor_ValidatesNameAndDescription()
    {
        // Arrange
        var validName = "Valid Name";
        var validDescription = "Valid Description";

        // Act
        var details = new Details(validName, validDescription);

        // Assert
        Assert.Equal(validName, details.Name);
        Assert.Equal(validDescription, details.Description);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("a")]
    [InlineData("ab")]
    [InlineData("This name is way too long to be valid because it has more than fifty characters")]
    public void Details_Constructor_ThrowsExceptionForInvalidName(string invalidName)
    {
        // Arrange
        var validDescription = "Valid Description";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Details(invalidName, validDescription));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("This description is way too long to be valid because it has more than fifty characters")]
    public void Details_Constructor_ThrowsExceptionForInvalidDescription(string invalidDescription)
    {
        // Arrange
        var validName = "Valid Name";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Details(validName, invalidDescription));
    }
}
