namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_03, "Coin Change (Min Coins & Number of Ways)", Medium,
"(1) Fewest coins that make up amount (-1 if impossible). (2) Number of combinations that make up amount. Unlimited coins of each type.")]
public static class CoinChange
{
    // dp[a] = fewest coins for amount a = 1 + min(dp[a - coin]) over all coins.
    public static int MinCoins(int[] coins, int amount)
    {
        var dp = new int[amount + 1];
        Array.Fill(dp, int.MaxValue);
        dp[0] = 0;
        for (int a = 1; a <= amount; a++)
            foreach (int c in coins)
                if (c <= a && dp[a - c] != int.MaxValue)
                    dp[a] = Math.Min(dp[a], dp[a - c] + 1);
        return dp[amount] == int.MaxValue ? -1 : dp[amount];
    }

    // Loop coins OUTSIDE so each combination is counted once (1+2 and 2+1 are the same).
    public static int Ways(int[] coins, int amount)
    {
        var dp = new int[amount + 1];
        dp[0] = 1;
        foreach (int c in coins)
            for (int a = c; a <= amount; a++) dp[a] += dp[a - c];
        return dp[amount];
    }

    public static void Run()
    {
        Check("MinCoins([1,2,5], 11)", MinCoins([1, 2, 5], 11), 3);
        Check("MinCoins([2], 3)", MinCoins([2], 3), -1);
        Check("MinCoins([1,3,4], 6) (greedy would give 3)", MinCoins([1, 3, 4], 6), 2);
        Check("Ways([1,2,5], 5)", Ways([1, 2, 5], 5), 4);
        Check("Ways([2], 3)", Ways([2], 3), 0);
    }
}
