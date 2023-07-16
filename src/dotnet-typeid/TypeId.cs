using System;

namespace TcKs.TypeId; 

/// <summary>
/// Represents typed id.
/// </summary>
public readonly partial struct TypeId {
  private readonly byte separatorIndex;
  private readonly string text;

  private TypeId(string text, byte separatorIndex) {
    this.text = text;
    this.separatorIndex = separatorIndex;
  }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="type">The type of id.</param>
  /// <param name="id">The id.</param>
  public TypeId(ReadOnlyMemory<char> type, Guid id) {
    Span<byte> idBytes = stackalloc byte[16];
    if (!id.TryWriteBytes(idBytes)) {
      throw new FormatException("Can not format id.");
    }

    Span<byte> uuidBytes = stackalloc byte[16];
    SwapEndians(idBytes, uuidBytes);

    var suffix = Base32.Encode(uuidBytes);
    text = $"{type}_{suffix}";
    separatorIndex = (byte)type.Length;
  }

  /// <summary>
  /// Constructor.
  /// </summary>
  /// <param name="type">The type of id.</param>
  /// <param name="id">The id.</param>
  public TypeId(string type, Guid id) : this(type.AsMemory(), id) { }

  /// <summary>
  /// Creates new unique id for specified <paramref name="type"/>.
  /// </summary>
  /// <param name="type">The type of id.</param>
  /// <returns>
  /// New unique typed id.
  /// </returns>
  public static TypeId NewId(string type)
    => new(type, UUIDNext.Uuid.NewSequential());

  /// <summary>
  /// Returns slice of memory with characters of type of id.
  /// </summary>
  public ReadOnlyMemory<char> GetTypeChars()
    => text?.AsMemory().Slice(0, separatorIndex) ?? default;

  /// <summary>
  /// The type of id.
  /// </summary>
  public string Type
    => text?.Substring(0, separatorIndex) ?? string.Empty;

  /// <summary>
  /// The id.
  /// </summary>
  public Guid Id {
    get {
      var str = text;
      if (string.IsNullOrEmpty(str)) {
        return Guid.Empty;
      }

      var suffix = str.AsSpan(separatorIndex + 1);
      Span<byte> uuidBytes = stackalloc byte[16];
      Base32.Decode(suffix, uuidBytes);

      Span<byte> guidBytes = stackalloc byte[16];
      SwapEndians(uuidBytes, guidBytes);

      return new(guidBytes);
    }
  }

  /// <summary>
  /// The id encoded as a 26-character string in base32 (using Crockford's alphabet in lowercase).
  /// </summary>
  public string Suffix
    => text?.Substring(separatorIndex + 1) ?? string.Empty;

  public ReadOnlyMemory<char> SuffixChars
    => text?.AsMemory().Slice(separatorIndex + 1) ?? default;

  /// <summary>
  /// Returns true if <see cref="Id"/> is empty (Guid.Empty).
  /// </summary>
  public bool IsEmpty
    => string.IsNullOrEmpty(text) || Id == Guid.Empty;

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
  public override string ToString()
    => text ?? string.Empty;

  /// <summary>
  /// The type id can be casted to <see cref="Guid"/>.
  /// </summary>
  /// <param name="self"></param>
  /// <returns></returns>
  public static explicit operator Guid(TypeId self)
    => self.Id;
}