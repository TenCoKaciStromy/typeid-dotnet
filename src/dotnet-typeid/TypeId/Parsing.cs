using System;
using System.Linq;

namespace TcKs.TypeId; 

partial struct TypeId {
  /// <summary>
  /// The maximum length of prefix.
  /// </summary>
  public const int MaxPrefixLength = 63;
  
  /// <summary>
  /// The separator for type part and id part.
  /// </summary>
  public const char PartsSeparator = '_';
  
  /// <summary>
  /// The length of the id part.
  /// </summary>
  public const int IdPartLength = 26;
  
  /// <summary>
  /// The minimal lenght of parseable string.
  /// We support empty type. 
  /// </summary>
  public const int MinTotalLength = IdPartLength + 1;

  /// <summary>
  /// Allowed characters for type prefix.
  /// </summary>
  public const string AllowedPrefixCharacters = "abcdefghijklmnopqrstuvwxyz";

  /// <summary>
  /// Parses string to <see cref="TypeId"/>.
  /// </summary>
  /// <param name="input">
  /// String input to parse.
  /// </param>
  /// <returns>
  /// Parsed <see cref="TypeId"/>.
  /// </returns>
  /// <exception cref="FormatException">
  /// Thrown if <paramref name="input"/> can not be parsed.
  /// </exception>
  public static TypeId Parse(string input)
    => TryParse(input, out var result)
      ? result
      : throw new FormatException("Can not parse TypeId.");

  /// <summary>
  /// Returns true if <paramref name="input"/> is successfully parsed.
  /// Otherwise return true.
  /// </summary>
  /// <param name="input">
  /// String input to parse.
  /// </param>
  /// <param name="result">
  /// Contains result of parsing.
  /// If parsing fails, the default value will be there.
  /// </param>
  /// <returns>
  /// True if parsing was successfull.
  /// Otherwise false.
  /// </returns>
  public static bool TryParse(string input, out TypeId result)
    => TryParse(input.AsMemory(), out result);

  public static bool TryParse(ReadOnlyMemory<char> input, out TypeId result) {
    var inputLength = input.Length;
    if (inputLength < MinTotalLength)
      return Fail(out result);

    var inputSpan = input.Span;

    var ndxSeparator = inputSpan.IndexOf(PartsSeparator);
    if (ndxSeparator is < 0 or > MaxPrefixLength)
      return Fail(out result);

    var ndxId = ndxSeparator + 1;
    var idLength = inputLength - ndxId;
    if (idLength != IdPartLength)
      return Fail(out result);

    var prefix = input.Slice(0, ndxSeparator);
    var prefixSpan = prefix.Span;
    for (var i = prefixSpan.Length - 1; i >= 0; i--) {
      var ch = prefixSpan[i];
      if (ch is < 'a' or > 'z') {
        return Fail(out result);
      }
    }

    var idPart = input.Slice(ndxId).Span;
    
    // Check for overflow.
    if (idPart[0] is < '0' or > '7') {
      return Fail(out result);
    }

    Span<byte> idBytes = stackalloc byte[16];
    if (Base32.TryDecode(idPart, idBytes) is false)
      return Fail(out result);

    Span<byte> uuidBytes = stackalloc byte[16];
    SwapEndians(idBytes, uuidBytes);

    result = new(prefix, new(uuidBytes));
    return true;
    
    static bool Fail(out TypeId result) {
      result = default;
      return false;
    }
  }
}