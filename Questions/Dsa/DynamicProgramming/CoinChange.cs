namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 3, Title = "Coin Change (Fewest Coins)", Level = Medium, Problem = """
    Using coins of the given values (as many of each as you like), make exactly `amount` with the **fewest** coins.
    Return `-1` if it's impossible.
    `coins = [1, 2, 5]`, `amount = 11` → `3` (5 + 5 + 1).
    """)]
public static class CoinChange
{
    [Approach(Name = "Plain Recursion", Time = "Exponential", Space = "O(amount)", Idea = """
        Try each coin as the **last** coin: `Fewest(amount) = 1 + min(Fewest(amount - coin))` over all coins.
        `Fewest(0) = 0`, and a negative amount is impossible.

        Note: always taking the biggest coin (greedy) fails. For `[1, 3, 4]` and `6`, greedy gives 4+1+1, but 3+3 is better.
        """)]
    public static int FewestRecursive(int[] coins, int amount)
    {
        if (amount == 0)
        {
            return 0;
        }

        int best = -1;
        foreach (int coin in coins)
        {
            if (coin > amount)
            {
                continue;
            }
            int rest = FewestRecursive(coins, amount - coin);
            if (rest != -1 && (best == -1 || rest + 1 < best))
            {
                best = rest + 1;
            }
        }
        return best;
    }

    [Approach(Name = "Memoization", Time = "O(amount · coins)", Space = "O(amount)", Idea = """
        The only thing that changes between calls is `amount`, so there are just `amount + 1` different sub-problems.
        Store each answer and reuse it. `-2` marks "not computed yet".
        """)]
    public static int FewestMemoized(int[] coins, int amount)
    {
        int[] memory = new int[amount + 1];
        Array.Fill(memory, -2);
        return Fewest(coins, amount, memory);
    }

    private static int Fewest(int[] coins, int amount, int[] memory)
    {
        if (amount == 0)
        {
            return 0;
        }
        if (memory[amount] != -2)
        {
            return memory[amount];
        }

        int best = -1;
        foreach (int coin in coins)
        {
            if (coin > amount)
            {
                continue;
            }
            int rest = Fewest(coins, amount - coin, memory);
            if (rest != -1 && (best == -1 || rest + 1 < best))
            {
                best = rest + 1;
            }
        }
        memory[amount] = best;
        return best;
    }

    [Approach(Name = "Bottom-Up Table", Time = "O(amount · coins)", Space = "O(amount)", Idea = """
        Fill `fewest[a]` for every amount from 0 up to `amount`:
        `fewest[a] = 1 + min(fewest[a - coin])`. Smaller amounts are always filled first, so no recursion is needed.
        `amount + 1` is used as "impossible" because no answer can need that many coins.
        """)]
    public static int FewestBottomUp(int[] coins, int amount)
    {
        int impossible = amount + 1;
        int[] fewest = new int[amount + 1];
        Array.Fill(fewest, impossible);
        fewest[0] = 0;

        for (int a = 1; a <= amount; a++)
        {
            foreach (int coin in coins)
            {
                if (coin <= a)
                {
                    fewest[a] = Math.Min(fewest[a], fewest[a - coin] + 1);
                }
            }
        }
        return fewest[amount] == impossible ? -1 : fewest[amount];
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 5 }, 11], 3),
        new([new[] { 2 }, 3], -1),
        new([new[] { 1, 3, 4 }, 6], 2),
    ];
}
