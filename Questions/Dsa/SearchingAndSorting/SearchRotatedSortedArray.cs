namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_03, "Search in Rotated Sorted Array", Medium,
"A sorted array of distinct values was rotated at an unknown pivot, e.g. [4,5,6,7,0,1,2]. Find target in O(log n).")]
public static class SearchRotatedSortedArray
{
    // At every mid, one half is sorted. Check whether target lies inside that sorted half; if not, search the other.
    public static int Solve(int[] a, int target)
    {
        int lo = 0, hi = a.Length - 1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            if (a[mid] == target) return mid;

            if (a[lo] <= a[mid]) // left half sorted
            {
                if (a[lo] <= target && target < a[mid]) hi = mid - 1; else lo = mid + 1;
            }
            else // right half sorted
            {
                if (a[mid] < target && target <= a[hi]) lo = mid + 1; else hi = mid - 1;
            }
        }
        return -1;
    }

    public static void Run()
    {
        Check("[4,5,6,7,0,1,2], 0", Solve([4, 5, 6, 7, 0, 1, 2], 0), 4);
        Check("[4,5,6,7,0,1,2], 3", Solve([4, 5, 6, 7, 0, 1, 2], 3), -1);
        Check("[1], 1", Solve([1], 1), 0);
        Check("[3,1], 1", Solve([3, 1], 1), 1);
    }
}
