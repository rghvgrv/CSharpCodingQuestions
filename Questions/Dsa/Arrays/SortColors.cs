namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_12, "Sort Colors (Dutch National Flag)", Medium,
"An array contains only 0s, 1s and 2s. Sort it in place in one pass without a sort function.")]
public static class SortColors
{
    // Three regions: [0, low) = 0s, [low, mid) = 1s, (high, end] = 2s. mid scans the unknown part.
    public static int[] Solve(int[] a)
    {
        int low = 0, mid = 0, high = a.Length - 1;
        while (mid <= high)
        {
            switch (a[mid])
            {
                case 0: (a[low], a[mid]) = (a[mid], a[low]); low++; mid++; break;
                case 1: mid++; break;
                default: (a[mid], a[high]) = (a[high], a[mid]); high--; break; // don't advance mid: swapped-in value is unchecked
            }
        }
        return a;
    }

    public static void Run()
    {
        Check("[2,0,2,1,1,0]", Solve([2, 0, 2, 1, 1, 0]), [0, 0, 1, 1, 2, 2]);
        Check("[2,0,1]", Solve([2, 0, 1]), [0, 1, 2]);
    }
}
