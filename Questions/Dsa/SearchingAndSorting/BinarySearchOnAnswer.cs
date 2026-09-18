namespace CodingQuestions.Dsa.SearchingAndSorting;

[Q(1_05_05, "Binary Search on the Answer: Sqrt & Koko Eating Bananas", Medium,
"(1) Integer square root of x without Math.Sqrt. (2) Koko has piles of bananas and h hours; find the minimum eating speed k (bananas/hour) to finish in time.")]
public static class BinarySearchOnAnswer
{
    // Pattern: the answer lies in a range and a yes/no test is monotonic → binary search the range.

    // Largest m with m² <= x
    public static int Sqrt(int x)
    {
        long lo = 0, hi = x;
        while (lo < hi)
        {
            long mid = (lo + hi + 1) / 2;
            if (mid * mid <= x) lo = mid; else hi = mid - 1;
        }
        return (int)lo;
    }

    // Smallest speed k where total hours <= h. Faster speed never takes more hours → monotonic.
    public static int MinEatingSpeed(int[] piles, int h)
    {
        int lo = 1, hi = piles.Max();
        while (lo < hi)
        {
            int k = lo + (hi - lo) / 2;
            long hours = piles.Sum(p => (long)(p + k - 1) / k);
            if (hours <= h) hi = k; else lo = k + 1;
        }
        return lo;
    }

    public static void Run()
    {
        Check("Sqrt(8)", Sqrt(8), 2);
        Check("Sqrt(16)", Sqrt(16), 4);
        Check("Sqrt(2147395600)", Sqrt(2147395600), 46340);
        Check("MinEatingSpeed([3,6,7,11], h=8)", MinEatingSpeed([3, 6, 7, 11], 8), 4);
        Check("MinEatingSpeed([30,11,23,4,20], h=5)", MinEatingSpeed([30, 11, 23, 4, 20], 5), 30);
        Check("MinEatingSpeed([30,11,23,4,20], h=6)", MinEatingSpeed([30, 11, 23, 4, 20], 6), 23);
    }
}
