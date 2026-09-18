namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_07, "Selection Sort", Easy,
"Sort an array with selection sort: repeatedly select the minimum of the unsorted part and put it at the front.")]
public static class SelectionSort
{
    // Time O(n²) always, Space O(1). At most n-1 swaps (good when writes are expensive). Not stable.
    public static int[] Sort(int[] a)
    {
        for (int i = 0; i < a.Length - 1; i++)
        {
            int min = i;
            for (int j = i + 1; j < a.Length; j++)
                if (a[j] < a[min]) min = j;
            (a[i], a[min]) = (a[min], a[i]);
            Console.WriteLine($"  step {i + 1}: {Fmt(a)}");
        }
        return a;
    }

    public static void Run()
    {
        Check("Sort([64,25,12,22,11])", Sort([64, 25, 12, 22, 11]), [11, 12, 22, 25, 64]);
    }
}
