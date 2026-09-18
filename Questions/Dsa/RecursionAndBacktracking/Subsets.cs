namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_02, "Subsets (Power Set)", Medium,
"Return all possible subsets of a set of distinct numbers. Then handle input with duplicates without returning duplicate subsets.")]
public static class Subsets
{
    // Backtracking template: choose → explore → un-choose. Every node of the recursion tree is a subset. O(n · 2ⁿ)
    public static List<List<int>> Solve(int[] nums)
    {
        var result = new List<List<int>>();
        var current = new List<int>();
        void Backtrack(int start)
        {
            result.Add([.. current]);
            for (int i = start; i < nums.Length; i++)
            {
                current.Add(nums[i]);
                Backtrack(i + 1);
                current.RemoveAt(current.Count - 1);
            }
        }
        Backtrack(0);
        return result;
    }

    // With duplicates: sort, and at each level skip a value equal to the one just tried.
    public static List<List<int>> WithDuplicates(int[] nums)
    {
        Array.Sort(nums);
        var result = new List<List<int>>();
        var current = new List<int>();
        void Backtrack(int start)
        {
            result.Add([.. current]);
            for (int i = start; i < nums.Length; i++)
            {
                if (i > start && nums[i] == nums[i - 1]) continue;
                current.Add(nums[i]);
                Backtrack(i + 1);
                current.RemoveAt(current.Count - 1);
            }
        }
        Backtrack(0);
        return result;
    }

    public static void Run()
    {
        Check("[1,2,3]", Solve([1, 2, 3]), [[], [1], [1, 2], [1, 2, 3], [1, 3], [2], [2, 3], [3]]);
        Check("WithDuplicates([1,2,2])", WithDuplicates([1, 2, 2]), [[], [1], [1, 2], [1, 2, 2], [2], [2, 2]]);
    }
}
