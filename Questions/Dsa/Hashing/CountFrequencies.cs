namespace CSharpCodingQuestions.Questions.Dsa.Hashing;

[Question(Order = 1, Title = "Count How Often Each Number Appears", Level = Easy, Problem = """
    Return how many times each number appears, in order of first appearance.
    `[1, 3, 2, 3, 4, 3, 1]` → `{1: 2, 3: 3, 2: 1, 4: 1}`.
    """)]
public static class CountFrequencies
{
    [Approach(Name = "Count With a Second Loop", Time = "O(n²)", Space = "O(n)", Idea = """
        For each number we haven't counted yet, scan the whole array to count it.
        """)]
    public static Dictionary<int, int> CountBruteForce(int[] numbers)
    {
        var counts = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            if (counts.ContainsKey(number))
            {
                continue;
            }

            int count = 0;
            foreach (int other in numbers)
            {
                if (other == number)
                {
                    count++;
                }
            }
            counts[number] = count;
        }
        return counts;
    }

    [Approach(Name = "One Pass With a Dictionary", Time = "O(n)", Space = "O(n)", Idea = """
        Walk the array once. For each number, add 1 to its count in the dictionary.
        `GetValueOrDefault` returns 0 for a number we haven't seen yet.
        """)]
    public static Dictionary<int, int> CountWithDictionary(int[] numbers)
    {
        var counts = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            counts[number] = counts.GetValueOrDefault(number) + 1;
        }
        return counts;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 3, 2, 3, 4, 3, 1 }], new Dictionary<int, int> { [1] = 2, [3] = 3, [2] = 1, [4] = 1 }),
        new([new[] { 7, 7 }], new Dictionary<int, int> { [7] = 2 }),
    ];
}
