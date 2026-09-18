namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 13, Title = "Boundary of a Binary Tree", Level = Medium, Problem = """
    Walk around the outside of the tree, counter-clockwise: the root, the **left edge** going down (without leaves),
    **all leaves** from left to right, then the **right edge** going up (without leaves).
    `[20, 8, 22, 4, 12, null, 25, null, null, 10, 14]` → `[20, 8, 4, 10, 14, 25, 22]`.
    """)]
public static class BoundaryTraversal
{
    [Approach(Name = "Left Edge, Leaves, Right Edge", Time = "O(n)", Space = "O(h)", Idea = """
        Collect three parts separately so no node is added twice:

        1. **Left edge**: from the root's left child, keep going left (or right if there's no left child), stopping before a leaf.
        2. **Leaves**: any traversal that goes left before right lists them in left-to-right order.
        3. **Right edge**: like the left edge but on the right side. Collect it top-down, then reverse it so it goes up.
        """)]
    public static List<int> Boundary(TreeNode? root)
    {
        var result = new List<int>();
        if (root == null)
        {
            return result;
        }
        if (!IsLeaf(root))
        {
            result.Add(root.Value);
        }

        for (TreeNode? node = root.Left; node != null && !IsLeaf(node); node = node.Left ?? node.Right)
        {
            result.Add(node.Value);
        }

        AddLeaves(root, result);

        var rightEdge = new List<int>();
        for (TreeNode? node = root.Right; node != null && !IsLeaf(node); node = node.Right ?? node.Left)
        {
            rightEdge.Add(node.Value);
        }
        rightEdge.Reverse();
        result.AddRange(rightEdge);
        return result;
    }

    private static bool IsLeaf(TreeNode node) => node.Left == null && node.Right == null;

    private static void AddLeaves(TreeNode? node, List<int> result)
    {
        if (node == null)
        {
            return;
        }
        if (IsLeaf(node))
        {
            result.Add(node.Value);
            return;
        }
        AddLeaves(node.Left, result);
        AddLeaves(node.Right, result);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(20, 8, 22, 4, 12, null, 25, null, null, 10, 14)], new[] { 20, 8, 4, 10, 14, 25, 22 }),
        new([TreeNode.FromLevelOrder(1, null, 2, 3, 4)], new[] { 1, 3, 4, 2 }),
    ];
}
