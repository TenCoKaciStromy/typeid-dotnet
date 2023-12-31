using System;
using System.Diagnostics.CodeAnalysis;
using TcKs.TypeId;
using UUIDNext;
using Xunit;

namespace dotnet_typeid_tests;

public class TypeId_Test {
  [Fact]
  public void DefaultIsEmpty() {
    TypeId value = default;
    
    Assert.True(value.IsEmpty);
    Assert.Equal(0, value.Type.Length);
    Assert.Equal(Guid.Empty, value.Id);
  }

  [Fact]
  public void ConstructorWorks() {
    var id = Uuid.NewSequential();
    var value = new TypeId("user", id);
    
    Assert.Equal("user", value.Type);
    Assert.Equal(id, value.Id);
    Assert.False(value.IsEmpty);
  }

  [Fact]
  public void ParseWorks() {
    var tid = TypeId.Parse("prefix_01h2xcejqtf2nbrexx3vqjhp41");
    
    Assert.Equal("prefix", tid.Type);
    Assert.Equal("01h2xcejqtf2nbrexx3vqjhp41", tid.Suffix);
    Assert.Equal(Guid.Parse("0188bac7-4afa-78aa-bc3b-bd1eef28d881"), tid.Id);
  }

  [Fact]
  public void CanFormatAndParse() {
    var id = Uuid.NewDatabaseFriendly(Database.PostgreSql);
  
    var value0 = new TypeId("user", id);
    var text0 = value0.ToString();
  
    var value1 = TypeId.Parse(text0);
    Assert.Equal(value0.Type, value1.Type);
    Assert.Equal(value0.Id, value1.Id);
  }

  [Fact]
  public void ParsingWillFailWhen128BitOverflow() {
    Assert.False(TypeId.TryParse("prefix_80041061050r3gg28a1c60t3gg", out _));
    Assert.False(TypeId.TryParse("prefix_8zzzzzzzzzzzzzzzzzzzzzzzzz", out _));
  }

  [Fact]
  [SuppressMessage("ReSharper", "EqualExpressionComparison")]
  public void EqualsWorks() {
    var user0 = TypeId.NewId("user");
    var user1 = TypeId.NewId("user");
    
    Assert.Equal(user0, user0);
    #pragma warning disable CS1718
    Assert.True(user0 == user0);
    Assert.False(user0 != user0);
    #pragma warning restore CS1718
    
    Assert.NotEqual(user0, user1);
    Assert.False(user0 == user1);
    Assert.True(user0 != user1);
    
    var product0 = new TypeId("product", user0.Id);
    
    Assert.NotEqual(user0, product0);
    Assert.False(user0 == product0);
    Assert.True(user0 != product0);
  }
}