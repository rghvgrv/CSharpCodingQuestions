namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 7, Title = "Two Sum", Level = Easy, Problem = """
    Find the two numbers that add up to `target` and return their **positions** (indexes).
    There is exactly one answer. `numbers = [2, 7, 11, 15]`, `target = 9` → `[0, 1]`, because `2 + 7 = 9`.
    """)]
public static class TwoSum
{
    [Approach(Name = "Check Every Pair", Time = "O(n²)", Space = "O(1)", Idea = """
        For each number, try adding it to every number after it. Stop when a pair adds up to `target`.
        With 10,000 numbers that's about 50 million pairs.
        """)]
    public static int[] TwoSumBruteForce(int[] numbers, int target)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            for (int j = i + 1; j < numbers.Length; j++)
            {
                if (numbers[i] + numbers[j] == target)
                {
                    return new int[] { i, j };
                }
            }
        }
        return new int[0];
    }

    [Approach(Name = "Hash Map", Time = "O(n)", Space = "O(n)", Idea = """
        For each number, the partner we need is `target - number`. Instead of searching for it with a second loop,
        remember every number we've passed, and its index, in a dictionary. Checking the dictionary is instant.

        `[2, 7, 11, 15]`, target 9: at 2 we need 7 (not seen yet), so save 2. At 7 we need 2, which is already saved at index 0. Answer `[0, 1]`.
        """)]
    public static int[] TwoSumHashMap(int[] numbers, int target)
    {
        var indexByNumber = new Dictionary<int, int>();
        for (int i = 0; i < numbers.Length; i++)
        {
            int needed = target - numbers[i];
            if (indexByNumber.ContainsKey(needed))
            {
                return new int[] { indexByNumber[needed], i };
            }
            indexByNumber[numbers[i]] = i;
        }
        return new int[0];
    }

    public static Example[] Examples =>
    [
        new([new[] { 2, 7, 11, 15 }, 9], new[] { 0, 1 }),
        new([new[] { 3, 2, 4 }, 6], new[] { 1, 2 }),
        new([new[] { 3, 3 }, 6], new[] { 0, 1 }),
    ];
}
