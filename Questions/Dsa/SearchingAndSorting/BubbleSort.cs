namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_06, "Bubble Sort", Easy,
"Sort an array with bubble sort: repeatedly swap adjacent elements that are out of order. Stop early if a pass makes no swaps.")]
public static class BubbleSort
{
    // Time O(n²) worst, O(n) on already-sorted input (early exit). Space O(1). Stable.
    public static int[] Sort(int[] a)
    {
        for (int pass = 0; pass < a.Length - 1; pass++)
        {
            bool swapped = false;
            for (int i = 0; i < a.Length - 1 - pass; i++) // the last `pass` items are already in place
            {
                if (a[i] > a[i + 1])
                {
                    (a[i], a[i + 1]) = (a[i + 1], a[i]);
                    swapped = true;
                }
            }
            Console.WriteLine($"  after pass {pass + 1}: {Fmt(a)}");
            if (!swapped) break;
        }
        return a;
    }

    public static void Run()
    {
        Check("Sort([5,1,4,2,8])", Sort([5, 1, 4, 2, 8]), [1, 2, 4, 5, 8]);
    }
}
