namespace WeShare.Domain;

public static class CharSet
{
    public static readonly char[] LowerLetters = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
    public static readonly char[] UpperLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
    public static readonly char[] Numbers = "0123456789".ToCharArray();
    public static readonly char[] SpecialChars = "*.! @#$%^&(){}[]:;<>,.?/~_+-=|\\\".".ToCharArray();

    public static readonly char[] Url = Enumerable.Concat(LowerLetters, UpperLetters).Concat(Numbers).ToArray();
}
