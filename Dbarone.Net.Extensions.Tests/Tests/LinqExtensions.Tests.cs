using Xunit;
using System.Linq;
using System.Linq.Expressions;

namespace Dbarone.Net.Extensions.Tests;

public class LinqExtensionsTests
{
    public enum AddressType
    {
        Main,
        Business,
        Postal,
        Home
    }

    public class Country
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }

    public class Address
    {
        public AddressType AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public Country Country { get; set; }
    }

    public class Customer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Address Address { get; set; }
        public Address[] AlternateAddresses { get; set; }
    }

    [Fact]
    public void SingleMember()
    {
        // Arrange
        Expression<Func<Customer, object>> member = c => c.CustomerId;

        // Act
        var path = member.GetMemberPath();

        // Assert
        Assert.Equal("CustomerId", path);
    }

    [Fact]
    public void NestedMember()
    {
        // Arrange
        Expression<Func<Customer, object>> member = c => c.Address.City;

        // Act
        var path = member.GetMemberPath();

        // Assert
        Assert.Equal("Address.City", path);
    }

    [Fact]
    public void EnumMember()
    {
        // Arrange
        Expression<Func<Customer, object>> member = c => c.Address.AddressType;

        // Act
        var path = member.GetMemberPath();

        // Assert
        Assert.Equal("Address.AddressType", path);
    }
    [Fact]
    public void SelectMember()
    {
        // Arrange
        Expression<Func<Customer, object>> member = c => c.AlternateAddresses.Select(a=>a.AddressType);

        // Act
        var path = member.GetMemberPath();

        // Assert
        Assert.Equal("AlternateAddresses.AddressType", path);
    }
}