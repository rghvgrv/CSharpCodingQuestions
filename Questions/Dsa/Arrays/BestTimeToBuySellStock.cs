namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_07, "Best Time to Buy and Sell Stock", Easy,
"prices[i] is a stock's price on day i. Buy on one day and sell on a later day to get the maximum profit (0 if no profit is possible). Bonus: unlimited transactions.")]
public static class BestTimeToBuySellStock
{
    // Time O(n): track the cheapest price so far; profit = today - cheapest.
    public static int OneTransaction(int[] prices)
    {
        int minPrice = int.MaxValue, best = 0;
        foreach (int p in prices)
        {
            minPrice = Math.Min(minPrice, p);
            best = Math.Max(best, p - minPrice);
        }
        return best;
    }

    // Unlimited transactions: collect every upward step.
    public static int Unlimited(int[] prices)
    {
        int profit = 0;
        for (int i = 1; i < prices.Length; i++) profit += Math.Max(0, prices[i] - prices[i - 1]);
        return profit;
    }

    public static void Run()
    {
        Check("OneTransaction([7,1,5,3,6,4])", OneTransaction([7, 1, 5, 3, 6, 4]), 5);
        Check("OneTransaction([7,6,4,3,1])", OneTransaction([7, 6, 4, 3, 1]), 0);
        Check("Unlimited([7,1,5,3,6,4])", Unlimited([7, 1, 5, 3, 6, 4]), 7);
        Check("Unlimited([1,2,3,4,5])", Unlimited([1, 2, 3, 4, 5]), 4);
    }
}
