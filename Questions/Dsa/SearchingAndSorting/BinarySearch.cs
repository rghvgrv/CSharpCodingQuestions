namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_01, "Linear vs Binary Search", Easy,
"Find the index of a target in an array. Write linear search (any array), then binary search (sorted array) both iteratively and recursively.")]
public static class BinarySearch
{
    // Time O(n): check every element.
    public static int Linear(int[] a, int target)
    {
        for (int i = 0; i < a.Length; i++)
            if (a[i] == target) return i;
        return -1;
    }

    // Time O(log n): halve the search range each step. lo + (hi - lo) / 2 avoids int overflow.
    public static int Iterative(int[] a, int target)
    {
        int lo = 0, hi = a.Length - 1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            if (a[mid] == target) return mid;
            if (a[mid] < target) lo = mid + 1; else hi = mid - 1;
        }
        return -1;
    }

    public static int Recursive(int[] a, int target, int lo, int hi)
    {
        if (lo > hi) return -1;
        int mid = lo + (hi - lo) / 2;
        if (a[mid] == target) return mid;
        return a[mid] < target ? Recursive(a, target, mid + 1, hi) : Recursive(a, target, lo, mid - 1);
    }

    public static void Run()
    {
        int[] a = [-1, 0, 3, 5, 9, 12];
        Check("Linear(9)", Linear(a, 9), 4);
        Check("Iterative(9)", Iterative(a, 9), 4);
        Check("Iterative(2)", Iterative(a, 2), -1);
        Check("Recursive(12)", Recursive(a, 12, 0, a.Length - 1), 5);
        Check("Built-in Array.BinarySearch(5)", Array.BinarySearch(a, 5), 3);

        var big = Enumerable.Range(0, 1_000_000).ToArray();
        int steps = 0;
        for (int lo = 0, hi = big.Length - 1; lo <= hi; steps++)
        {
            int mid = lo + (hi - lo) / 2;
            if (big[mid] == 999_999) break;
            if (big[mid] < 999_999) lo = mid + 1; else hi = mid - 1;
        }
        Console.WriteLine($"Binary search in 1,000,000 items took {steps + 1} steps (linear would take 1,000,000)");
    }
}
