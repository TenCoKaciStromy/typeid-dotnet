// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using dotnet_typeid_benchmarks;

BenchmarkRunner.Run<TypeIdBenchmarks>();
// BenchmarkRunner.Run<SwapEndiansBenchmarks>();
