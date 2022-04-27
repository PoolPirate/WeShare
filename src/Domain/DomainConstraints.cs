namespace WeShare.Domain;

public static class DomainConstraints
{
    public const int UsernameLengthMinimum = 3;
    public const int UsernameLengthMaximum = 20;

    public const int NicknameLengthMinimum = 3;
    public const int NicknameLengthMaximum = 20;

    public const int EmailLengthMaximum = 128;

    public const int PasswordLengthMinimum = 8;
    public const int PasswordLengthMaximum = 64;

    public const int ShareNameLengthMinimum = 3;
    public const int ShareNameLengthMaximum = 32;

    public const int ShareDescriptionLengthMaximum = 256;

    public const int ShareReadmeLengthMaximum = 4096;

    public const int ShareSecretLength = 32;

    public const int PostDataSizeMaximum = 1024 * 1024 * 10; //10MB

    public const int CallbackSecretLength = 32;
}
