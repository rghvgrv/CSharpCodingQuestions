namespace CodingQuestions.Dsa.Hashing;

[Q(1_04_03, "Longest Consecutive Sequence", Medium,
"Given an unsorted array, find the length of the longest run of consecutive integers in O(n). Example: [100,4,200,1,3,2] → 4 (1,2,3,4).")]
public static class LongestConsecutiveSequence
{
    // Only start counting at x when x-1 is missing (x starts a run). Each number is visited at most twice.
    public static int Solve(int[] nums)
    {
        var set = new HashSet<int>(nums);
        int best = 0;
        foreach (int x in set)
        {
            if (set.Contains(x - 1)) continue;
            int length = 1;
            while (set.Contains(x + length)) length++;
            best = Math.Max(best, length);
        }
        return best;
    }

    public static void Run()
    {
        Check("[100,4,200,1,3,2]", Solve([100, 4, 200, 1, 3, 2]), 4);
        Check("[0,3,7,2,5,8,4,6,0,1]", Solve([0, 3, 7, 2, 5, 8, 4, 6, 0, 1]), 9);
        Check("[]", Solve([]), 0);
    }
}
