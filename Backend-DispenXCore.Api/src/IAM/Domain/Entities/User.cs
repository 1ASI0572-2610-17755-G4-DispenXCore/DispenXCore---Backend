namespace Backend_DispenXCore.Api.src.IAM.Domain.Entities;

public enum UserRole { USER, ADMIN }
public enum UserStatus { ACTIVE, INACTIVE }

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public string? PhotoUrl { get; private set; }

    private User() { }

    public User(string firstName, string lastName, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        Role = UserRole.USER;
        Status = UserStatus.ACTIVE;
    }

    public void UpdateProfile(string firstName, string lastName, string? photoUrl)
    {
        FirstName = firstName;
        LastName = lastName;
        PhotoUrl = photoUrl;
    }

    public void ChangePassword(string newPasswordHash)
    {
        PasswordHash = newPasswordHash;
    }
}