namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_11, "Heap Sort", Medium,
"Sort in place using a binary max-heap: build the heap, then repeatedly move the max to the end and restore the heap.")]
public static class HeapSort
{
    // Time O(n log n) always, Space O(1). Not stable.
    // Heap stored in the array: children of i are 2i+1 and 2i+2.
    public static int[] Sort(int[] a)
    {
        int n = a.Length;
        for (int i = n / 2 - 1; i >= 0; i--) SiftDown(a, i, n); // build heap bottom-up, O(n)
        for (int end = n - 1; end > 0; end--)
        {
            (a[0], a[end]) = (a[end], a[0]); // largest goes to its final spot
            SiftDown(a, 0, end);
        }
        return a;
    }

    static void SiftDown(int[] a, int i, int size)
    {
        while (true)
        {
            int largest = i, l = 2 * i + 1, r = 2 * i + 2;
            if (l < size && a[l] > a[largest]) largest = l;
            if (r < size && a[r] > a[largest]) largest = r;
            if (largest == i) return;
            (a[i], a[largest]) = (a[largest], a[i]);
            i = largest;
        }
    }

    public static void Run()
    {
        Check("Sort([12,11,13,5,6,7])", Sort([12, 11, 13, 5, 6, 7]), [5, 6, 7, 11, 12, 13]);
        Check("Sort([4,10,3,5,1])", Sort([4, 10, 3, 5, 1]), [1, 3, 4, 5, 10]);
    }
}
