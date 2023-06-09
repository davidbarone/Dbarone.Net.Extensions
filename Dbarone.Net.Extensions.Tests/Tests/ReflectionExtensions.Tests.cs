namespace Dbarone.Net.Extensions.Tests;
using Xunit;
using System;
using Dbarone.Net.Extensions;
using System.Collections;

public class ReflectionExtensionsTests
{
    [Theory]
    [InlineData("123", typeof(int), 123)]
    [InlineData("true", typeof(bool), true)]
    [InlineData("123.45", typeof(float), 123.45f)]
    public void TestParse(string input, Type type, object expected)
    {
        Assert.Equal(expected, type.Parse(input));
    }

    [Theory]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(int?), true)]
    [InlineData(typeof(string), false)] // string is not a nullable type. It is reference type, so allows nulls though.
    [InlineData(typeof(object), false)] // object is not a nullable type. It is reference type, so allows nulls though.
    public void TestIsNullable(Type type, bool expected)
    {
        Assert.Equal(expected, type.IsNullable());
    }

    [Theory]
    [InlineData(typeof(int), typeof(int?))]
    [InlineData(typeof(int?), typeof(int?))]        // Already nullable - nothing to do
    [InlineData(typeof(string), typeof(string))]    // String is reference type. Already nullable
    public void TestGetNullableType(Type type, Type expectedType)
    {
        Assert.Equal(expectedType, type.GetNullableType());
    }

    [Theory]
    [InlineData(typeof(int?), typeof(int))]
    [InlineData(typeof(int), null)]        // Already nullable - nothing to do
    [InlineData(typeof(string), null)]    // String is reference type. Already nullable
    public void TestGetNullableUnderlyingType(Type type, Type expectedType)
    {
        if (expectedType == null)
        {
            // Test where GetNullableUnderlyingType should return null value (i.e. not already nullable type)
            Assert.Null(type.GetNullableUnderlyingType());
        }
        else
        {
            // Normal test - Type is a nullable type - check the underlying types.
            Assert.Equal(expectedType, type.GetNullableUnderlyingType());
        }
    }


    [Theory]
    [InlineData("123", typeof(int), 123)]
    [InlineData("", typeof(int), null)]
    [InlineData("", typeof(int?), null)]        // Can use either nullable or non-nullable type.
    [InlineData(null, typeof(int), null)]
    public void TestParseNullable(string input, Type type, object expected)
    {
        Assert.Equal(expected, type.ParseNullable(input));
    }


    [Theory]
    [InlineData((int)123, typeof(long), true)]
    [InlineData("a string value", typeof(int), false)]
    [InlineData((long)long.MaxValue, typeof(int), false)]
    public void TestCanConvertTo(object obj, Type type, bool expected)
    {

        Assert.Equal(expected, obj.CanConvertTo(type));
    }

    [Theory]
    [InlineData(typeof(List<string>), new Type[] { typeof(string) })]
    [InlineData(typeof(Dictionary<string, object>), new Type[] { typeof(string), typeof(object) })]
    public void TestGetGenericTypes(Type genericType, Type[] expectedTypes)
    {
        var args = genericType.GetGenericArguments();
        Assert.Equal(expectedTypes, args);
    }

    [Theory]
    [InlineData(typeof(List<>), new Type[] { typeof(string) }, typeof(List<string>))]
    [InlineData(typeof(Dictionary<,>), new Type[] { typeof(string), typeof(object) }, typeof(Dictionary<string, object>))]
    public void TestMakeGenericType(Type genericType, Type[] typeArguments, Type expectedType)
    {
        var newType = genericType.MakeGenericType(typeArguments);
        Assert.Equal(expectedType, newType);
    }

    [Theory]
    [InlineData(typeof(List<string>), true)]
    [InlineData(typeof(int[]), true)]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(object), false )]
    [InlineData(typeof(Hashtable), false )]
    public void TestIsEnumerableType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsEnumerableType());
    }

    [Theory]
    [InlineData(typeof(List<string>), true)]
    [InlineData(typeof(int[]), true)]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(object), false )]
    [InlineData(typeof(Hashtable), true )]
    public void TestIsCollectionType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsCollectionType());
    }

   [Theory]
    [InlineData(typeof(Dictionary<string, object>), true)]
    [InlineData(typeof(Hashtable), true )]
    [InlineData(typeof(List<string>), false)]
    public void TestIsDictionaryType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsDictionaryType());
    }

    [Theory]
    [InlineData(typeof(bool), true)]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(object), true )]
    [InlineData(typeof(DateTime), false )]
    [InlineData(typeof(int[]), false)]
    public void TestIsBuiltinType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsBuiltInType());
    }

   [Theory]
    [InlineData(typeof(bool), "bool")]
    [InlineData(typeof(Boolean), "bool")]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(Int32), "int")]
    [InlineData(typeof(string), "string")]
    [InlineData(typeof(String), "string")]
    [InlineData(typeof(int[]), "int[]")]
    [InlineData(typeof(List<string>), "List<string>")]
    public void TestGetFriendlyName(Type testType, string expectedResult)
    {
        Assert.Equal(expectedResult, testType.GetFriendlyName());
    }
}