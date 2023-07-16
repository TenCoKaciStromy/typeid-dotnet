using BenchmarkDotNet.Attributes;

namespace dotnet_typeid_benchmarks; 

public class SwapEndiansBenchmarks {
  public static readonly Guid someGuid = new("7a8a996d-bd50-4e08-aedf-8795db15bdf0");

  [Params(1_000_000_000)]
  public long Iterations;

  [Benchmark]
  public void SwapEndians() {
    Span<byte> guid = someGuid.ToByteArray();
    Span<byte> output = stackalloc byte[16];

    for (var i = 0; i < Iterations; i++) {
      SwapEndians(guid, output);
    }
  }
  
  [Benchmark]
  public void SwapEndians_Slice() {
    Span<byte> guid = someGuid.ToByteArray();
    Span<byte> output = stackalloc byte[16];

    for (var i = 0; i < Iterations; i++) {
      SwapEndians_Slice(guid, output);
    }
  }
  
  [Benchmark]
  public void SwapEndians_Slice_2() {
    Span<byte> guid = someGuid.ToByteArray();
    Span<byte> output = stackalloc byte[16];

    for (var i = 0; i < Iterations; i++) {
      SwapEndians_Slice_2(guid, output);
    }
  }

  static void SwapEndians(Span<byte> guid, Span<byte> result) {
    guid.CopyTo(result);
    
    result[7] = guid[6];
    result[6] = guid[7];
    result[5] = guid[4];
    result[4] = guid[5];
    result[3] = guid[0];
    result[2] = guid[1];
    result[1] = guid[2];
    result[0] = guid[3];
  }
  
  static void SwapEndians_Slice(Span<byte> guid, Span<byte> result) {
    guid.Slice(8).CopyTo(result.Slice(8));
    
    result[7] = guid[6];
    result[6] = guid[7];
    result[5] = guid[4];
    result[4] = guid[5];
    result[3] = guid[0];
    result[2] = guid[1];
    result[1] = guid[2];
    result[0] = guid[3];
  }
  
  static void SwapEndians_Slice_2(Span<byte> guid, Span<byte> result) {
    guid.Slice(8).CopyTo(result.Slice(8));

    var lastNum = guid[7];
    result[7] = guid[6];
    result[6] = lastNum;
    result[5] = guid[4];
    result[4] = guid[5];
    result[3] = guid[0];
    result[2] = guid[1];
    result[1] = guid[2];
    result[0] = guid[3];
  }
}