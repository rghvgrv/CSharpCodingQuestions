namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 2, Title = "All Subsets", Level = Medium, Problem = """
    Return every subset of the numbers, including the empty one and the full one.
    `[1, 2, 3]` has 2³ = 8 subsets: `[]`, `[1]`, `[1, 2]`, `[1, 2, 3]`, `[1, 3]`, `[2]`, `[2, 3]`, `[3]`.
    """)]
public static class Subsets
{
    [Approach(Name = "Build Up One Number at a Time", Time = "O(n · 2ⁿ)", Space = "O(n · 2ⁿ)", Idea = """
        Start with just the empty subset. For each number, copy every subset you have so far and add the number to the copy.
        After each number the count doubles: 1 → 2 → 4 → 8.
        """)]
    public static List<List<int>> SubsetsIterative(int[] numbers)
    {
        var result = new List<List<int>> { new List<int>() };
        foreach (int number in numbers)
        {
            int existing = result.Count;
            for (int i = 0; i < existing; i++)
            {
                var withNumber = new List<int>(result[i]) { number };
                result.Add(withNumber);
            }
        }
        return result;
    }

    [Approach(Name = "Backtracking", Time = "O(n · 2ⁿ)", Space = "O(n) + output", Idea = """
        The general recipe for "list all combinations": **choose → explore → un-choose**.

        Every call saves the current subset. Then for each remaining number, add it (choose),
        recurse to add more (explore), and remove it again (un-choose) before trying the next one.
        The same template solves permutations, combination sum, N-Queens and more.
        """)]
    public static List<List<int>> SubsetsBacktracking(int[] numbers)
    {
        var result = new List<List<int>>();
        Backtrack(numbers, 0, new List<int>(), result);
        return result;
    }

    private static void Backtrack(int[] numbers, int start, List<int> current, List<List<int>> result)
    {
        result.Add(new List<int>(current));   // save a copy
        for (int i = start; i < numbers.Length; i++)
        {
            current.Add(numbers[i]);               // choose
            Backtrack(numbers, i + 1, current, result);   // explore
            current.RemoveAt(current.Count - 1);   // un-choose
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3 }], new[] { new int[0], new[] { 1 }, new[] { 1, 2 }, new[] { 1, 2, 3 }, new[] { 1, 3 }, new[] { 2 }, new[] { 2, 3 }, new[] { 3 } }, AnyOrder: true),
        new([new[] { 0 }], new[] { new int[0], new[] { 0 } }, AnyOrder: true),
    ];
}
