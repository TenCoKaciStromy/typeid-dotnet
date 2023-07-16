using System;

namespace TcKs.TypeId; 

partial struct TypeId {
  /// <summary>
  /// Returns <see cref="Id"/> transformed to little-endian form for UUID standard.
  /// </summary>
  /// <returns></returns>
  public Guid GetUuid()
    => SwapEndians(Id);

  static Guid SwapEndians(Guid input)
    => new(SwapEndians(input.ToByteArray()));

  static byte[] SwapEndians(byte[] guid) {
    var result = new byte[16];
    result[0] = guid[3];
    result[1] = guid[2];
    result[2] = guid[1];
    result[3] = guid[0];
    result[4] = guid[5];
    result[5] = guid[4];
    result[6] = guid[7];
    result[7] = guid[6];
    Array.Copy(guid, 8, result, 8, 8);

    return result;
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
}