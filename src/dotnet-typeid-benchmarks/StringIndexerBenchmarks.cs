using BenchmarkDotNet.Attributes;

namespace dotnet_typeid_benchmarks; 

public class StringIndexerBenchmarks {
  public static readonly string SomeString = "prefix_01h2xcejqtf2nbrexx3vqjhp41";

  [Params(100_000_000)]
  public long Iterations;

  [Benchmark]
  public void IndexerString() {
    var str = SomeString;
    for (var i = 0; i < Iterations; i++) {
      for (var k = 0; k < str.Length; k++) {
        _ = str[k];
      }
    }
  }
  
  [Benchmark]
  public void ReverseIndexerString() {
    var str = SomeString;
    for (var i = 0; i < Iterations; i++) {
      for (var k = str.Length - 1; k >= 0; k--) {
        _ = str[k];
      }
    }
  }
  
  [Benchmark]
  public void IndexerSpan() {
    var str = SomeString.AsSpan();
    for (var i = 0; i < Iterations; i++) {
      for (var k = 0; k < str.Length; k++) {
        _ = str[k];
      }
    }
  }
  
  [Benchmark]
  public void ReverseIndexerSpan() {
    var str = SomeString.AsSpan();
    for (var i = 0; i < Iterations; i++) {
      for (var k = str.Length - 1; k >= 0; k--) {
        _ = str[k];
      }
    }
  }
}