namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_02, "Validate a Binary Search Tree", Medium,
"Check whether a binary tree is a valid BST. Watch out: comparing a node only with its direct children is not enough.")]
public static class ValidateBst
{
    // Every node must lie in a (min, max) range inherited from its ancestors.
    // long bounds so int.MinValue / int.MaxValue node values still work.
    public static bool Solve(TreeNode? n, long min = long.MinValue, long max = long.MaxValue) =>
        n == null || (min < n.Val && n.Val < max && Solve(n.Left, min, n.Val) && Solve(n.Right, n.Val, max));

    public static void Run()
    {
        Check("[2,1,3]", Solve(TreeNode.From(2, 1, 3)), true);
        Check("[5,1,4,null,null,3,6]", Solve(TreeNode.From(5, 1, 4, null, null, 3, 6)), false);
        // 6 is > 4 (its parent's check passes) but lies in 5's LEFT subtree → invalid
        Check("[5,4,7,null,6]", Solve(TreeNode.From(5, 4, 7, null, 6)), false);
        Check("[2147483647]", Solve(TreeNode.From(int.MaxValue)), true);
    }
}
