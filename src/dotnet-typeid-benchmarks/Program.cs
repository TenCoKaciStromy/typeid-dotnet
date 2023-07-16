// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Running;
using dotnet_typeid_benchmarks;

BenchmarkRunner.Run<TypeIdBenchmarks>();
// BenchmarkRunner.Run<StringIndexerBenchmarks>();
// BenchmarkRunner.Run<CheckAlphabetBenchmarks>();
// BenchmarkRunner.Run<SwapEndiansBenchmarks>();
