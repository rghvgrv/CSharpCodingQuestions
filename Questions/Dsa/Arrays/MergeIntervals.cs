namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 14, Title = "Merge Overlapping Intervals", Level = Medium, Problem = """
    Each interval is `[start, end]`. Merge all intervals that overlap.
    `[[1, 3], [2, 6], [8, 10]]` → `[[1, 6], [8, 10]]`, because `[1, 3]` and `[2, 6]` overlap.
    """)]
public static class MergeIntervals
{
    [Approach(Name = "Sort by Start, Then Merge", Time = "O(n log n)", Space = "O(n)", Idea = """
        After sorting by start, overlapping intervals sit next to each other.

        1. Sort the intervals by their start.
        2. Go through them in order. If an interval starts before the last merged one ends, they overlap: stretch the last one's end.
        3. Otherwise, it starts a new merged interval.
        """)]
    public static List<int[]> Merge(int[][] intervals)
    {
        int[][] sorted = intervals.OrderBy(interval => interval[0]).ToArray();
        var merged = new List<int[]>();

        foreach (int[] interval in sorted)
        {
            if (merged.Count > 0 && interval[0] <= merged[^1][1])
            {
                merged[^1][1] = Math.Max(merged[^1][1], interval[1]);
            }
            else
            {
                merged.Add(new int[] { interval[0], interval[1] });
            }
        }
        return merged;
    }

    public static Example[] Examples =>
    [
        new([new[] { new[] { 1, 3 }, new[] { 2, 6 }, new[] { 8, 10 }, new[] { 15, 18 } }], new[] { new[] { 1, 6 }, new[] { 8, 10 }, new[] { 15, 18 } }),
        new([new[] { new[] { 1, 4 }, new[] { 4, 5 } }], new[] { new[] { 1, 5 } }),
        new([new[] { new[] { 4, 7 }, new[] { 1, 4 }, new[] { 2, 3 } }], new[] { new[] { 1, 7 } }),
    ];
}
