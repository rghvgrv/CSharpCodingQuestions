namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 3, Title = "All Permutations", Level = Medium, Problem = """
    Return every possible ordering of the numbers (they are all different).
    `[1, 2, 3]` → `[1, 2, 3]`, `[1, 3, 2]`, `[2, 1, 3]`, `[2, 3, 1]`, `[3, 1, 2]`, `[3, 2, 1]`.
    """)]
public static class Permutations
{
    [Approach(Name = "Backtracking With a Used List", Time = "O(n · n!)", Space = "O(n) + output", Idea = """
        Build an ordering one position at a time. For each position, try every number that isn't used yet:
        mark it used, place it, recurse for the next position, then take it back out and unmark it.

        There are `n!` orderings (3! = 6, 10! = 3,628,800), so this grows extremely fast.
        """)]
    public static List<List<int>> Permute(int[] numbers)
    {
        var result = new List<List<int>>();
        Build(numbers, new bool[numbers.Length], new List<int>(), result);
        return result;
    }

    private static void Build(int[] numbers, bool[] used, List<int> current, List<List<int>> result)
    {
        if (current.Count == numbers.Length)
        {
            result.Add(new List<int>(current));
            return;
        }

        for (int i = 0; i < numbers.Length; i++)
        {
            if (used[i])
            {
                continue;
            }
            used[i] = true;
            current.Add(numbers[i]);
            Build(numbers, used, current, result);
            current.RemoveAt(current.Count - 1);
            used[i] = false;
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3 }], new[] { new[] { 1, 2, 3 }, new[] { 1, 3, 2 }, new[] { 2, 1, 3 }, new[] { 2, 3, 1 }, new[] { 3, 1, 2 }, new[] { 3, 2, 1 } }),
        new([new[] { 0, 1 }], new[] { new[] { 0, 1 }, new[] { 1, 0 } }),
    ];
}
