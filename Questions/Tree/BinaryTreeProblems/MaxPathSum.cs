namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_09, "Binary Tree Maximum Path Sum", Hard,
"A path is any sequence of connected nodes (not necessarily through the root). Values can be negative. Return the maximum path sum.")]
public static class MaxPathSum
{
    // Gain(n) = best sum of a path going DOWN from n (one side only). A negative side contributes 0 (skip it).
    // The best path that "bends" at n = n.Val + gain(left) + gain(right) → update the global answer.
    public static int Solve(TreeNode root)
    {
        int best = int.MinValue;
        int Gain(TreeNode? n)
        {
            if (n == null) return 0;
            int l = Math.Max(0, Gain(n.Left)), r = Math.Max(0, Gain(n.Right));
            best = Math.Max(best, n.Val + l + r);
            return n.Val + Math.Max(l, r);
        }
        Gain(root);
        return best;
    }

    public static void Run()
    {
        Check("[1,2,3]", Solve(TreeNode.From(1, 2, 3)!), 6);
        Check("[-10,9,20,null,null,15,7]", Solve(TreeNode.From(-10, 9, 20, null, null, 15, 7)!), 42);
        Check("[-3]", Solve(TreeNode.From(-3)!), -3);
    }
}
