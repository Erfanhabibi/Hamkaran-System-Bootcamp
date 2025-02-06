using System;
using System.Collections.Generic;
using System.Linq;

public static class Program
{
    public static void Main(string[] args)
    {
        // Sample data
        IEnumerable<int> data = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        // Paging through the data with a page size of 3
        foreach (var page in data.ToPage(2))
        {
            Console.WriteLine("Page:");
            foreach (var item in page)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("------");
        }
    }
}

// Extension method for paging any IEnumerable<T>
public static class PagingExtensions
{
    public static IEnumerable<IEnumerable<T>> ToPage<T>(this IEnumerable<T> source, int pageSize)
    {
        if (pageSize <= 0)
            throw new ArgumentException("Page size must be greater than zero.");


        // var list = source.ToList();
        for (int pageIndex = 0; pageIndex * pageSize < source.Count(); pageIndex++)
        {
            yield return source.Skip(pageIndex * pageSize).Take(pageSize);
        }
    }
}