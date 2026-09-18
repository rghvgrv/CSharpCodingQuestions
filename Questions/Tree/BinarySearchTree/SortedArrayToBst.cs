namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_05, "Sorted Array to Balanced BST", Easy,
"Convert a sorted array into a height-balanced BST.")]
public static class SortedArrayToBst
{
    // The middle element becomes the root; recurse on each half. Height = ⌈log₂(n+1)⌉.
    public static TreeNode? Solve(int[] a, int lo, int hi)
    {
        if (lo > hi) return null;
        int mid = lo + (hi - lo) / 2;
        return new TreeNode(a[mid], Solve(a, lo, mid - 1), Solve(a, mid + 1, hi));
    }

    static int Height(TreeNode? n) => n == null ? 0 : 1 + Math.Max(Height(n.Left), Height(n.Right));

    public static void Run()
    {
        int[] a = [-10, -3, 0, 5, 9];
        Check("[-10,-3,0,5,9]", Solve(a, 0, a.Length - 1)?.ToString(), "[0, -10, 5, null, -3, null, 9]");
        var big = Enumerable.Range(1, 1000).ToArray();
        Check("1000 items → height", Height(Solve(big, 0, big.Length - 1)), 10);
    }
}
