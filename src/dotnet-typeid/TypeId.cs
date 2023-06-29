using System;
using System.Security.Cryptography;
using SimpleBase;

namespace TcKs.TypeId; 

public readonly struct TypeId : IEquatable<TypeId> {
  public const char PartsSeparator = '_';
  public const int IdPartLength = 26;
  public const int MinTotalLength = IdPartLength + 1;
   
  private readonly string? type;
  private readonly Guid id;

  public TypeId(string type, Guid id) {
    this.type = type;
    this.id = id;
  }

  public string Type
    => type ?? string.Empty;

  public Guid Id
    => id;

  public bool IsEmpty
    => id == Guid.Empty;

  public override string ToString() {
    var bytes = id.ToByteArray();
    var text = Base32.Crockford.Encode(bytes.AsSpan());

    return $"{Type}_{text}";
  }

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

  public bool Equals(TypeId other)
    => id == other.id && (type ?? string.Empty) == (other.type ?? string.Empty);

  public override bool Equals(object obj)
    => obj is TypeId other ? Equals(other) : false;


  public static bool operator ==(TypeId a, TypeId b)
    => a.Equals(b);
    
  public static bool operator !=(TypeId a, TypeId b)
    => !a.Equals(b);

  public static TypeId NewId(string type)
    => new(type, Guid.NewGuid());

  public static TypeId Parse(string input)
    => TryParse(input, out var result)
      ? result
      : throw new FormatException("Can not parse TypeId.");

  public static bool TryParse(string input, out TypeId result) {
    if (input is null)
      return Fail(out result);

    var inputLength = input.Length;
    if (inputLength < MinTotalLength)
      return Fail(out result);

    var ndxSeparator = input.IndexOf(PartsSeparator);
    if (ndxSeparator < 0)
      return Fail(out result);

    var ndxId = ndxSeparator + 1;
    var idLength = inputLength - ndxId;
    if (idLength != IdPartLength)
      return Fail(out result);

    var idPart = input.AsMemory().Slice(ndxId).Span;
    var bytes = new byte[16];
    if (!Base32.Crockford.TryDecode(idPart, bytes.AsSpan(), out var numBytesWritten) || numBytesWritten != 16)
      return Fail(out result);

    result = new(input.Substring(0, ndxSeparator), new Guid(bytes));
    return true;
    
    static bool Fail(out TypeId result) {
      result = default;
      return false;
    }
  }
}