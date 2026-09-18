namespace CodingQuestions.Dsa.Hashing;

[Q(1_04_02, "Contains Duplicate", Easy,
"Return true if any value appears at least twice. Bonus: return true if two equal values are at most k indices apart.")]
public static class ContainsDuplicate
{
    // HashSet.Add returns false if the value was already there. Time O(n)
    public static bool Solve(int[] nums)
    {
        var seen = new HashSet<int>();
        foreach (int x in nums)
            if (!seen.Add(x)) return true;
        return false;
    }

    // Keep a set of only the last k values (a sliding window).
    public static bool WithinK(int[] nums, int k)
    {
        var window = new HashSet<int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (!window.Add(nums[i])) return true;
            if (window.Count > k) window.Remove(nums[i - k]);
        }
        return false;
    }

    public static void Run()
    {
        Check("[1,2,3,1]", Solve([1, 2, 3, 1]), true);
        Check("[1,2,3,4]", Solve([1, 2, 3, 4]), false);
        Check("WithinK([1,2,3,1], 3)", WithinK([1, 2, 3, 1], 3), true);
        Check("WithinK([1,2,3,1,2,3], 2)", WithinK([1, 2, 3, 1, 2, 3], 2), false);
    }
}
