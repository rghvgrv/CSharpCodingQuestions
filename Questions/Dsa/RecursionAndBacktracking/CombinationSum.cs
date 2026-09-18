namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_04, "Combination Sum", Medium,
"Given distinct candidates and a target, return all unique combinations that sum to target. A number may be used unlimited times.")]
public static class CombinationSum
{
    // Recurse with `start` = i (not i + 1) so the same number can be reused. Sorting lets us stop early.
    public static List<List<int>> Solve(int[] candidates, int target)
    {
        Array.Sort(candidates);
        var result = new List<List<int>>();
        var current = new List<int>();
        void Backtrack(int start, int remaining)
        {
            if (remaining == 0) { result.Add([.. current]); return; }
            for (int i = start; i < candidates.Length && candidates[i] <= remaining; i++)
            {
                current.Add(candidates[i]);
                Backtrack(i, remaining - candidates[i]);
                current.RemoveAt(current.Count - 1);
            }
        }
        Backtrack(0, target);
        return result;
    }

    public static void Run()
    {
        Check("[2,3,6,7], target=7", Solve([2, 3, 6, 7], 7), [[2, 2, 3], [7]]);
        Check("[2,3,5], target=8", Solve([2, 3, 5], 8), [[2, 2, 2, 2], [2, 3, 3], [3, 5]]);
        Check("[2], target=1", Solve([2], 1), []);
    }
}
