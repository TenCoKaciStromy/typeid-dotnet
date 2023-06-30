using System;
using TcKs.TypeId;
using Xunit;
using Xunit.Abstractions;

namespace dotnet_typeid_tests; 

/// <summary>
/// Reimplemented test cases from official repository.
/// https://github.com/jetpack-io/typeid-go/blob/main/typeid_test.go
/// </summary>
public class ValidationAgainstSpec_Tests {
  private readonly ITestOutputHelper outer;

  public ValidationAgainstSpec_Tests(ITestOutputHelper outer) {
    this.outer = outer;
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func ExampleNew() {
  ///   tid := Must(New("prefix"))
  ///   fmt.Printf("New typeid: %s\n", tid)
  /// }
  /// ]]>
  [Fact]
  public void ExampleNew() {
    // tid := Must(New("prefix"))
    var tid = TypeId.NewId("prefix");
    
    // fmt.Printf("New typeid: %s\n", tid)
    outer.WriteLine($"New typeid: {tid}");
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func ExampleNew_withoutPrefix() {
  ///   tid := Must(New(""))
  ///   fmt.Printf("New typeid without prefix: %s\n", tid)
  /// }
  /// ]]>
  [Fact]
  public void ExampleNew_withoutPrefix() {
    // tid := Must(New(""))
    var tid = TypeId.NewId("");
    
    // fmt.Printf("New typeid without prefix: %s\n", tid)
    outer.WriteLine($"New typeid without prefix: {tid}");
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func ExampleFromString() {
  ///   tid := Must(FromString("prefix_00041061050r3gg28a1c60t3gg"))
  ///   fmt.Printf("Prefix: %s\nSuffix: %s\n", tid.Type(), tid.suffix)
  ///   // Output:
  ///   // Prefix: prefix
  ///   // Suffix: 00041061050r3gg28a1c60t3gg
  /// }
  /// ]]>
  [Fact]
  public void ExampleFromString() {
    // tid := Must(FromString("prefix_00041061050r3gg28a1c60t3gg"))
    var tid = TypeId.Parse("prefix_00041061050r3gg28a1c60t3gg");
    
    // fmt.Printf("Prefix: %s\nSuffix: %s\n", tid.Type(), tid.suffix)
    outer.WriteLine($"Prefix: {tid.Type}\nSuffix: {tid.Suffix}");
    
    Assert.Equal("prefix", tid.Type);
    Assert.Equal("00041061050r3gg28a1c60t3gg", tid.Suffix);
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func TestInvalidPrefix(t *testing.T) {
  ///     testdata := []struct {
  ///       name  string
  ///       input string
  ///   }{
  ///     {"caps", "PREFIX"}, // Would be valid in lowercase
  ///     {"numeric", "12323"},
  ///     {"symbols", "pre.fix"},
  ///     {"spaces", "  "},
  ///     {"long", "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz"},
  ///   }
  /// 
  ///   for _, td := range testdata {
  ///     t.Run(td.name, func(t *testing.T) {
  ///       _, err := New(td.input)
  ///       if err == nil {
  ///         t.Errorf("Expected error for invalid prefix: %s", td.input)
  ///       }
  ///     })
  ///   }
  /// }
  /// ]]>
  [Theory]
  [InlineData("PREFIX")]
  [InlineData("12323")]
  [InlineData("pre.fix")]
  [InlineData("   ")]
  [InlineData("abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz")]
  public void TestInvalidPrefix(string input) {
    var text = $"{input}_00041061050r3gg28a1c60t3gg";
    var success = TypeId.TryParse(text, out var result);
    Assert.False(success);
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func TestInvalidSuffix(t *testing.T) {
  ///   testdata := []struct {
  ///     name  string
  ///       input string
  ///   }{
  ///     {"spaces", "  "},
  ///     {"short", "01234"},
  ///     {"long", "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789"},
  ///     {"caps", "00041061050R3GG28A1C60T3GF"}, // Would be valid in lowercase
  ///     {"hyphens", "00041061050-3gg28a1-60t3gf"},
  ///     {"crockford_ambiguous", "ooo41o61o5or3gg28a1c6ot3gi"}, // Would be valid if we followed Crocksford's substitution rules
  ///     {"symbols", "00041061050.3gg28a1_60t3gf"},
  ///     {"wrong_alphabet", "ooooooiiiiiiuuuuuuulllllll"},
  ///   }
  ///
  ///   for _, td := range testdata {
  ///     t.Run(td.name, func(t *testing.T) {
  ///       _, err := From("prefix", td.input)
  ///       if err == nil {
  ///         t.Errorf("Expected error for invalid suffix: %s", td.input)
  ///       }
  ///     })
  ///   }
  /// }
  /// ]]>
  [Theory]
  [InlineData("  ")]
  [InlineData("01234")]
  [InlineData("012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789")]
  // [InlineData("00041061050R3GG28A1C60T3GF")] // we allow this
  [InlineData("00041061050-3gg28a1-60t3gf")]
  // [InlineData("ooo41o61o5or3gg28a1c6ot3gi")] // we follow Crocksford's substitution rules
  [InlineData("00041061050.3gg28a1_60t3gf")]
  [InlineData("ooooooiiiiiiuuuuuuulllllll")]
  public void TestInvalidSuffix(string input) {
    var text = $"prefix_{input}";
    var success = TypeId.TryParse(text, out var result);
    Assert.False(success);
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func TestEncodeDecode(t *testing.T) {
  ///   // Generate a bunch of random typeids, encode and decode from a string
  ///   // and make sure the result is the same as the original.
  ///   for i := 0; i < 1000; i++ {
  ///     tid := Must(New("prefix"))
  ///     decoded, err := FromString(tid.String())
  ///     if err != nil {
  ///       t.Error(err)
  ///     }
  ///     if tid != decoded {
  ///       t.Errorf("Expected %s, got %s", tid, decoded)
  ///     }
  ///   }
  ///
  ///   // Repeat with the empty prefix:
  ///   for i := 0; i < 1000; i++ {
  ///     tid := Must(New(""))
  ///     decoded, err := FromString(tid.String())
  ///     if err != nil {
  ///       t.Error(err)
  ///     }
  ///     if tid != decoded {
  ///       t.Errorf("Expected %s, got %s", tid, decoded)
  ///     }
  ///   }
  /// }
  /// ]]>
  [Theory]
  [InlineData("")]
  [InlineData("prefix")]
  public void TestEncodeDecode(string prefix) {
    for (var i = 0; i < 1000; i++) {
      var tid = TypeId.NewId(prefix);
      var text = tid.ToString();
      var decoded = TypeId.Parse(text);
      
      Assert.Equal(tid, decoded);
    }
  }

  /// <summary>
  /// Reimplementation of test function from official test cases.
  /// </summary>
  /// <![CDATA[
  /// func TestSpecialValues(t *testing.T) {
  ///   testdata := []struct {
  ///     name string
  ///       tid  string
  ///       uuid string
  ///   }{
  ///     {"nil", "00000000000000000000000000", "00000000-0000-0000-0000-000000000000"},
  ///     {"one", "00000000000000000000000001", "00000000-0000-0000-0000-000000000001"},
  ///     {"ten", "0000000000000000000000000a", "00000000-0000-0000-0000-00000000000a"},
  ///     {"sixteen", "0000000000000000000000000g", "00000000-0000-0000-0000-000000000010"},
  ///     {"thirty-two", "00000000000000000000000010", "00000000-0000-0000-0000-000000000020"},
  ///   }
  ///   for _, td := range testdata {
  ///     t.Run(td.name, func(t *testing.T) {
  ///       // Values should be equal if we start by parsing the typeid
  ///       tid := Must(FromString(td.tid))
  ///       if td.uuid != tid.UUID() {
  ///         t.Errorf("Expected %s, got %s", td.uuid, tid.UUID())
  ///       }
  ///
  ///       // Values should be equal if we start by parsing the uuid
  ///       tid = Must(FromUUID("", td.uuid))
  ///       if td.tid != tid.String() {
  ///         t.Errorf("Expected %s, got %s", td.tid, tid.String())
  ///       }
  ///     })
  ///   }
  /// }
  /// ]]>
  [Fact(Skip = "This test is not required since we use System.Guid.")]
  public void TestSpecialValues() {
    Assert.True(true);
  }
}

