namespace Ether.Validation;

internal static class Assert
{
    public static void That(bool condition, Exception exception)
    {
        if (!condition)
        {
            throw exception;
        }
    }

    public static void That(bool condition, string message) => That(condition, new Exception(message));
}