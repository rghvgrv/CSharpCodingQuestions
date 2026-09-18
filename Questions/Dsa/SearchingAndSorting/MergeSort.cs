namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_09, "Merge Sort", Medium,
"Sort with merge sort: split the array in half, sort each half recursively, then merge the two sorted halves. Also count inversions (pairs i < j with a[i] > a[j]) during the merge.")]
public static class MergeSort
{
    // Time O(n log n) always, Space O(n). Stable. Good for linked lists and external sorting.
    public static long Sort(int[] a) => Sort(a, new int[a.Length], 0, a.Length - 1);

    // Returns the inversion count as a bonus: when right[j] goes before left[i], it jumps over every remaining left item.
    static long Sort(int[] a, int[] tmp, int lo, int hi)
    {
        if (lo >= hi) return 0;
        int mid = lo + (hi - lo) / 2;
        long inversions = Sort(a, tmp, lo, mid) + Sort(a, tmp, mid + 1, hi);

        int i = lo, j = mid + 1, k = lo;
        while (i <= mid && j <= hi)
        {
            if (a[i] <= a[j]) tmp[k++] = a[i++];
            else
            {
                inversions += mid - i + 1;
                tmp[k++] = a[j++];
            }
        }
        while (i <= mid) tmp[k++] = a[i++];
        while (j <= hi) tmp[k++] = a[j++];
        Array.Copy(tmp, lo, a, lo, hi - lo + 1);
        return inversions;
    }

    public static void Run()
    {
        int[] a = [38, 27, 43, 3, 9, 82, 10];
        long inv = Sort(a);
        Check("Sort([38,27,43,3,9,82,10])", a, [3, 9, 10, 27, 38, 43, 82]);

        int[] b = [2, 4, 1, 3, 5];
        Check("Inversions in [2,4,1,3,5]", Sort(b), 3L);

        var rnd = new Random(42);
        var big = Enumerable.Range(0, 100_000).Select(_ => rnd.Next()).ToArray();
        var expected = big.Order().ToArray();
        Sort(big);
        Check("100,000 random ints sorted", big.SequenceEqual(expected), true);
    }
}
