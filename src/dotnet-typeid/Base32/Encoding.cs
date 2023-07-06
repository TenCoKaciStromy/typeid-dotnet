using System;
using System.Runtime.CompilerServices;

namespace TcKs.TypeId;

partial class Base32 {
  public static string Encode(byte[] input)
    => TryEncode(input, out var output) is true && output is not null
      ? output
      : throw new FormatException("Input was can not be encoded.");
      
  public static bool TryEncode(byte[] input, out string? output)
    => TryEncode(input.AsSpan(), out output);

  public static string Encode(ReadOnlyMemory<byte> input)
    => TryEncode(input, out var output) is true && output is not null
      ? output
      : throw new FormatException("Input was can not be encoded.");
      
  public static bool TryEncode(ReadOnlyMemory<byte> input, out string? output)
    => TryEncode(input.Span, out output);

  public static string Encode(ReadOnlySpan<byte> input)
    => TryEncode(input, out var output) is true && output is not null
      ? output
      : throw new FormatException("Input was can not be encoded.");
      
  public static bool TryEncode(ReadOnlySpan<byte> input, out string? output) {
    if (input.Length != 16)
      return Fail(out output);

    var alphabet = Alphabet;
    var src = input;
    var dst = new char[26];

    // 10 byte timestamp
    dst[0] = alphabet[(src[0] & 224) >> 5];
    dst[1] = alphabet[src[0] & 31];
    dst[2] = alphabet[(src[1] & 248) >> 3];
    dst[3] = alphabet[((src[1] & 7) << 2) | ((src[2] & 192) >> 6)];
    dst[4] = alphabet[(src[2] & 62) >> 1];
    dst[5] = alphabet[((src[2] & 1) << 4) | ((src[3] & 240) >> 4)];
    dst[6] = alphabet[((src[3] & 15) << 1) | ((src[4] & 128) >> 7)];
    dst[7] = alphabet[(src[4] & 124) >> 2];
    dst[8] = alphabet[((src[4] & 3) << 3) | ((src[5] & 224) >> 5)];
    dst[9] = alphabet[src[5] & 31];

    // 16 bytes of entropy
    dst[10] = alphabet[(src[6] & 248) >> 3];
    dst[11] = alphabet[((src[6] & 7) << 2) | ((src[7] & 192) >> 6)];
    dst[12] = alphabet[(src[7] & 62) >> 1];
    dst[13] = alphabet[((src[7] & 1) << 4) | ((src[8] & 240) >> 4)];
    dst[14] = alphabet[((src[8] & 15) << 1) | ((src[9] & 128) >> 7)];
    dst[15] = alphabet[(src[9] & 124) >> 2];
    dst[16] = alphabet[((src[9] & 3) << 3) | ((src[10] & 224) >> 5)];
    dst[17] = alphabet[src[10] & 31];
    dst[18] = alphabet[(src[11] & 248) >> 3];
    dst[19] = alphabet[((src[11] & 7) << 2) | ((src[12] & 192) >> 6)];
    dst[20] = alphabet[(src[12] & 62) >> 1];
    dst[21] = alphabet[((src[12] & 1) << 4) | ((src[13] & 240) >> 4)];
    dst[22] = alphabet[((src[13] & 15) << 1) | ((src[14] & 128) >> 7)];
    dst[23] = alphabet[(src[14] & 124) >> 2];
    dst[24] = alphabet[((src[14] & 3) << 3) | ((src[15] & 224) >> 5)];
    dst[25] = alphabet[src[15] & 31];

    output = new(dst);
    return true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool Fail(out string? output) {
      output = default!;
      return false;
    }
  }
}