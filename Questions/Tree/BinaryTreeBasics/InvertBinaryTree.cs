namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_07, "Invert (Mirror) a Binary Tree", Easy,
"Swap the left and right children of every node.")]
public static class InvertBinaryTree
{
    // Swap children, recurse into both. Time O(n)
    public static TreeNode? Solve(TreeNode? n)
    {
        if (n == null) return null;
        (n.Left, n.Right) = (Solve(n.Right), Solve(n.Left));
        return n;
    }

    public static void Run()
    {
        Check("[4,2,7,1,3,6,9]", Solve(TreeNode.From(4, 2, 7, 1, 3, 6, 9))?.ToString(), "[4, 7, 2, 9, 6, 3, 1]");
        Check("[2,1,3]", Solve(TreeNode.From(2, 1, 3))?.ToString(), "[2, 3, 1]");
    }
}
