namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_06, "Two Sum", Easy,
"Given an array nums and a target, return the indices of the two numbers that add up to target. Exactly one answer exists.")]
public static class TwoSum
{
    // Time O(n), Space O(n): remember each value's index, look up target - x as we go.
    public static int[] Solve(int[] nums, int target)
    {
        var seen = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (seen.TryGetValue(target - nums[i], out int j)) return [j, i];
            seen[nums[i]] = i;
        }
        return [];
    }

    public static void Run()
    {
        Check("Solve([2,7,11,15], 9)", Solve([2, 7, 11, 15], 9), [0, 1]);
        Check("Solve([3,2,4], 6)", Solve([3, 2, 4], 6), [1, 2]);
        Check("Solve([3,3], 6)", Solve([3, 3], 6), [0, 1]);
    }
}
