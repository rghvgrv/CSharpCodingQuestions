namespace CSharpCodingQuestions.Questions.Dsa.BitManipulation;

[Question(Order = 3, Title = "Find the Number That Appears Once", Level = Easy, Problem = """
    Every number appears twice, except one. Find that one. `[4, 1, 2, 1, 2]` → `4`.
    """)]
public static class SingleNumber
{
    [Approach(Name = "Count With a Dictionary", Time = "O(n)", Space = "O(n)", Idea = """
        Count each number, then return the one whose count is 1.
        """)]
    public static int SingleWithDictionary(int[] numbers)
    {
        var counts = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            counts[number] = counts.GetValueOrDefault(number) + 1;
        }
        foreach (var pair in counts)
        {
            if (pair.Value == 1)
            {
                return pair.Key;
            }
        }
        return -1;
    }

    [Approach(Name = "XOR Everything", Time = "O(n)", Space = "O(1)", Idea = """
        XOR (`^`) has two useful rules: `x ^ x = 0` and `x ^ 0 = x`, and the order doesn't matter.
        XOR all numbers together: every pair cancels itself out to 0, and only the single number is left.
        """)]
    public static int SingleWithXor(int[] numbers)
    {
        int result = 0;
        foreach (int number in numbers)
        {
            result ^= number;
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { 4, 1, 2, 1, 2 }], 4),
        new([new[] { 2, 2, 1 }], 1),
        new([new[] { 7 }], 7),
    ];
}
