using System;
using System.Security.Cryptography;
using SimpleBase;

namespace TcKs.TypeId; 

partial struct TypeId {
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