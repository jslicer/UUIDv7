// <copyright file="Program.cs" company="Always Elucidated Solution Pioneers, LLC">
// Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>

// Ignore Spelling: Dv
namespace UUIDv7.Console;

using static System.Console;
using static Uuid7;

/// <summary>
/// Holds the entry point of the application.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    private static void Main()
    {
        WriteLine(UnixEpoch);
        WriteLine(UnixEpochMax);
        for (int i = 0; i < 20; i++)
        {
            WriteLine(Create());
        }

        WriteLine();

        DateTimeOffset offset = DateTimeOffset.UtcNow;

        for (int i = 0; i < 10; i++)
        {
            WriteLine(Create(offset));
        }
    }
}