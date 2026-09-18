namespace CSharpCodingQuestions.Questions.Parallelism.TasksAndAsync;

[Question(Order = 3, Title = "Run Independent Calls Together (Task.WhenAll)", Level = Easy, Problem = """
    Fetch the prices of 5 products. Each call takes about 150 ms, and the calls don't depend on each other.
    """)]
public static class RunCallsTogether
{
    private static async Task<decimal> GetPriceAsync(string product)
    {
        await Task.Delay(150);   // simulated web call
        return product.Length * 1.5m;
    }

    [Approach(Name = "Await One by One", Idea = """
        `await` inside a loop waits for each call to finish **before starting the next**: about 5 × 150 ms.
        """)]
    public static async Task<List<decimal>> GetPricesOneByOne(string[] products)
    {
        var prices = new List<decimal>();
        foreach (string product in products)
        {
            prices.Add(await GetPriceAsync(product));
        }
        return prices;
    }

    [Approach(Name = "Start All, Then Task.WhenAll", Idea = """
        Start every call first (without `await`), collecting the tasks. Then `await Task.WhenAll(tasks)` waits for all of them together.
        Total time ≈ the slowest single call, about 150 ms. The results come back in the same order as the tasks.
        """)]
    public static async Task<List<decimal>> GetPricesTogether(string[] products)
    {
        var tasks = new List<Task<decimal>>();
        foreach (string product in products)
        {
            tasks.Add(GetPriceAsync(product));
        }
        decimal[] prices = await Task.WhenAll(tasks);
        return prices.ToList();
    }

    public static void Demo()
    {
        string[] products = ["apple", "banana", "cherry", "date", "fig"];

        var clock = Stopwatch.StartNew();
        List<decimal> oneByOne = GetPricesOneByOne(products).Result;
        long oneByOneMs = clock.ElapsedMilliseconds;

        clock.Restart();
        List<decimal> together = GetPricesTogether(products).Result;
        long togetherMs = clock.ElapsedMilliseconds;

        Print("One by one", $"{Formatter.Format(oneByOne)} in {oneByOneMs} ms");
        Print("Task.WhenAll", $"{Formatter.Format(together)} in {togetherMs} ms");
        Print("Same prices", oneByOne.SequenceEqual(together), expected: true);
        Print("WhenAll was faster", togetherMs < oneByOneMs, expected: true);
    }
}
