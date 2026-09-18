namespace CSharpCodingQuestions.Questions.Dsa.Greedy;

[Question(Order = 3, Title = "Fractional Knapsack", Level = Easy, Problem = """
    Each item has a `value` and a `weight`. Your bag holds at most `capacity` kilos, and you may take **part** of an item
    (like pouring some rice). Maximize the total value.
    `values = [60, 100, 120]`, `weights = [10, 20, 30]`, `capacity = 50` → `240`.
    """)]
public static class FractionalKnapsack
{
    [Approach(Name = "Greedy: Best Value per Kilo First", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort items by `value / weight` (value per kilo), highest first. Take whole items while they fit,
        then take the fraction of the next item that fills the bag.

        This works **only because you can take fractions**. When items must be taken whole, greedy can fail;
        see *0/1 Knapsack* in Dynamic Programming.
        """)]
    public static double MaxValue(int[] values, int[] weights, int capacity)
    {
        int[] order = Enumerable.Range(0, values.Length)
            .OrderByDescending(i => (double)values[i] / weights[i])
            .ToArray();

        double total = 0;
        int spaceLeft = capacity;
        foreach (int i in order)
        {
            if (spaceLeft == 0)
            {
                break;
            }
            int take = Math.Min(weights[i], spaceLeft);
            total += (double)values[i] * take / weights[i];
            spaceLeft -= take;
        }
        return total;
    }

    public static Example[] Examples =>
    [
        new([new[] { 60, 100, 120 }, new[] { 10, 20, 30 }, 50], 240.0),
        new([new[] { 500 }, new[] { 30 }, 10], 166.66667),
    ];
}
