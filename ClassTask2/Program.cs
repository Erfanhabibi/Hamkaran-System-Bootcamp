using System;
using System.Collections.Generic;
using System.Linq.Expressions;

public static class Program
{
    static void Main()
    {
        var queryString = new QueryApi<Invoice>("http://localhost/")
            .Filter(i => i.number == "ABC20")
            .Filter(i => i.Totalquantity > 20)
            .GetQueryString();

        Console.WriteLine(queryString);
    }
}

internal class QueryApi<T>
{
    private readonly List<string> filterParts = new List<string>();
    public string HostAddress { get; set; }

    public QueryApi(string hostAddress)
    {
        HostAddress = hostAddress;
    }

    public QueryApi<T> Filter(Expression<Func<T, bool>> predicate)
    {
        if (predicate.Body is BinaryExpression binary)
        {
            if (!(binary.Left is MemberExpression member))
                throw new NotSupportedException("Only simple member.");

            string propertyName = member.Member.Name;

            if (propertyName.Equals("number", StringComparison.OrdinalIgnoreCase))
                propertyName = "Number";


            string op = binary.NodeType switch
            {
                ExpressionType.Equal => "=",
                ExpressionType.GreaterThan => ">",
                _ => throw new NotSupportedException("Only == and >.")
            };


            if (!(binary.Right is ConstantExpression constant))
                throw new NotSupportedException("Only constant.");

            object value = constant.Value;

            if (propertyName == "Number" && value is string s && s.StartsWith("ABC"))
            {
                value = s.Substring(3);
            }

            filterParts.Add($"{propertyName}{op}{value}");
        }
        else
        {
            throw new NotSupportedException("Only binary.");
        }

        return this;
    }

    public string GetQueryString()
    {
        return $"{HostAddress}{typeof(T).Name}?{string.Join("&", filterParts)}";
    }
}

public class Invoice
{
    public int Totalquantity { get; set; }
    public string number { get; set; }
}