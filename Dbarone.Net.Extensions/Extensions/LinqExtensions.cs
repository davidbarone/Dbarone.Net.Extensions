namespace Dbarone.Net.Extensions;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

/// <summary>
/// Extension methods related to Linq and expressions.
/// </summary>
public static class LinqExtensions
{
    // more dirty as possible: removing ".Select(x => x." sentence
    private static Regex _removeSelect = new Regex(@"\.Select\s*\(\s*\w+\s*=>\s*\w+\.", RegexOptions.Compiled);
    private static Regex _removeList = new Regex(@"\.get_Item\(\d+\)", RegexOptions.Compiled);
    private static Regex _removeArray = new Regex(@"\[\d+\]", RegexOptions.Compiled);

    /// <summary>
    /// Converts a lambda member expression to a path string.
    /// Used to convert lambda expressions like x => x.CustomerId to "CustomerId".
    /// Supports multiple levels: x => x.Customer.Address.
    /// Supports list levels: x => x.Addresses.Select(z => z.StreetName).
    /// Based on http://stackoverflow.com/questions/671968/retrieving-property-name-from-lambda-expression and
    /// litedb project implementation.
    /// </summary>
    public static string GetMemberPath(this Expression expr)
    {
        // enum properties seem to get compiled with Convert(prop, Int32) wrapper calls on Mono 5.0+
        // this causes path extraction code below to fail, since a clean "x.y.z" string is expected
        // thus we strip out any Converts found, using a loop in case there are nested Convert expressions
        while (expr.NodeType == ExpressionType.Convert || expr.NodeType == ExpressionType.ConvertChecked)
        {
            expr = ((UnaryExpression)expr).Operand;
        }

        // if is a method call, get first
        while (expr.NodeType == ExpressionType.Lambda)
        {
            if (((LambdaExpression)expr).Body is UnaryExpression unary)
            {
                expr = unary.Operand;
            }
            else
            {
                break;
            }
        }

        var str = expr.ToString(); // gives you: "o => o.Whatever"
        var firstDelim = str.IndexOf('.'); // make sure there is a beginning property indicator; the "." in "o.Whatever" -- this may not be necessary?

        var path = firstDelim < 0 ? str : str.Substring(firstDelim + 1).TrimEnd(')');

        path = _removeList.Replace(path, "");
        path = _removeArray.Replace(path, "");
        path = _removeSelect.Replace(path, ".")
            .Replace(")", "");

        return path;
    }
}
