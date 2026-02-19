namespace FarmTests;

public static class TestUtils
{
    private static readonly object _consoleLock = new();

    public static string CaptureConsoleOutput(Action action)
    {
        lock (_consoleLock)
        {
            var originalOut = Console.Out;
            var sw = new StringWriter();

            try
            {
                Console.SetOut(sw);
                action();
                Console.Out.Flush();
                return sw.ToString();
            }
            finally
            {
                Console.SetOut(originalOut);
            }
        }
    }
}