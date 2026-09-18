namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 1, Title = "Diameter of a Binary Tree", Level = Easy, Problem = """
    The diameter is the number of **edges** on the longest path between any two nodes. The path doesn't have to go through the root.
    `[1, 2, 3, 4, 5]` → `3` (the path 4 → 2 → 1 → 3).
    """)]
public static class DiameterOfBinaryTree
{
    [Approach(Name = "Height at Every Node", Time = "O(n²)", Space = "O(h)", Idea = """
        The longest path that bends at a node has length `height(left) + height(right)`.
        Visit every node and compute both heights from scratch. The same heights get recomputed many times.
        """)]
    public static int DiameterSlow(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        int throughThisNode = Height(node.Left) + Height(node.Right);
        return Math.Max(throughThisNode, Math.Max(DiameterSlow(node.Left), DiameterSlow(node.Right)));
    }

    private static int Height(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        return 1 + Math.Max(Height(node.Left), Height(node.Right));
    }

    [Approach(Name = "One Pass: Return Height, Track the Best", Time = "O(n)", Space = "O(h)", Idea = """
        Compute the heights **once**, bottom-up. While a node knows its left and right heights,
        it also checks `left + right` against the best diameter so far. Then it returns its own height to its parent.
        This "return one thing, update a best answer" pattern solves many tree problems.
        """)]
    public static int DiameterFast(TreeNode? root)
    {
        int best = 0;
        HeightAndUpdate(root, ref best);
        return best;
    }

    private static int HeightAndUpdate(TreeNode? node, ref int best)
    {
        if (node == null)
        {
            return 0;
        }
        int left = HeightAndUpdate(node.Left, ref best);
        int right = HeightAndUpdate(node.Right, ref best);
        best = Math.Max(best, left + right);
        return 1 + Math.Max(left, right);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 5)], 3),
        new([TreeNode.FromLevelOrder(1, 2)], 1),
        new([TreeNode.FromLevelOrder(1, 2, null, 3, 4, 5, null, null, 6)], 4),
    ];
}
