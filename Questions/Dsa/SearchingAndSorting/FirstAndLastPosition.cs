namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_02, "First and Last Position (Lower / Upper Bound)", Medium,
"In a sorted array with duplicates, find the first and last index of target in O(log n). Return [-1, -1] if absent.")]
public static class FirstAndLastPosition
{
    // LowerBound = first index with a[i] >= target. First = LowerBound(t), Last = LowerBound(t + 1) - 1.
    static int LowerBound(int[] a, int target)
    {
        int lo = 0, hi = a.Length;
        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            if (a[mid] < target) lo = mid + 1; else hi = mid;
        }
        return lo;
    }

    public static int[] Solve(int[] a, int target)
    {
        int first = LowerBound(a, target);
        if (first == a.Length || a[first] != target) return [-1, -1];
        return [first, LowerBound(a, target + 1) - 1];
    }

    public static void Run()
    {
        Check("[5,7,7,8,8,10], 8", Solve([5, 7, 7, 8, 8, 10], 8), [3, 4]);
        Check("[5,7,7,8,8,10], 6", Solve([5, 7, 7, 8, 8, 10], 6), [-1, -1]);
        Check("[2,2,2,2], 2", Solve([2, 2, 2, 2], 2), [0, 3]);
        Check("[], 0", Solve([], 0), [-1, -1]);
    }
}
