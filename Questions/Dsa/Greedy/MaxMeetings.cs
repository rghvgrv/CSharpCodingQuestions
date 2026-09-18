namespace CSharpCodingQuestions.Questions.Dsa.Greedy;

[Question(Order = 1, Title = "Most Meetings in One Room", Level = Medium, Problem = """
    Each meeting is `[start, end]`. One room can hold one meeting at a time (a meeting may start exactly when another ends).
    What is the largest number of meetings you can fit?
    `[[1, 2], [3, 4], [0, 6], [5, 7], [8, 9], [5, 9]]` → `4`.
    """)]
public static class MaxMeetings
{
    [Approach(Name = "Try Every Combination", Time = "O(2ⁿ · n log n)", Space = "O(n)", Idea = """
        Check every subset of meetings: sort the chosen ones by start and see whether any two overlap.
        Keep the biggest subset that fits. Correct, but hopeless beyond about 20 meetings.
        """)]
    public static int MaxMeetingsBruteForce(int[][] meetings)
    {
        int n = meetings.Length;
        int best = 0;
        for (int mask = 0; mask < (1 << n); mask++)
        {
            var chosen = new List<int[]>();
            for (int i = 0; i < n; i++)
            {
                if ((mask & (1 << i)) != 0)
                {
                    chosen.Add(meetings[i]);
                }
            }

            chosen.Sort((a, b) => a[0].CompareTo(b[0]));
            bool fits = true;
            for (int i = 1; i < chosen.Count; i++)
            {
                if (chosen[i][0] < chosen[i - 1][1])
                {
                    fits = false;
                    break;
                }
            }
            if (fits)
            {
                best = Math.Max(best, chosen.Count);
            }
        }
        return best;
    }

    [Approach(Name = "Greedy: Earliest End First", Time = "O(n log n)", Space = "O(n)", Idea = """
        Sort the meetings by **end** time. Take the first one, then every next meeting that starts after the last taken one ends.

        Why it works: the meeting that ends first leaves the room free as early as possible,
        so choosing it can never block more meetings than any other choice would.
        """)]
    public static int MaxMeetingsGreedy(int[][] meetings)
    {
        int[][] byEnd = meetings.OrderBy(meeting => meeting[1]).ToArray();
        int count = 0;
        int roomFreeAt = int.MinValue;

        foreach (int[] meeting in byEnd)
        {
            if (meeting[0] >= roomFreeAt)
            {
                count++;
                roomFreeAt = meeting[1];
            }
        }
        return count;
    }

    public static Example[] Examples =>
    [
        new([new[] { new[] { 1, 2 }, new[] { 3, 4 }, new[] { 0, 6 }, new[] { 5, 7 }, new[] { 8, 9 }, new[] { 5, 9 } }], 4),
        new([new[] { new[] { 1, 10 }, new[] { 2, 3 }, new[] { 4, 5 } }], 2),
    ];
}
