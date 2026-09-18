namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_08, "Insertion Sort", Easy,
"Sort an array with insertion sort: take each element and slide it left into its place in the sorted prefix, like sorting cards in your hand.")]
public static class InsertionSort
{
    // Time O(n²) worst, O(n) when nearly sorted. Space O(1). Stable.
    // .NET's Array.Sort switches to insertion sort for small ranges (≤ 16 items) because it's fast there.
    public static int[] Sort(int[] a)
    {
        for (int i = 1; i < a.Length; i++)
        {
            int key = a[i], j = i - 1;
            while (j >= 0 && a[j] > key)
            {
                a[j + 1] = a[j];
                j--;
            }
            a[j + 1] = key;
            Console.WriteLine($"  insert {key}: {Fmt(a)}");
        }
        return a;
    }

    public static void Run()
    {
        Check("Sort([12,11,13,5,6])", Sort([12, 11, 13, 5, 6]), [5, 6, 11, 12, 13]);
    }
}
