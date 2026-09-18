namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 11, Title = "Majority Element", Level = Easy, Problem = """
    One value appears in **more than half** of the positions. Find it.
    `[2, 2, 1, 1, 1, 2, 2]` → `2`.
    """)]
public static class MajorityElement
{
    [Approach(Name = "Count Each Value", Time = "O(n²)", Space = "O(1)", Idea = """
        For each value, count how many times it appears by scanning the whole array. Return the one with more than `n / 2`.
        """)]
    public static int MajorityByCounting(int[] numbers)
    {
        foreach (int candidate in numbers)
        {
            int count = 0;
            foreach (int number in numbers)
            {
                if (number == candidate)
                {
                    count++;
                }
            }
            if (count > numbers.Length / 2)
            {
                return candidate;
            }
        }
        return -1;
    }

    [Approach(Name = "Dictionary of Counts", Time = "O(n)", Space = "O(n)", Idea = """
        Count every value in one pass using a dictionary (`value → count`), and stop as soon as a count passes `n / 2`.
        """)]
    public static int MajorityWithDictionary(int[] numbers)
    {
        var countByValue = new Dictionary<int, int>();
        foreach (int number in numbers)
        {
            countByValue[number] = countByValue.GetValueOrDefault(number) + 1;
            if (countByValue[number] > numbers.Length / 2)
            {
                return number;
            }
        }
        return -1;
    }

    [Approach(Name = "Boyer–Moore Voting", Time = "O(n)", Space = "O(1)", Idea = """
        Think of it as an election. Keep one `candidate` and a `count`:

        - Same as the candidate → `count + 1`.
        - Different → `count - 1` (one vote cancels one vote).
        - When `count` drops to 0, the next number becomes the new candidate.

        The majority has more votes than all the others combined, so it's always the one left standing.
        """)]
    public static int MajorityByVoting(int[] numbers)
    {
        int candidate = 0;
        int count = 0;
        foreach (int number in numbers)
        {
            if (count == 0)
            {
                candidate = number;
            }
            count += number == candidate ? 1 : -1;
        }
        return candidate;
    }

    public static Example[] Examples =>
    [
        new([new[] { 3, 2, 3 }], 3),
        new([new[] { 2, 2, 1, 1, 1, 2, 2 }], 2),
    ];
}
