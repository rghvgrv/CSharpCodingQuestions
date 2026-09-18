namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_13, "Merge Intervals", Medium,
"Given a list of intervals [start, end], merge all overlapping intervals.")]
public static class MergeIntervals
{
    // Time O(n log n): sort by start, then either extend the last merged interval or start a new one.
    public static List<int[]> Solve(int[][] intervals)
    {
        var merged = new List<int[]>();
        foreach (var cur in intervals.OrderBy(i => i[0]))
        {
            if (merged.Count > 0 && cur[0] <= merged[^1][1])
                merged[^1][1] = Math.Max(merged[^1][1], cur[1]);
            else
                merged.Add([cur[0], cur[1]]);
        }
        return merged;
    }

    public static void Run()
    {
        Check("[[1,3],[2,6],[8,10],[15,18]]", Solve([[1, 3], [2, 6], [8, 10], [15, 18]]), [[1, 6], [8, 10], [15, 18]]);
        Check("[[1,4],[4,5]]", Solve([[1, 4], [4, 5]]), [[1, 5]]);
        Check("[[4,7],[1,4],[2,3]]", Solve([[4, 7], [1, 4], [2, 3]]), [[1, 7]]);
    }
}
