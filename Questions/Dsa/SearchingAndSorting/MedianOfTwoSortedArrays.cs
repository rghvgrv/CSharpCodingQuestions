namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_13, "Median of Two Sorted Arrays", Hard,
"Find the median of two sorted arrays in O(log(min(m, n))).")]
public static class MedianOfTwoSortedArrays
{
    // Binary search a cut in the smaller array A (i items taken from A, j = half - i from B)
    // so that every left-side item <= every right-side item: A[i-1] <= B[j] and B[j-1] <= A[i].
    public static double Solve(int[] a, int[] b)
    {
        if (a.Length > b.Length) (a, b) = (b, a);
        int m = a.Length, n = b.Length, half = (m + n + 1) / 2;
        int lo = 0, hi = m;
        while (true)
        {
            int i = (lo + hi) / 2, j = half - i;
            int aLeft = i > 0 ? a[i - 1] : int.MinValue, aRight = i < m ? a[i] : int.MaxValue;
            int bLeft = j > 0 ? b[j - 1] : int.MinValue, bRight = j < n ? b[j] : int.MaxValue;

            if (aLeft > bRight) hi = i - 1;      // took too many from A
            else if (bLeft > aRight) lo = i + 1; // took too few from A
            else
                return (m + n) % 2 == 1
                    ? Math.Max(aLeft, bLeft)
                    : (Math.Max(aLeft, bLeft) + (double)Math.Min(aRight, bRight)) / 2;
        }
    }

    public static void Run()
    {
        Check("[1,3] + [2]", Solve([1, 3], [2]), 2.0);
        Check("[1,2] + [3,4]", Solve([1, 2], [3, 4]), 2.5);
        Check("[] + [1]", Solve([], [1]), 1.0);
        Check("[1,5,9,12] + [2,3,4,20,21]", Solve([1, 5, 9, 12], [2, 3, 4, 20, 21]), 5.0);
    }
}
