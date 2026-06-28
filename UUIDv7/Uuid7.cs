// <copyright file="Uuid7.cs" company="Always Elucidated Solution Pioneers, LLC">
// Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>

// Ignore Spelling: Dv Uuid timestamp
namespace UUIDv7;

/// <summary>
/// Generate a Version 7 UUID as per RFC 9562. Inspired by the following sources:
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/Guid.cs">Guid</see>.
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTimeOffset.cs">DateTimeOffset</see>.
/// <see href="https://github.com/dotnet/runtime/blob/main/src/libraries/System.Private.CoreLib/src/System/DateTime.cs">DateTime</see>.
/// </summary>
public static class Uuid7
{
    // ReSharper disable once InconsistentNaming
    private const byte Variant10xxMask = 0xC0;

    // ReSharper disable once InconsistentNaming
    private const byte Variant10xxValue = 0x80;

    private const ushort VersionMask = 0xF000;

    private const ushort Version7Value = 0x7000;

    /// <summary>
    /// Gets the unix epoch. The value of this constant is equivalent to 00:00:00.0000000 UTC, January 1, 1970, in
    /// the Gregorian calendar. <see cref="UnixEpoch" /> defines the point in time when Unix time is equal to 0.
    /// </summary>
    /// <value>
    /// The unix epoch - equivalent to 00:00:00.0000000 UTC, January 1, 1970, in the Gregorian calendar.
    /// </value>
#pragma warning disable format
    public static DateTimeOffset UnixEpoch { get; } = new (1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
#pragma warning restore format

    /// <summary>
    /// Gets the unix epoch. The value of this constant is equivalent to 03:14:07.0000000 UTC, January 19, 2038, in
    /// the Gregorian calendar. <see cref="UnixEpochMax" /> defines the point in time when Unix time is equal to
    /// 2147483647.
    /// </summary>
    /// <value>
    /// The unix epoch - equivalent to 03:14:07.0000000 UTC, January 19, 2038, in the Gregorian calendar.
    /// </value>
#pragma warning disable format
    public static DateTimeOffset UnixEpochMax { get; } = new (2038, 1, 19, 3, 14, 7, TimeSpan.Zero);
#pragma warning restore format

    /// <summary>Creates a new <see cref="Guid" /> using the current date/time, according to RFC 9562, following
    /// the Version 7 format.</summary>
    /// <returns>A new <see cref="Guid" /> according to RFC 9562, following the Version 7 format.</returns>
    // ReSharper disable once MethodTooLong
    public static Guid Create() => Create(DateTimeOffset.UtcNow);

    /// <summary>Creates a new <see cref="Guid" /> according to RFC 9562, following the Version 7 format.</summary>
    /// <param name="timestamp">The optional date time offset used to determine the Unix Epoch timestamp.</param>
    /// <returns>A new <see cref="Guid" /> according to RFC 9562, following the Version 7 format.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="timestamp" /> represents an offset prior to
    /// <see cref="DateTimeOffset" /> of zero.</exception>
    // ReSharper disable once MethodTooLong
    // ReSharper disable once TooManyDeclarations
    public static Guid Create(DateTimeOffset timestamp)
    {
        if (timestamp < UnixEpoch)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timestamp),
                timestamp,
                "Dates before 1970-01-01 are not supported.");
        }

        if (timestamp > UnixEpochMax)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timestamp),
                timestamp,
                "Dates after 2038-01-19 are not supported.");
        }

        //// ReSharper disable ComplexConditionExpression
        long unixTsMs = timestamp.ToUnixTimeMilliseconds();
        byte[] initialGuid = Guid.NewGuid().ToByteArray();
        int a = (int)(unixTsMs >> 16);
        short b = (short)unixTsMs;
        short resultC = (short)(initialGuid[6] | (initialGuid[7] << 8));
        short c = (short)((resultC & ~VersionMask) | Version7Value);
        byte resultD = initialGuid[8];
        byte d = (byte)((resultD & ~Variant10xxMask) | Variant10xxValue);
        //// ReSharper restore ComplexConditionExpression

#pragma warning disable format
        return new (
            a,
            b,
            c,
            d,
            initialGuid[9],
            initialGuid[10],
            initialGuid[11],
            initialGuid[12],
            initialGuid[13],
            initialGuid[14],
            initialGuid[15]);
#pragma warning restore format
    }
}