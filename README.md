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

## Benchmark Results

<!-- BENCHMARK_RESULTS_START -->
```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.45GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3


```
| Method          | PayloadLength | Mean         | Error       | StdDev      |
|---------------- |-------------- |-------------:|------------:|------------:|
| **BenchmarkMethod** | **1000**          |     **710.9 μs** |     **4.54 μs** |     **4.02 μs** |
| **BenchmarkMethod** | **10000**         |   **7,176.4 μs** |    **43.55 μs** |    **40.73 μs** |
| **BenchmarkMethod** | **100000**        |  **71,055.8 μs** |   **238.53 μs** |   **211.45 μs** |
| **BenchmarkMethod** | **1000000**       | **706,102.5 μs** | **1,425.34 μs** | **1,190.23 μs** |
<!-- BENCHMARK_RESULTS_END -->
