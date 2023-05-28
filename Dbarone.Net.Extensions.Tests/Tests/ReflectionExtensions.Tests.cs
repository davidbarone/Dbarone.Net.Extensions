using Xunit;
using System;

namespace Dbarone.Net.Extensions.Tests;

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
}