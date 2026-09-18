namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 2, Title = "Is the Tree Balanced?", Level = Easy, Problem = """
    A tree is **balanced** when, at every node, the heights of the left and right subtrees differ by at most 1.
    """)]
public static class BalancedBinaryTree
{
    [Approach(Name = "Check Heights at Every Node", Time = "O(n²)", Space = "O(h)", Idea = """
        For each node, compute the height of both subtrees from scratch and compare them, then check both children the same way.
        """)]
    public static bool IsBalancedSlow(TreeNode? node)
    {
        if (node == null)
        {
            return true;
        }
        if (Math.Abs(Height(node.Left) - Height(node.Right)) > 1)
        {
            return false;
        }
        return IsBalancedSlow(node.Left) && IsBalancedSlow(node.Right);
    }

    private static int Height(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        return 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    [Approach(Name = "One Pass, -1 Means Unbalanced", Time = "O(n)", Space = "O(h)", Idea = """
        Compute heights bottom-up once. If any subtree turns out unbalanced, return `-1` instead of a height.
        Every parent passes that `-1` straight up, so the check stops early and nothing is computed twice.
        """)]
    public static bool IsBalancedFast(TreeNode? root)
    {
        return CheckedHeight(root) != -1;
    }

    private static int CheckedHeight(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        int left = CheckedHeight(node.Left);
        if (left == -1)
        {
            return -1;
        }
        int right = CheckedHeight(node.Right);
        if (right == -1 || Math.Abs(left - right) > 1)
        {
            return -1;
        }
        return 1 + Math.Max(left, right);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7)], true),
        new([TreeNode.FromLevelOrder(1, 2, 2, 3, 3, null, null, 4, 4)], false),
        new([null], true),
    ];
}
