namespace CodingQuestions.Dsa.Greedy;

[Q(1_11_03, "Fractional Knapsack", Easy,
"Items have a value and a weight. You may take fractions of items. Maximize total value within the capacity.")]
public static class FractionalKnapsack
{
    // Greedy by value per unit weight works because fractions are allowed.
    // (For whole items only, greedy fails; see 0/1 Knapsack in Dynamic Programming.)
    public static double Solve((int Value, int Weight)[] items, int capacity)
    {
        double total = 0;
        foreach (var (value, weight) in items.OrderByDescending(i => (double)i.Value / i.Weight))
        {
            if (capacity == 0) break;
            int take = Math.Min(weight, capacity);
            total += (double)value * take / weight;
            capacity -= take;
        }
        return total;
    }

    public static void Run()
    {
        Check("[(60,10),(100,20),(120,30)], W=50", Solve([(60, 10), (100, 20), (120, 30)], 50), 240.0);
        Check("[(500,30)], W=10", Solve([(500, 30)], 10), 166.66667);
    }
}
