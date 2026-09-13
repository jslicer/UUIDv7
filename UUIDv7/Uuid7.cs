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

    private const ushort Version7Value = 0x7000;

    /// <summary>
    /// Mask isolating the 12-bit <c>rand_a</c> field (the bits immediately following the version nibble) that is
    /// repurposed as a monotonic counter per RFC 9562 Section 6.2, Method 3 (Monotonic Random).
    /// </summary>
    private const ushort CounterMask = 0x0FFF;

    /// <summary>
    /// 281,474,976,710,655 milliseconds. Added to 00:00:00.0000000 UTC, January 1, 1970, in the Gregorian calendar
    /// becomes approximately 10889-09-17 00:25:26.710 UTC.
    /// </summary>
    private const long MaxUnixTimestampMilliseconds = (1L << 48) - 1;

#pragma warning disable IDE1006 // Naming Styles
    /// <summary>
    /// Synchronizes access to <see cref="_lastTimestampMilliseconds" /> and <see cref="_lastCounter" /> so that the
    /// monotonic counter is updated atomically across concurrent callers.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static readonly object _MonotonicityLock = new();

    /// <summary>
    /// The Unix timestamp, in milliseconds, used by the most recently created Version 7 <see cref="Guid" />.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static long _lastTimestampMilliseconds = -1;

    /// <summary>
    /// The 12-bit counter value embedded in the <c>rand_a</c> field of the most recently created Version 7
    /// <see cref="Guid" />, used to preserve ordering for values created within the same millisecond.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    private static ushort _lastCounter;
#pragma warning restore IDE1006 // Naming Styles

    /// <summary>
    /// Gets the unix epoch. The value of this constant is equivalent to 00:00:00.0000000 UTC, January 1, 1970, in
    /// the Gregorian calendar. <see cref="UnixEpoch" /> defines the point in time when Unix time is equal to 0.
    /// </summary>
    /// <value>
    /// The unix epoch - equivalent to 00:00:00.0000000 UTC, January 1, 1970, in the Gregorian calendar.
    /// </value>
#pragma warning disable format
    public static DateTimeOffset UnixEpoch { get; } = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
#pragma warning restore format

    /// <summary>Creates a new <see cref="Guid" /> using the current date/time, according to RFC 9562, following
    /// the Version 7 format.</summary>
    /// <returns>A new <see cref="Guid" /> according to RFC 9562, following the Version 7 format.</returns>
    // ReSharper disable once MethodTooLong
    public static Guid Create() => Create(DateTimeOffset.UtcNow);

    /// <summary>Creates a new <see cref="Guid" /> according to RFC 9562, following the Version 7 format.</summary>
    /// <param name="timestamp">The optional date time offset used to determine the Unix Epoch timestamp.</param>
    /// <returns>A new <see cref="Guid" /> according to RFC 9562, following the Version 7 format.</returns>
    /// <remarks>Values created within the same millisecond are monotonically increasing: the 12-bit <c>rand_a</c>
    /// field is used as a counter (RFC 9562 Section 6.2, Method 3) instead of being fully random, so a value
    /// created after another within the same millisecond will always sort later. This method is thread-safe with
    /// respect to that guarantee.</remarks>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="timestamp" /> represents an offset prior to
    /// <see cref="DateTimeOffset" /> of zero, or a value whose monotonic counter would overflow past the maximum
    /// representable Unix Epoch timestamp.</exception>
    // ReSharper disable once MethodTooLong
    // ReSharper disable once TooManyDeclarations
    public static Guid Create(DateTimeOffset timestamp)
    {
        if (timestamp < UnixEpoch)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timestamp),
                timestamp,
                "Date/times before 1970-01-01 00:00:00.000 UTC are not supported.");
        }

        //// ReSharper disable ComplexConditionExpression
        long unixTsMs = timestamp.ToUnixTimeMilliseconds();

        if (unixTsMs > MaxUnixTimestampMilliseconds)
        {
            throw new ArgumentOutOfRangeException(
                nameof(timestamp),
                timestamp,
                "Dates after 10889-09-17 00:25:26.710 UTC are not supported.");
        }

        byte[] initialGuid = Guid.NewGuid().ToByteArray();
        ushort counter;

        lock (_MonotonicityLock)
        {
            if (unixTsMs == _lastTimestampMilliseconds)
            {
                // Same millisecond as the previous value: advance the counter to preserve ordering.
                counter = (ushort)((_lastCounter + 1) & CounterMask);

                if (counter == 0)
                {
                    // The counter wrapped around within this millisecond. Borrow a millisecond so the resulting Guid
                    // still sorts after every value already generated for the previous timestamp.
                    unixTsMs++;

                    if (unixTsMs > MaxUnixTimestampMilliseconds)
                    {
                        throw new ArgumentOutOfRangeException(
                            nameof(timestamp),
                            timestamp,
                            "Dates after 10889-09-17 00:25:26.710 UTC are not supported.");
                    }
                }
            }
            else
            {
                // New millisecond: reseed the counter from fresh randomness rather than starting at zero, so
                // consecutive timestamps don't leak how many values were generated in the prior millisecond.
                counter = (ushort)(((initialGuid[6] << 8) | initialGuid[7]) & CounterMask);
            }

            _lastTimestampMilliseconds = unixTsMs;
            _lastCounter = counter;
        }

        // Guid's first three fields use mixed-endian representation internally. Supplying the timestamp as
        // these numeric fields causes Guid's canonical representation to contain the required big-endian
        // 48-bit Unix timestamp.
        int a = (int)(unixTsMs >> 16);
        short b = (short)unixTsMs;
        short c = (short)(counter | Version7Value);
        byte randomD = initialGuid[8];
        byte d = (byte)((randomD & ~Variant10xxMask) | Variant10xxValue);
        //// ReSharper restore ComplexConditionExpression

#pragma warning disable format
        return new(
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