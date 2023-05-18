namespace Northwind.GraphQL;

public class Query
{
    public string GetGreeting() => "Hello GraphQL!";
    public string Farewell() => "Goodbye GraphQL!";
    public int RollTheDie() => Random.Shared.Next(1, 7);
}