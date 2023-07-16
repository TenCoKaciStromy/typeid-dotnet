using BenchmarkDotNet.Attributes;
using TcKs.TypeId;

namespace dotnet_typeid_benchmarks; 

[MemoryDiagnoser]
public class TypeIdBenchmarks {
  // [Params(1, 10, 100, 1_000, 10_000, 100_000, 1_000_000)]
  [Params(1_000_000)]
  public int Iterations;

  [Params(5, 10, 63)]
  public int PrefixLength;
    
  private string? prefix;
  
  private string[]? typeIdStrings;

  private static Random randomizer = new Random(42);

  static char Nextaz()
    => (char)randomizer.Next('a', 'z');

  static char Nextaz(int ignored)
    => Nextaz();
    
  static string NextRandomPrefix(int lenght)
    => string.Concat(Enumerable.Range(0, lenght).Select(Nextaz));

  [GlobalSetup]
  public void Setup() {
    prefix = NextRandomPrefix(PrefixLength);
    typeIdStrings = Enumerable.Range(0, Iterations)
      .Select(_ => TypeId.NewId(prefix).ToString())
      .ToArray();
  }

  // [Benchmark]
  // public TypeId NewId() {
  //   TypeId typeId = default;
  //   for (var i = 0; i < Iterations; i++) {
  //     typeId = TypeId.NewId(prefix!);
  //   }
  //
  //   return typeId;
  // }
  
  [Benchmark]
  public TypeId TryParse()
  {
    TypeId typeId = default;
  
    var arr = typeIdStrings!;
    for (var i = arr.Length - 1; i >= 0; i--) {
      TypeId.TryParse(arr[i], out typeId);
    }
        
    return typeId;
  }

  // [Benchmark]
  // public TypeId TryParseOne()
  // {
  //   TypeId typeId = default;
  //
  //   var str = typeIdStrings![0];
  //   var max = Iterations;
  //   for (var i = 0; i < max; i++) {
  //     TypeId.TryParse(str, out typeId);
  //   }
  //       
  //   return typeId;
  // }
}