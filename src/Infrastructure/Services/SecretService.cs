using Common.Services;
using System.Security.Cryptography;
using System.Text;
using WeShare.Application.Services;
using WeShare.Domain;
using WeShare.Domain.Entities;
using WeShare.Infrastructure.Options;

namespace WeShare.Infrastructure.Services;
public class SecretService : Singleton, ISecretService
{
    private const int ByteSize = 256;

    [Inject]
    private readonly RandomNumberGenerator NumberGenerator;

    [Inject]
    private readonly SecurityOptions SecurityOptions;

    protected override ValueTask InitializeAsync() => base.InitializeAsync();

    public string HashPassword(PlainTextPassword plainPassword)
        => BCrypt.Net.BCrypt.EnhancedHashPassword(plainPassword.Value, SecurityOptions.HashWorkFactor);

    public bool VerifyPassword(string passwordHash, PlainTextPassword input)
        => BCrypt.Net.BCrypt.EnhancedVerify(input.Value, passwordHash);

    public CallbackSecret GenerateCallbackSecret()
        => CallbackSecret.From(GenerateRandomString(DomainConstraints.CallbackSecretLength));

    public ShareSecret GenerateShareSecret()
        => ShareSecret.From(GenerateRandomString(DomainConstraints.ShareSecretLength));

    private string GenerateRandomString(int length)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length), "length cannot be less than zero.");
        }

        var result = new StringBuilder();
        byte[]? buf = new byte[128];
        while (result.Length < length)
        {
            NumberGenerator.GetBytes(buf);
            for (int i = 0; i < buf.Length && result.Length < length; ++i)
            {
                // Divide the byte into allowedCharSet-sized groups. If the
                // random value falls into the last group and the last group is
                // too small to choose from the entire allowedCharSet, ignore
                // the value in order to avoid biasing the result.
                int outOfRangeStart = ByteSize - (ByteSize % CharSet.Url.Length);
                if (outOfRangeStart <= buf[i])
                {
                    continue;
                }

                result.Append(CharSet.Url[buf[i] % CharSet.Url.Length]);
            }
        }

        return result.ToString();
    }
}
