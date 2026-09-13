```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.5 LTS (Noble Numbat)
AMD EPYC 7763 2.66GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.401
  [Host]     : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.12 (10.0.12, 10.0.1226.42308), X64 RyuJIT x86-64-v3


```
| Method          | PayloadLength | Mean         | Error       | StdDev      |
|---------------- |-------------- |-------------:|------------:|------------:|
| **BenchmarkMethod** | **1000**          |     **719.8 μs** |     **2.05 μs** |     **1.60 μs** |
| **BenchmarkMethod** | **10000**         |   **7,129.7 μs** |    **11.41 μs** |    **10.67 μs** |
| **BenchmarkMethod** | **100000**        |  **70,909.9 μs** |   **122.12 μs** |   **108.25 μs** |
| **BenchmarkMethod** | **1000000**       | **711,240.6 μs** | **1,528.18 μs** | **1,354.69 μs** |
