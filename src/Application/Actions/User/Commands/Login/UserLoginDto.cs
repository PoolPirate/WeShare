namespace WeShare.Application.DTOs;

public class UserLoginDto
{
    /// <summary>
    /// A snippet of information about the account that you have just logged into.
    /// </summary>
    public UserSnippetDto UserSnippet { get; private set; }

    /// <summary>
    /// The JWT Bearer token used for auth in protected endpoints.
    /// </summary>
    public string Token { get; private set; }

    /// <summary>
    /// The duration in seconds that this token is valid.
    /// </summary>
    public int ExpiresIn { get; private set; }

    public UserLoginDto(UserSnippetDto userSnippet, string token, int expiresIn)
    {
        UserSnippet = userSnippet;
        Token = token;
        ExpiresIn = expiresIn;
    }
}
