using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using WeShare.Domain.Common;
using WeShare.Domain.Events;

namespace WeShare.Domain.Entities;

public class User : AuditableEntity, IHasDomainEvents
{
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    /// <summary>
    /// The id of the user.
    /// </summary>
    public UserId Id { get; init; }

    /// <summary>
    /// The name of the user.
    /// </summary>
    public Username Username { get; set; }

    #region Account
    /// <summary>
    /// The email address of the user.
    /// </summary>
    [MaxLength(DomainConstraints.EmailLengthMaximum)]
    public string Email { get; private set; }

    /// <summary>
    /// Whether or not the email is verified.
    /// </summary>
    public bool EmailVerified { get; private set; }

    /// <summary>
    /// The hashed password of the user.
    /// </summary>
    public string PasswordHash { get; set; }
    #endregion

    #region Profile
    /// <summary>
    /// The nickname of this profile.
    /// </summary>
    public Nickname Nickname { get; set; }

    /// <summary>
    /// Wether or not the likes of this user can be accessed by anyone or just the user itself.
    /// </summary>
    public bool LikesPublished { get; set; }
    #endregion

    /// <summary>
    /// The shares which this user owns.
    /// </summary>
    public List<Share>? Shares { get; init; } //Navigation Property

    /// <summary>
    /// The likes which this user owns.
    /// </summary>
    public List<Like>? Likes { get; init; } //Navigation Property

    /// <summary>
    /// The callbacks which this account owns.
    /// </summary>
    public List<Callback>? Callbacks { get; init; } //Navigation Property

    public void UpdateEmail(string email)
    {
        string newEmail = email.ToUpper();

        var updatedEvent = new UserEmailUpdatedEvent(Id, Email, newEmail);
        DomainEvents.Add(updatedEvent);

        Email = newEmail;
        EmailVerified = false;
    }
    public void VerifyEmail()
        => EmailVerified = true;

    public static User Create(Username username, string email, string passwordHash, Nickname nickname)
    {
        var user = new User(username, email, passwordHash, nickname);
        user.DomainEvents.Add(new UserCreatedEvent(user));
        return user;
    }

    private User(Username username, string email, string passwordHash, Nickname nickname)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        Nickname = nickname;
    }

    [JsonConstructor]
#pragma warning disable CS8618
    public User()
#pragma warning restore CS8618
    {
    }
}
