using DemoApp.Domain.Entities;

namespace DemoApp.Domain.Tests.UnitTests;

public class UserTests
{
    [Fact]
    public void User_Creation_ShouldInitializePropertiesCorrectly()
    {
        // Arrange
        var name = "John Doe";
        var email = "john.doe@example.com";
        var password = "Password123";

        // Act
        var user = new User(name, email, password);

        // Assert
        Assert.Equal(name, user.Name);
        Assert.Equal(email, user.Email);
        Assert.False(user.IsEmailVerified);
        Assert.NotNull(user.PasswordHash);
        Assert.NotNull(user.PasswordSalt);
        Assert.True(user.VerifyPassword(password));
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("another@invalid")]
    [InlineData("noatsymbol.com")]
    public void User_Creation_ShouldThrowException_ForInvalidEmail(string invalidEmail)
    {
        // Arrange
        var name = "John Doe";
        var password = "Password123";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new User(name, invalidEmail, password));
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForCorrectPassword()
    {
        // Arrange
        var user = new User("Jane Doe", "jane.doe@example.com", "SecurePassword456");

        // Act
        var result = user.VerifyPassword("SecurePassword456");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForIncorrectPassword()
    {
        // Arrange
        var user = new User("Jane Doe", "jane.doe@example.com", "SecurePassword456");

        // Act
        var result = user.VerifyPassword("WrongPassword");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ChangePassword_ShouldUpdatePasswordHashAndSalt()
    {
        // Arrange
        var user = new User("John Smith", "john.smith@example.com", "OldPassword789");
        var oldPasswordHash = user.PasswordHash;

        // Act
        user.ChangePassword("NewPassword123");

        // Assert
        Assert.NotEqual(oldPasswordHash, user.PasswordHash);
        Assert.True(user.VerifyPassword("NewPassword123"));
        Assert.False(user.VerifyPassword("OldPassword789"));
    }

    [Fact]
    public void SetEmailToVerified_ShouldSetIsEmailVerifiedToTrue()
    {
        // Arrange
        var user = new User("Jane Smith", "jane.smith@example.com", "AnotherPassword123");

        // Act
        user.SetEmailToVerified();

        // Assert
        Assert.True(user.IsEmailVerified);
    }
}

