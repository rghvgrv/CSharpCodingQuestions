namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_04, "Find Peak Element", Medium,
"A peak is an element strictly greater than its neighbors (outside the array counts as -∞). Return the index of any peak in O(log n).")]
public static class FindPeakElement
{
    // If a[mid] < a[mid+1] we're on an upward slope, so a peak must exist to the right. Otherwise one exists at mid or to the left.
    public static int Solve(int[] a)
    {
        int lo = 0, hi = a.Length - 1;
        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            if (a[mid] < a[mid + 1]) lo = mid + 1; else hi = mid;
        }
        return lo;
    }

    public static void Run()
    {
        Check("[1,2,3,1]", Solve([1, 2, 3, 1]), 2);
        Check("[1,2,1,3,5,6,4]", Solve([1, 2, 1, 3, 5, 6, 4]), 5);
        Check("[5,4,3]", Solve([5, 4, 3]), 0);
    }
}
