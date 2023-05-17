// See https://aka.ms/new-console-template for more information
using System.Linq;

List<int> numbers = new() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

var subset = from obj in numbers
                            where obj < 5
                            select obj;

Console.WriteLine($"Values in subset: {subset.Count()}");
foreach (var i in subset)
{
    Console.WriteLine($"{i}");
}

var newSubSet = numbers.Where(e => e < 5).ToList();

foreach (var i in newSubSet)
{
    Console.WriteLine($"{i}");
}
Console.WriteLine("Hello, World!");