namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 9, Title = "Maximum Path Sum", Level = Hard, Problem = """
    A path is any chain of connected nodes (each node used at most once, and it doesn't have to pass through the root).
    Values can be negative. Return the largest possible sum of a path.
    `[-10, 9, 20, null, null, 15, 7]` → `42` (15 → 20 → 7).
    """)]
public static class MaxPathSum
{
    [Approach(Name = "One Pass: Best Downward Gain", Time = "O(n)", Space = "O(h)", Idea = """
        For each node, compute its **gain**: the best sum of a path that starts at this node and goes **down** one side only.
        A negative side is worth nothing, so use `max(0, gain)`.

        The best path that **bends** at a node is `value + leftGain + rightGain`. Check it against the best so far,
        then return `value + max(leftGain, rightGain)` to the parent, because a parent can continue down only one side.
        """)]
    public static int MaxSum(TreeNode root)
    {
        int best = int.MinValue;
        Gain(root, ref best);
        return best;
    }

    private static int Gain(TreeNode? node, ref int best)
    {
        if (node == null)
        {
            return 0;
        }
        int left = Math.Max(0, Gain(node.Left, ref best));
        int right = Math.Max(0, Gain(node.Right, ref best));
        best = Math.Max(best, node.Value + left + right);
        return node.Value + Math.Max(left, right);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3)], 6),
        new([TreeNode.FromLevelOrder(-10, 9, 20, null, null, 15, 7)], 42),
        new([TreeNode.FromLevelOrder(-3)], -3),
    ];
}
