partial class Program
{
    static async IAsyncEnumerable<string> GetStockAsync()
    {
        for (int i = 0; i < 10; i++)
        {
            // return a random four-letter stock code
            yield return $"{AtoZ()}{AtoZ()}{AtoZ()}{AtoZ()}";

            await Task.Delay(3000);
        }
    }

    static string AtoZ()
    {
        return char.ConvertFromUtf32(Random.Shared.Next(65, 91));
    }
}