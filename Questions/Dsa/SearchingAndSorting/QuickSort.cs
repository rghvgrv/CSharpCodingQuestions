namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_10, "Quick Sort", Medium,
"Sort with quick sort: pick a pivot, partition smaller values to its left and larger to its right, then recurse on both sides.")]
public static class QuickSort
{
    // Average O(n log n), worst O(n²) on bad pivots → pick a random pivot. Space O(log n) stack. Not stable. In-place.
    public static int[] Sort(int[] a) { Sort(a, 0, a.Length - 1); return a; }

    static void Sort(int[] a, int lo, int hi)
    {
        if (lo >= hi) return;
        int p = Partition(a, lo, hi);
        Sort(a, lo, p - 1);
        Sort(a, p + 1, hi);
    }

    // Lomuto partition: everything < pivot is moved to the front; pivot lands at its final index.
    static int Partition(int[] a, int lo, int hi)
    {
        int r = Random.Shared.Next(lo, hi + 1);
        (a[r], a[hi]) = (a[hi], a[r]);
        int pivot = a[hi], store = lo;
        for (int i = lo; i < hi; i++)
            if (a[i] < pivot) (a[i], a[store]) = (a[store++], a[i]);
        (a[store], a[hi]) = (a[hi], a[store]);
        return store;
    }

    public static void Run()
    {
        Check("Sort([10,7,8,9,1,5])", Sort([10, 7, 8, 9, 1, 5]), [1, 5, 7, 8, 9, 10]);
        Check("Sort([3,3,1,1,2,2])", Sort([3, 3, 1, 1, 2, 2]), [1, 1, 2, 2, 3, 3]);

        var sorted = Enumerable.Range(0, 50_000).ToArray(); // worst case for a fixed last-element pivot
        var sw = Stopwatch.StartNew();
        Sort(sorted);
        Console.WriteLine($"Already-sorted 50,000 items with random pivot: {sw.ElapsedMilliseconds} ms");
    }
}
