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
    [InlineData("TheCatSatOnTheMat", CaseType.None, "TheCatSatOnTheMat")]
    [InlineData("theCatSatOnTheMat", CaseType.None, "theCatSatOnTheMat")]
    [InlineData("the_cat_sat_on_the_mat", CaseType.None, "the_cat_sat_on_the_mat")]
    public void TestChangeCase(string input, CaseType @case, string expected)
    {
        var actual = input.ChangeCase(@case);
    }

    [Theory]
    [InlineData(true, "%", "")]
    [InlineData(true, "%", " ")]
    [InlineData(true, "%", "asdfa asdf asdf")]
    [InlineData(true, "%", "%")]
    [InlineData(false, "_", "")]
    [InlineData(true, "_", " ")]
    [InlineData(true, "_", "4")]
    [InlineData(true, "_", "C")]
    [InlineData(false, "_", "CX")]
    [InlineData(false, "[ABCD]", "")]
    [InlineData(true, "[ABCD]", "A")]
    [InlineData(true, "[ABCD]", "b")]
    [InlineData(false, "[ABCD]", "X")]
    [InlineData(false, "[ABCD]", "AB")]
    [InlineData(true, "[B-D]", "C")]
    [InlineData(true, "[B-D]", "D")]
    [InlineData(false, "[B-D]", "A")]
    [InlineData(false, "[^B-D]", "C")]
    [InlineData(false, "[^B-D]", "D")]
    [InlineData(true, "[^B-D]", "A")]
    [InlineData(true, "%TEST[ABCD]XXX", "lolTESTBXXX")]
    [InlineData(false, "%TEST[ABCD]XXX", "lolTESTZXXX")]
    [InlineData(false, "%TEST[^ABCD]XXX", "lolTESTBXXX")]
    [InlineData(true, "%TEST[^ABCD]XXX", "lolTESTZXXX")]
    [InlineData(true, "%TEST[B-D]XXX", "lolTESTBXXX")]
    [InlineData(true, "%TEST[^B-D]XXX", "lolTESTZXXX")]
    [InlineData(true, "%Stuff.txt", "Stuff.txt")]
    [InlineData(true, "%Stuff.txt", "MagicStuff.txt")]
    [InlineData(false, "%Stuff.txt", "MagicStuff.txt.img")]
    [InlineData(false, "%Stuff.txt", "Stuff.txt.img")]
    [InlineData(false, "%Stuff.txt", "MagicStuff001.txt.img")]
    [InlineData(true, "Stuff.txt%", "Stuff.txt")]
    [InlineData(false, "Stuff.txt%", "MagicStuff.txt")]
    [InlineData(false, "Stuff.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "Stuff.txt%", "Stuff.txt.img")]
    [InlineData(false, "Stuff.txt%", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff.txt%", "Stuff.txt")]
    [InlineData(true, "%Stuff.txt%", "MagicStuff.txt")]
    [InlineData(true, "%Stuff.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "%Stuff.txt%", "Stuff.txt.img")]
    [InlineData(false, "%Stuff.txt%", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff%.txt", "Stuff.txt")]
    [InlineData(true, "%Stuff%.txt", "MagicStuff.txt")]
    [InlineData(false, "%Stuff%.txt", "MagicStuff.txt.img")]
    [InlineData(false, "%Stuff%.txt", "Stuff.txt.img")]
    [InlineData(false, "%Stuff%.txt", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff%.txt", "MagicStuff001.txt")]
    [InlineData(true, "Stuff%.txt%", "Stuff.txt")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff.txt")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "Stuff%.txt%", "Stuff.txt.img")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff001.txt.img")]
    [InlineData(false, "Stuff%.txt%", "MagicStuff001.txt")]
    [InlineData(true, "%Stuff%.txt%", "Stuff.txt")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff.txt")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff.txt.img")]
    [InlineData(true, "%Stuff%.txt%", "Stuff.txt.img")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff001.txt.img")]
    [InlineData(true, "%Stuff%.txt%", "MagicStuff001.txt")]
    [InlineData(true, "_Stuff_.txt_", "1Stuff3.txt4")]
    [InlineData(false, "_Stuff_.txt_", "1Stuff.txt4")]
    [InlineData(false, "_Stuff_.txt_", "1Stuff3.txt")]
    [InlineData(false, "_Stuff_.txt_", "Stuff3.txt4")]
    public void TestLike(bool expectedResult, string pattern, string input)
    {
        var result = input.Like(pattern);
        Assert.Equal(expectedResult, result);
    }
}