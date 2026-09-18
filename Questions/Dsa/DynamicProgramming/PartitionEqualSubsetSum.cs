namespace CodingQuestions.Dsa.DynamicProgramming;

[Q(1_12_10, "Partition Equal Subset Sum", Medium,
"Can the array be split into two subsets with equal sums?")]
public static class PartitionEqualSubsetSum
{
    // Same as: is there a subset summing to total/2? That's 0/1 knapsack with booleans.
    // reachable[s] = some subset sums to s. Iterate s downward so each number is used once.
    public static bool Solve(int[] nums)
    {
        int total = nums.Sum();
        if (total % 2 == 1) return false;
        int target = total / 2;
        var reachable = new bool[target + 1];
        reachable[0] = true;
        foreach (int x in nums)
            for (int s = target; s >= x; s--)
                reachable[s] |= reachable[s - x];
        return reachable[target];
    }

    public static void Run()
    {
        Check("[1,5,11,5]", Solve([1, 5, 11, 5]), true);
        Check("[1,2,3,5]", Solve([1, 2, 3, 5]), false);
        Check("[2,2,3,5]", Solve([2, 2, 3, 5]), false);
    }
}
