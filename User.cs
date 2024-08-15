
using System.Text.RegularExpressions;

namespace DemoApp.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public byte[] PasswordHash { get; private set; }
    public byte[] PasswordSalt { get; private set; }
    public bool IsEmailVerified { get; private set; }

    // Public constructor enforcing all attributes to be provided.
    public User(string name, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = ValidateEmail(email);
        IsEmailVerified = false;

        // Generate password hash and salt
        (PasswordHash, PasswordSalt) = CreatePasswordHash(password);
    }

    // Parameterless constructor required by EF Core
    #pragma warning disable CS8618
    protected User() { }
    #pragma warning restore CS8618

    // Method to verify the password
    public bool VerifyPassword(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(PasswordHash);
    }

    // Method to change the password
    public void ChangePassword(string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
        {
            throw new ArgumentException("New password cannot be empty.", nameof(newPassword));
        }

        // Generate new password hash and salt
        (PasswordHash, PasswordSalt) = CreatePasswordHash(newPassword);
    }

    // Method to verify the user's email
    public void SetEmailToVerified()
    {
        IsEmailVerified = true;
    }

    // Private method to validate the email format
    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");

        // Basic email validation regex
        var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        if (!emailRegex.IsMatch(email))
        {
            throw new ArgumentException("Invalid email format.", nameof(email));
        }

        return email;
    }

    // Private method to create a password hash and salt
    private static (byte[] hash, byte[] salt) CreatePasswordHash(string password)
    {
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return (hash, salt);
    }

}

