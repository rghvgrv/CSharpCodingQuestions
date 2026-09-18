namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_12, "Counting Sort & Radix Sort", Medium,
"Sort integers without comparisons. Counting sort for a small value range; radix sort (LSD) for large non-negative numbers.")]
public static class CountingSort
{
    // Time O(n + k) where k = value range. Beats O(n log n) when k is small.
    public static int[] Counting(int[] a)
    {
        if (a.Length == 0) return a;
        int min = a.Min(), max = a.Max();
        var count = new int[max - min + 1];
        foreach (int x in a) count[x - min]++;
        int i = 0;
        for (int v = 0; v < count.Length; v++)
            while (count[v]-- > 0) a[i++] = v + min;
        return a;
    }

    // Stable counting sort by each digit, least significant first. Time O(d · (n + 10)).
    public static int[] Radix(int[] a)
    {
        var output = new int[a.Length];
        for (long exp = 1; a.Length > 0 && a.Max() / exp > 0; exp *= 10)
        {
            var count = new int[10];
            foreach (int x in a) count[x / exp % 10]++;
            for (int d = 1; d < 10; d++) count[d] += count[d - 1]; // prefix sums = end positions
            for (int i = a.Length - 1; i >= 0; i--) output[--count[a[i] / exp % 10]] = a[i]; // backwards keeps it stable
            Array.Copy(output, a, a.Length);
        }
        return a;
    }

    public static void Run()
    {
        Check("Counting([4,2,-3,6,1,2])", Counting([4, 2, -3, 6, 1, 2]), [-3, 1, 2, 2, 4, 6]);
        Check("Radix([170,45,75,90,802,24,2,66])", Radix([170, 45, 75, 90, 802, 24, 2, 66]), [2, 24, 45, 66, 75, 90, 170, 802]);
    }
}
