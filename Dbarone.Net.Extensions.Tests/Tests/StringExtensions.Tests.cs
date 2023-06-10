using Xunit;
using Dbarone.Net.Extensions;

namespace Dbarone.Net.Extensions.Tests;

public class StringExtensionTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("Foo", "foo")]
    [InlineData("HelloWorld", "hello_world")]
    [InlineData("TheCatSatOnTheMat", "the_cat_sat_on_the_mat")]
    public void ToSnakeCaseTheory(string input, string expected)
    {
        Assert.Equal(expected, input.ToSnakeCase());
    }

    [Theory]
    [InlineData("TheCatSatOnTheMat", CaseType.CamelCase, "theCatSatOnTheMat")]
    [InlineData("TheCatSatOnTheMat", CaseType.SnakeCase, "the_cat_sat_on_the_mat")]
    [InlineData("the_cat_sat_on_the_mat", CaseType.CamelCase, "theCatSatOnTheMat")]
    [InlineData("the_cat_sat_on_the_mat", CaseType.PascalCase, "TheCatSatOnTheMat")]
    [InlineData("theCatSatOnTheMat", CaseType.PascalCase, "TheCatSatOnTheMat")]
    [InlineData("theCatSatOnTheMat", CaseType.SnakeCase, "the_cat_sat_on_the_mat")]
    public void a(string input, CaseType @case, string expected) {
        var actual = input.ChangeCase(@case);

    }
}