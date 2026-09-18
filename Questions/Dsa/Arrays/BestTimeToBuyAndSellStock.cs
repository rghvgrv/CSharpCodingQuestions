namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 8, Title = "Best Time to Buy and Sell Stock", Level = Easy, Problem = """
    `prices[i]` is a stock's price on day `i`. Buy on one day and sell on a **later** day.
    Return the biggest possible profit, or `0` if no trade makes money.
    `[7, 1, 5, 3, 6, 4]` → `5` (buy at 1, sell at 6).
    """)]
public static class BestTimeToBuyAndSellStock
{
    [Approach(Name = "Try Every Buy and Sell Day", Time = "O(n²)", Space = "O(1)", Idea = """
        For each buy day, try every later sell day and keep the best profit.
        """)]
    public static int MaxProfitBruteForce(int[] prices)
    {
        int bestProfit = 0;
        for (int buy = 0; buy < prices.Length; buy++)
        {
            for (int sell = buy + 1; sell < prices.Length; sell++)
            {
                bestProfit = Math.Max(bestProfit, prices[sell] - prices[buy]);
            }
        }
        return bestProfit;
    }

    [Approach(Name = "One Pass, Track the Cheapest Day", Time = "O(n)", Space = "O(1)", Idea = """
        If you sell today, the best buy day is the cheapest day **before** today.
        So walk through the days once, remembering the lowest price so far, and check `today - lowest` each day.
        """)]
    public static int MaxProfitOnePass(int[] prices)
    {
        int lowestPrice = int.MaxValue;
        int bestProfit = 0;
        foreach (int price in prices)
        {
            if (price < lowestPrice)
            {
                lowestPrice = price;
            }
            else if (price - lowestPrice > bestProfit)
            {
                bestProfit = price - lowestPrice;
            }
        }
        return bestProfit;
    }

    public static Example[] Examples =>
    [
        new([new[] { 7, 1, 5, 3, 6, 4 }], 5),
        new([new[] { 7, 6, 4, 3, 1 }], 0),
        new([new[] { 2, 4, 1, 7 }], 6),
    ];
}
