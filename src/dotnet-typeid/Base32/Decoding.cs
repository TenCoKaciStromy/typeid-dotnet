using System;
using System.Runtime.CompilerServices;

namespace TcKs.TypeId; 

partial class Base32 {
  public static byte[] Decode(string input)
    => TryDecode(input, out var output) is true && output is not null
      ? output
      : throw new FormatException("Input can not be decoded.");
      
  public static bool TryDecode(string input, out byte[]? output)
    => TryDecode(input.AsSpan(), out output);
  
  public static byte[] Decode(ReadOnlyMemory<char> input)
    => TryDecode(input, out var output) is true && output is not null
      ? output
      : throw new FormatException("Input can not be decoded.");
        
  public static bool TryDecode(ReadOnlyMemory<char> input, out byte[]? output)
    => TryDecode(input.Span, out output);

  public static byte[] Decode(ReadOnlySpan<char> input)
    => TryDecode(input, out var output) is true && output is not null
      ? output
      : throw new FormatException("Input can not be decoded.");
      
  public static bool TryDecode(ReadOnlySpan<char> input, out byte[]? output) {
    if (input.Length != 26)
      return Fail(out output);
      
    var inputBytes = new byte[26];
    if (System.Text.Encoding.UTF8.GetBytes(input, inputBytes.AsSpan()) != 26)
      return Fail(out output);

    if (!AllInputBytesAreOk(inputBytes))
      return Fail(out output);

    output = new byte[16];

    var v = inputBytes;
    var dec = DecBytes;
    
    // 6 bytes timestamp (48 bits)
    output[0] = (byte)((dec[v[0]] << 5) | dec[v[1]]);
    output[1] = (byte)((dec[v[2]] << 3) | (dec[v[3]] >> 2));
    output[2] = (byte)((dec[v[3]] << 6) | (dec[v[4]] << 1) | (dec[v[5]] >> 4));
    output[3] = (byte)((dec[v[5]] << 4) | (dec[v[6]] >> 1));
    output[4] = (byte)((dec[v[6]] << 7) | (dec[v[7]] << 2) | (dec[v[8]] >> 3));
    output[5] = (byte)((dec[v[8]] << 5) | dec[v[9]]);

    // 10 bytes of entropy (80 bits)
    output[6] = (byte)((dec[v[10]] << 3) | (dec[v[11]] >> 2)); // First 4 bits are the version
    output[7] = (byte)((dec[v[11]] << 6) | (dec[v[12]] << 1) | (dec[v[13]] >> 4));
    output[8] = (byte)((dec[v[13]] << 4) | (dec[v[14]] >> 1)); // First 2 bits are the variant
    output[9] = (byte)((dec[v[14]] << 7) | (dec[v[15]] << 2) | (dec[v[16]] >> 3));
    output[10] = (byte)((dec[v[16]] << 5) | dec[v[17]]);
    output[11] = (byte)((dec[v[18]] << 3) | dec[v[19]] >> 2);
    output[12] = (byte)((dec[v[19]] << 6) | (dec[v[20]] << 1) | (dec[v[21]] >> 4));
    output[13] = (byte)((dec[v[21]] << 4) | (dec[v[22]] >> 1));
    output[14] = (byte)((dec[v[22]] << 7) | (dec[v[23]] << 2) | (dec[v[24]] >> 3));
    output[15] = (byte)((dec[v[24]] << 5) | dec[v[25]]);

    return true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool AllInputBytesAreOk(byte[] inputBytes)
      => DecBytes is { } decBytes
         && decBytes[inputBytes[0]] != 0xFF
         && decBytes[inputBytes[1]] != 0xFF
         && decBytes[inputBytes[2]] != 0xFF
         && decBytes[inputBytes[3]] != 0xFF
         && decBytes[inputBytes[4]] != 0xFF
         && decBytes[inputBytes[5]] != 0xFF
         && decBytes[inputBytes[6]] != 0xFF
         && decBytes[inputBytes[7]] != 0xFF
         && decBytes[inputBytes[8]] != 0xFF
         && decBytes[inputBytes[9]] != 0xFF
         && decBytes[inputBytes[10]] != 0xFF
         && decBytes[inputBytes[11]] != 0xFF
         && decBytes[inputBytes[12]] != 0xFF
         && decBytes[inputBytes[13]] != 0xFF
         && decBytes[inputBytes[14]] != 0xFF
         && decBytes[inputBytes[15]] != 0xFF
         && decBytes[inputBytes[16]] != 0xFF
         && decBytes[inputBytes[17]] != 0xFF
         && decBytes[inputBytes[18]] != 0xFF
         && decBytes[inputBytes[19]] != 0xFF
         && decBytes[inputBytes[20]] != 0xFF
         && decBytes[inputBytes[21]] != 0xFF
         && decBytes[inputBytes[22]] != 0xFF
         && decBytes[inputBytes[23]] != 0xFF
         && decBytes[inputBytes[24]] != 0xFF
         && decBytes[inputBytes[25]] != 0xFF;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool Fail(out byte[]? output) {
      output = default!;
      return false;
    }
  }
}