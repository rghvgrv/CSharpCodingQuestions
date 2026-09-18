namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_10, "Majority Element (Boyer–Moore Voting)", Easy,
"Find the element that appears more than n/2 times, in O(n) time and O(1) space.")]
public static class MajorityElement
{
    // Pair off each majority vote against a different value; the majority always survives.
    public static int Solve(int[] nums)
    {
        int candidate = 0, count = 0;
        foreach (int x in nums)
        {
            if (count == 0) candidate = x;
            count += x == candidate ? 1 : -1;
        }
        return candidate;
    }

    public static void Run()
    {
        Check("[3,2,3]", Solve([3, 2, 3]), 3);
        Check("[2,2,1,1,1,2,2]", Solve([2, 2, 1, 1, 1, 2, 2]), 2);
    }
}
