```

BenchmarkDotNet v0.15.8, Linux Ubuntu 24.04.4 LTS (Noble Numbat)
AMD EPYC 7763 2.62GHz, 1 CPU, 4 logical and 2 physical cores
.NET SDK 10.0.301
  [Host]     : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.9 (10.0.9, 10.0.926.27113), X64 RyuJIT x86-64-v3


```
| Method          | PayloadLength | Mean         | Error     | StdDev    |
|---------------- |-------------- |-------------:|----------:|----------:|
| **BenchmarkMethod** | **1000**          |     **660.4 μs** |   **3.25 μs** |   **2.88 μs** |
| **BenchmarkMethod** | **10000**         |   **6,798.6 μs** |   **6.43 μs** |   **5.70 μs** |
| **BenchmarkMethod** | **100000**        |  **65,599.2 μs** |  **97.32 μs** |  **86.27 μs** |
| **BenchmarkMethod** | **1000000**       | **654,985.5 μs** | **752.81 μs** | **667.35 μs** |
