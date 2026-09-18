namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 4, Title = "Combination Sum", Level = Medium, Problem = """
    Find all combinations of `candidates` that add up to `target`. Each number may be used **any number of times**.
    `candidates = [2, 3, 6, 7]`, `target = 7` → `[[2, 2, 3], [7]]`.
    """)]
public static class CombinationSum
{
    [Approach(Name = "Backtracking", Time = "Exponential", Space = "O(target) + output", Idea = """
        Sort the candidates. Try adding each candidate to the combination and recurse with a smaller `remaining` target:

        - `remaining == 0` → a valid combination; save a copy.
        - A candidate bigger than `remaining` → stop the loop (the rest are even bigger, since the list is sorted).

        Recursing from `i` (not `i + 1`) allows reusing the same number, and never going back to earlier numbers avoids duplicates like `[3, 2, 2]`.
        """)]
    public static List<List<int>> FindCombinations(int[] candidates, int target)
    {
        int[] sorted = (int[])candidates.Clone();
        Array.Sort(sorted);
        var result = new List<List<int>>();
        Search(sorted, target, 0, new List<int>(), result);
        return result;
    }

    private static void Search(int[] candidates, int remaining, int start, List<int> current, List<List<int>> result)
    {
        if (remaining == 0)
        {
            result.Add(new List<int>(current));
            return;
        }

        for (int i = start; i < candidates.Length && candidates[i] <= remaining; i++)
        {
            current.Add(candidates[i]);
            Search(candidates, remaining - candidates[i], i, current, result);
            current.RemoveAt(current.Count - 1);
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 2, 3, 6, 7 }, 7], new[] { new[] { 2, 2, 3 }, new[] { 7 } }),
        new([new[] { 2, 3, 5 }, 8], new[] { new[] { 2, 2, 2, 2 }, new[] { 2, 3, 3 }, new[] { 3, 5 } }),
        new([new[] { 2 }, 1], Array.Empty<int[]>()),
    ];
}
