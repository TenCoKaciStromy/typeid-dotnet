using System;

namespace TcKs.TypeId; 

partial struct TypeId : IEquatable<TypeId> {
  /// <summary>
  /// Returns hash code of this typed id.
  /// </summary>
  /// <returns></returns>
  public override int GetHashCode()
    => text?.GetHashCode() ?? -1;

  /// <summary>
  /// Returns true if both this typed id and <paramref name="other"/> are equals.
  /// Otherwise returns false.
  /// </summary>
  /// <param name="other">
  /// The other typed id.
  /// </param>
  /// <returns></returns>
  public bool Equals(TypeId other)
    => text.Equals(other.text, StringComparison.Ordinal);

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