using System;
using System.Security.Cryptography;
using SimpleBase;

namespace TcKs.TypeId; 

/// <summary>
/// Represents typed id.
/// </summary>
public readonly partial struct TypeId {   
  private readonly string? type;
  private readonly Guid id;

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="type">The type of id.</param>
  /// <param name="id">The id.</param>
  public TypeId(string type, Guid id) {
    this.type = type;
    this.id = id;
  }
  
  /// <summary>
  /// Creates new unique id for specified <paramref name="type"/>.
  /// </summary>
  /// <param name="type">The type of id.</param>
  /// <returns>
  /// New unique typed id.
  /// </returns>
  public static TypeId NewId(string type)
    => new(type, Guid.NewGuid());

  /// <summary>
  /// The type of id.
  /// </summary>
  public string Type
    => type ?? string.Empty;

  /// <summary>
  /// The id.
  /// </summary>
  public Guid Id
    => id;

  /// <summary>
  /// Returns true if <see cref="Id"/> is empty (Guid.Empty).
  /// </summary>
  public bool IsEmpty
    => id == Guid.Empty;

  /// <summary>
  /// Returns string representation of this typed id.
  /// The <see cref="Id"/> is encoded as a 26-character string in base32 (using Crockford's alphabet in lowercase).
  /// </summary>
  /// <example>
  ///   user_2x4y6z8a0b1c2d3e4f5g6h7j8k
  ///   └──┘ └────────────────────────┘
  ///   type    uuid suffix (base32)
  /// </example>
  /// <returns>
  /// String representation of this typed id.
  /// </returns>
  public override string ToString() {
    var bytes = id.ToByteArray();
    var text = Base32.Crockford.Encode(bytes.AsSpan());

    return $"{Type}_{text}";
  }

  /// <summary>
  /// The type id can be casted to <see cref="Guid"/>.
  /// </summary>
  /// <param name="self"></param>
  /// <returns></returns>
  public static explicit operator Guid(TypeId self)
    => self.id;
}