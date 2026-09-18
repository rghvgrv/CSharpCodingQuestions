namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_02, "Balanced Binary Tree", Easy,
"A tree is height-balanced if at every node the left and right subtree heights differ by at most 1. Check it in O(n).")]
public static class BalancedBinaryTree
{
    // Return the height, or -1 as soon as any subtree is unbalanced (so we never recompute heights).
    static int Height(TreeNode? n)
    {
        if (n == null) return 0;
        int l = Height(n.Left);
        if (l < 0) return -1;
        int r = Height(n.Right);
        if (r < 0 || Math.Abs(l - r) > 1) return -1;
        return 1 + Math.Max(l, r);
    }

    public static bool Solve(TreeNode? root) => Height(root) >= 0;

    public static void Run()
    {
        Check("[3,9,20,null,null,15,7]", Solve(TreeNode.From(3, 9, 20, null, null, 15, 7)), true);
        Check("[1,2,2,3,3,null,null,4,4]", Solve(TreeNode.From(1, 2, 2, 3, 3, null, null, 4, 4)), false);
        Check("[]", Solve(null), true);
    }
}
