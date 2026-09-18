namespace CodingQuestions.Dsa.Arrays;

[Q(1_02_02, "Reverse an Array In Place", Easy,
"Reverse an array without allocating a second array.")]
public static class ReverseArray
{
    // Time O(n), Space O(1): swap the ends and walk inward.
    public static int[] Solve(int[] a)
    {
        for (int i = 0, j = a.Length - 1; i < j; i++, j--)
            (a[i], a[j]) = (a[j], a[i]);
        return a;
    }

    public static void Run()
    {
        Check("[1, 2, 3, 4, 5]", Solve([1, 2, 3, 4, 5]), [5, 4, 3, 2, 1]);
        Check("[1, 2]", Solve([1, 2]), [2, 1]);
        Check("[]", Solve([]), []);
    }
}
