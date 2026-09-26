# UUIDv7
A .NET Standard 2.0 library to create UUIDv7-style Guids.

Example:

```cs
namespace UUIDv7.Console;

using static System.Console;
using static UUIDv7.Uuid7;

public static class Program
{
    public static void Main()
    {
        WriteLine(UnixEpoch);
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
```

## Benchmark Results

<!-- BENCHMARK_RESULTS_START -->
```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 9V45 4.07GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4
  DefaultJob : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v4


```
| Method          | PayloadLength | Mean         | Error       | StdDev      |
|---------------- |-------------- |-------------:|------------:|------------:|
| **BenchmarkMethod** | **1000**          |     **516.7 μs** |    **10.12 μs** |     **9.94 μs** |
| **BenchmarkMethod** | **10000**         |   **5,033.4 μs** |   **100.18 μs** |   **123.02 μs** |
| **BenchmarkMethod** | **100000**        |  **51,649.7 μs** |   **946.35 μs** |   **885.22 μs** |
| **BenchmarkMethod** | **1000000**       | **517,790.5 μs** | **2,478.79 μs** | **2,197.38 μs** |
<!-- BENCHMARK_RESULTS_END -->
