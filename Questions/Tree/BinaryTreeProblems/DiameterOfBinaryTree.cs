namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_01, "Diameter of Binary Tree", Easy,
"The diameter is the number of edges on the longest path between any two nodes. The path may or may not pass through the root.")]
public static class DiameterOfBinaryTree
{
    // Pattern: a helper returns the height, and updates a global best along the way.
    // The longest path through a node = height(left) + height(right).
    public static int Solve(TreeNode? root)
    {
        int best = 0;
        int Height(TreeNode? n)
        {
            if (n == null) return 0;
            int l = Height(n.Left), r = Height(n.Right);
            best = Math.Max(best, l + r);
            return 1 + Math.Max(l, r);
        }
        Height(root);
        return best;
    }

    public static void Run()
    {
        Check("[1,2,3,4,5]", Solve(TreeNode.From(1, 2, 3, 4, 5)), 3);
        Check("[1,2]", Solve(TreeNode.From(1, 2)), 1);
        Check("path not through root", Solve(TreeNode.From(1, 2, null, 3, 4, 5, null, null, 6, 7, null, null, 8)), 6);
    }
}
