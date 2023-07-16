using System;
using System.Linq;
using System.Runtime.CompilerServices;

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
  public static bool TryParse(string input, out TypeId result) {
    var inputLength = input.Length;
    if (inputLength < MinTotalLength)
      return Fail(out result);

    var ndxSeparator = input.IndexOf(PartsSeparator);
    if (ndxSeparator is < 0 or > MaxPrefixLength)
      return Fail(out result);

    var ndxId = ndxSeparator + 1;
    var idLength = inputLength - ndxId;
    if (idLength != IdPartLength)
      return Fail(out result);
      
    // checking characters in type
    for (var i = ndxSeparator - 1; i >= 0; i--) {
      if (input[i] is < 'a' or > 'z')
        return Fail(out result);
    }
    
    var ndxId0 = ndxSeparator + 1;
    // checking id bytes overflow
    if (input[ndxId0] is not '0' and not '7')
      return Fail(out result);
      
    // checking characters in id
    if (!Base32.CanDecode(input.AsSpan(ndxId0)))
      return Fail(out result);

    result = new(input, (byte)ndxSeparator);
    return true;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool Fail(out TypeId result) {
      result = default;
      return false;
    }
  }
}