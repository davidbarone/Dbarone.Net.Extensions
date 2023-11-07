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
    [InlineData(typeof(List<int>), typeof(List<>), true)]
    [InlineData(typeof(List<int>), typeof(IEnumerable<>), true)]
    [InlineData(typeof(List<int>), typeof(List<long>), false)]
    [InlineData(typeof(ICollection<int>), typeof(IList<int>), false)]
    public void TestIsAssignableToGenericType(Type testType, Type genericType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsAssignableToGenericType(genericType));
    }

    [Theory]
    [InlineData(typeof(List<string>), true)]
    [InlineData(typeof(int[]), true)]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(object), false)]
    [InlineData(typeof(Hashtable), true)]
    [InlineData(typeof(IEnumerable), true)]
    [InlineData(typeof(ArrayList), true)]
    public void TestIsEnumerableType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsEnumerableType());
    }

    [Theory]
    [InlineData(typeof(List<string>), typeof(string))]
    [InlineData(typeof(int[]), typeof(int))]
    [InlineData(typeof(string), typeof(char))]
    [InlineData(typeof(object), null)]
    [InlineData(typeof(Hashtable), typeof(object))]
    [InlineData(typeof(IEnumerable), typeof(object))]
    [InlineData(typeof(ArrayList), typeof(object))]
    public void TestGetElementType(Type testType, Type? expectedElementType)
    {
        var actualElementType = testType.GetEnumerableElementType();
        Assert.Equal(expectedElementType, actualElementType);
    }

    [Theory]
    [InlineData(typeof(List<string>), true)]
    [InlineData(typeof(int[]), true)]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(object), false)]
    [InlineData(typeof(Hashtable), true)]
    public void TestIsCollectionType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsCollectionType());
    }

    [Theory]
    [InlineData(typeof(Dictionary<string, object>), true)]
    [InlineData(typeof(Hashtable), true)]
    [InlineData(typeof(List<string>), false)]
    public void TestIsDictionaryType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsDictionaryType());
    }

    [Theory]
    [InlineData(typeof(bool), true)]
    [InlineData(typeof(int), true)]
    [InlineData(typeof(string), true)]
    [InlineData(typeof(object), true)]
    [InlineData(typeof(DateTime), false)]
    [InlineData(typeof(int[]), false)]
    public void TestIsBuiltinType(Type testType, bool expectedResult)
    {
        Assert.Equal(expectedResult, testType.IsBuiltInType());
    }

    [Theory]
    [InlineData(typeof(bool), "bool")]
    [InlineData(typeof(int), "int")]
    [InlineData(typeof(string), "string")]
    [InlineData(typeof(int[]), "int[]")]
    [InlineData(typeof(List<string>), "List<string>")]
    public void TestGetFriendlyName(Type testType, string expectedResult)
    {
        Assert.Equal(expectedResult, testType.GetFriendlyName());
    }

    [Fact]
    public void TestGetExtensionMethods()
    {
        IEnumerable<object> items = new List<object> { 1, 2, 3, 4, 5, 6 };
        var test = items as IEnumerable<int>;

        Assert.Null(test);  // we can just cast IEnumerable<object> -> IEnumerable<int>

        var type = (items as IEnumerable).GetType();
        var extensionMethods = (typeof(IEnumerable)).GetExtensionMethods();
        var castMethod = extensionMethods.First(m => m.Name == "Cast");
        var decType = castMethod.DeclaringType;

        // Line below should cast items to object which is essentially same as IEnumerable<int>, so
        // can be cast safely to int[].
        var results = ((IEnumerable<int>)castMethod.MakeGenericMethod(typeof(int)).Invoke(null, new object[] { items })).ToArray();

        Assert.Equal(typeof(int[]), results.GetType());
    }
}