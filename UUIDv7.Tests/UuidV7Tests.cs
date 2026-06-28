// <copyright file="UuidV7Tests.cs" company="Always Elucidated Solution Pioneers, LLC">
// Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>

// Ignore Spelling: Dv Uuid
namespace UUIDv7.Tests;

using static Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using static UUIDv7.Uuid7;

/// <summary>
/// Unit tests for <see cref="Uuid7" />.
/// </summary>
[TestClass]
#pragma warning disable CA1515 // Consider making public types internal
public sealed class UuidV7Tests
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Verifies <see cref="UnixEpoch" /> is set to the Unix epoch.
    /// </summary>
    [TestMethod]
    public void UnixEpochShouldMatchExpectedValue() => AreEqual(DateTimeOffset.UnixEpoch, UnixEpoch);

    /// <summary>
    /// Verifies <see cref="UnixEpochMax" /> has the expected maximum supported value.
    /// </summary>
    [TestMethod]
    public void UnixEpochMaxShouldMatchExpectedValue()
    {
#pragma warning disable format
        DateTimeOffset expected = new (2038, 1, 19, 3, 14, 7, TimeSpan.Zero);
#pragma warning restore format

        AreEqual(expected, UnixEpochMax);
    }

    /// <summary>
    /// Verifies <see cref="Create(DateTimeOffset)" /> throws for timestamps before Unix epoch.
    /// </summary>
    [TestMethod]
    public void CreateWithTimestampBeforeUnixEpochShouldThrow()
    {
        DateTimeOffset timestamp = UnixEpoch.AddMilliseconds(-1);
        ArgumentOutOfRangeException exception =
            ThrowsExactly<ArgumentOutOfRangeException>(() => Create(timestamp));

        AreEqual("timestamp", exception.ParamName);
    }

    /// <summary>
    /// Verifies <see cref="Create(DateTimeOffset)" /> throws for timestamps after the supported maximum.
    /// </summary>
    [TestMethod]
    public void CreateWithTimestampAfterUnixEpochMaxShouldThrow()
    {
        DateTimeOffset timestamp = UnixEpochMax.AddMilliseconds(1);
        ArgumentOutOfRangeException exception =
            ThrowsExactly<ArgumentOutOfRangeException>(() => Create(timestamp));

        AreEqual("timestamp", exception.ParamName);
    }

    /// <summary>
    /// Verifies creating with Unix epoch succeeds and encodes the expected timestamp.
    /// </summary>
    [TestMethod]
    public void CreateWithUnixEpochShouldSucceedAndEncodeTimestamp()
    {
        Guid guid = Create(UnixEpoch);
        long actualUnixMilliseconds = ExtractUnixMilliseconds(guid);

        AreEqual(UnixEpoch.ToUnixTimeMilliseconds(), actualUnixMilliseconds);
    }

    /// <summary>
    /// Verifies creating with the maximum supported Unix timestamp succeeds and encodes the expected timestamp.
    /// </summary>
    [TestMethod]
    public void CreateWithUnixEpochMaxShouldSucceedAndEncodeTimestamp()
    {
        Guid guid = Create(UnixEpochMax);
        long actualUnixMilliseconds = ExtractUnixMilliseconds(guid);

        AreEqual(UnixEpochMax.ToUnixTimeMilliseconds(), actualUnixMilliseconds);
    }

    /// <summary>
    /// Verifies the provided timestamp is encoded in the UUIDv7 high-order timestamp bytes.
    /// </summary>
    [TestMethod]
    public void CreateWithTimestampShouldEncodeUnixTimeMilliseconds()
    {
#pragma warning disable format
        DateTimeOffset timestamp = new (2024, 12, 31, 23, 59, 59, 123, TimeSpan.Zero);
#pragma warning restore format
        Guid guid = Create(timestamp);
        long actualUnixMilliseconds = ExtractUnixMilliseconds(guid);

        AreEqual(timestamp.ToUnixTimeMilliseconds(), actualUnixMilliseconds);
    }

    /// <summary>
    /// Verifies the version nibble is set to 7.
    /// </summary>
    [TestMethod]
    public void CreateWithTimestampShouldSetVersion7()
    {
#pragma warning disable format
        DateTimeOffset timestamp = new (2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
#pragma warning restore format
        Guid guid = Create(timestamp);
        byte[] bytes = guid.ToByteArray();
        int version = (bytes[7] & 0xF0) >> 4;

        AreEqual(7, version);
    }

    /// <summary>
    /// Verifies the variant bits are set to RFC 4122/9562 binary 10xx.
    /// </summary>
    [TestMethod]
    //// ReSharper disable once InconsistentNaming
    public void CreateWithTimestampShouldSetRfcVariant10xx()
    {
#pragma warning disable format
        DateTimeOffset timestamp = new (2025, 1, 1, 0, 0, 0, TimeSpan.Zero);
#pragma warning restore format
        Guid guid = Create(timestamp);
        byte[] bytes = guid.ToByteArray();
        int variant = bytes[8] & 0xC0;

        AreEqual(0x80, variant);
    }

    /// <summary>
    /// Verifies the parameterless create API emits a non-empty UUIDv7 with a current timestamp.
    /// </summary>
    [TestMethod]
    //// ReSharper disable once TooManyDeclarations
    public void CreateWithoutTimestampShouldProduceVersion7WithCurrentTimestampRange()
    {
        long before = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        Guid guid = Create();
        long after = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        byte[] bytes = guid.ToByteArray();
        int version = (bytes[7] & 0xF0) >> 4;
        int variant = bytes[8] & 0xC0;
        long actualUnixMilliseconds = ExtractUnixMilliseconds(guid);

        AreNotEqual(Guid.Empty, guid);
        AreEqual(7, version);
        AreEqual(0x80, variant);
        IsGreaterThanOrEqualTo(actualUnixMilliseconds, before);
        IsLessThanOrEqualTo(actualUnixMilliseconds, after);
    }

    /// <summary>
    /// Verifies two UUIDv7 values created at the same timestamp are expected to differ due to randomness.
    /// </summary>
    [TestMethod]
    public void CreateWithSameTimestampShouldTypicallyGenerateDifferentGuid()
    {
#pragma warning disable format
        DateTimeOffset timestamp = new (2025, 2, 3, 4, 5, 6, 789, TimeSpan.Zero);
#pragma warning restore format
        Guid first = Create(timestamp);
        Guid second = Create(timestamp);

        AreNotEqual(first, second);
    }

    private static long ExtractUnixMilliseconds(Guid guid)
    {
        byte[] bytes = guid.ToByteArray();
        int high32 = BitConverter.ToInt32(bytes, 0);
        ushort low16 = BitConverter.ToUInt16(bytes, 4);

        return ((long)high32 << 16) | low16;
    }
}