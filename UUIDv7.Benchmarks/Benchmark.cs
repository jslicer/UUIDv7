// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Benchmark.cs" company="Always Elucidated Solution Pioneers, LLC">
//   Copyright (c) Always Elucidated Solution Pioneers, LLC. All rights reserved.
// </copyright>
// <summary>
//   Benchmark the UUIDv7 algorithm.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace UUIDv7.Benchmarks;

using BenchmarkDotNet.Attributes;

/// <summary>
/// Benchmark the UUIDv7 algorithm.
/// </summary>
#pragma warning disable CA1515 // Consider making public types internal
[Config(typeof(BenchmarkConfig))]
public class Benchmark
#pragma warning restore CA1515 // Consider making public types internal
{
    /// <summary>
    /// Gets or sets the payload length for the benchmarks.
    /// </summary>
    [Params(1_000, 10_000, 100_000, 1_000_000)]
    //// ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int PayloadLength { get; set; }

    /// <summary>
    /// Benchmarks Uuid7.Create().
    /// </summary>
    [Benchmark]
    public void BenchmarkMethod()
    {
        for (int i = 0; i < PayloadLength; i++)
        {
            TextWriter.Null.WriteLine(Uuid7.Create());
        }
    }
}