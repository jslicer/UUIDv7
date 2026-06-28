# UUIDv7
A .NET Standard 2.0 library to create UUIDv7-style Guids.

Example:

```cs
namespace UUIDv7.Console;

using static System.Console;
using static Uuid7;

public static class Program
{
    public static void Main()
    {
        WriteLine(UnixEpoch);
        WriteLine(UnixEpochMax);
        for (int i = 0; i < 20; i++)
        {
            WriteLine(Create());
        }

        WriteLine();

        DateTimeOffset offset = DateTimeOffset.UtcNow;

        for (int i = 0; i < 20; i++)
        {
            WriteLine(Create(offset));
        }
    }
}
```