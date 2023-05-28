using Xunit;
using System.Linq;

namespace Dbarone.Net.Extensions.Tests;

public class CollectionExtensionsTests
{
    [Fact]
    public void UnionTest()
    {
        // Arrange
        string a = "The";
        string[] b = new string[] { "cat", "sat", "on", "the" };
        string c = "mat.";

        // Act
        var results = a.Union(b, c);


        // Assert
        Assert.Equal(6, results.Count());
    }
}