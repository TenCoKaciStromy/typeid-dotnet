using System;
using System.Security.Cryptography;
using SimpleBase;

namespace TcKs.TypeId; 

partial struct TypeId : IEquatable<TypeId> {
  /// <summary>
  /// Returns hash code of this typed id.
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode() {
    unchecked {
      const uint baseCode = 2166136261;
      const int nextCode = 16777619;

      var typeCode = type?.GetHashCode() ?? 1;
      if (typeCode == 0) typeCode = 1;
      typeCode *= nextCode;

      var idCode = id.GetHashCode();
      if (idCode == 0) idCode = 1;
      idCode *= nextCode;

      return (int)(baseCode * typeCode * idCode);
    }
  }

  /// <summary>
  /// Returns true if both this typed id and <paramref name="other"/> are equals.
  /// Otherwise returns false.
  /// </summary>
  /// <param name="other">
  /// The other typed id.
  /// </param>
  /// <returns></returns>
  public bool Equals(TypeId other)
    => id == other.id && (type ?? string.Empty) == (other.type ?? string.Empty);

  /// <summary>
  /// Returns true if both this typed id and <paramref name="obj"/> are equals.
  /// Otherwise returns false.
  /// </summary>
  /// <param name="obj">
  /// The other typed id.
  /// </param>
  /// <returns></returns>
  public override bool Equals(object? obj)
    => obj is TypeId other && Equals(other);

  /// <summary>
  /// Returns true if <paramref name="a"/> and <paramref name="b"/> are equal.
  /// Otherwise returns false.
  /// </summary>
  /// <param name="a">Left operand.</param>
  /// <param name="b">Right operand.</param>
  /// <returns></returns>
  public static bool operator ==(TypeId a, TypeId b)
    => a.Equals(b);
    
  /// <summary>
  /// Returns false if <paramref name="a"/> and <paramref name="b"/> are equal.
  /// Otherwise returns true.
  /// </summary>
  /// <param name="a">Left operand.</param>
  /// <param name="b">Right operand.</param>
  /// <returns></returns>
  public static bool operator !=(TypeId a, TypeId b)
    => !a.Equals(b);
}