namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 6, Title = "Count Nodes and Leaves", Level = Easy, Problem = """
    Return `(nodes, leaves)`: the total number of nodes, and the number of leaves (nodes without children).
    `[1, 2, 3, 4, 5, null, 6]` → `(6, 3)`.
    """)]
public static class CountNodes
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(h)", Idea = """
        The standard tree pattern: get the answer for the left subtree and for the right subtree, then combine them.

        - Nodes: `1 + nodes(left) + nodes(right)`
        - Leaves: a node with no children counts as 1; otherwise `leaves(left) + leaves(right)`
        """)]
    public static (int Nodes, int Leaves) Count(TreeNode? root)
    {
        return (CountAll(root), CountLeaves(root));
    }

    private static int CountAll(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        return 1 + CountAll(node.Left) + CountAll(node.Right);
    }

    private static int CountLeaves(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        if (node.Left == null && node.Right == null)
        {
            return 1;
        }
        return CountLeaves(node.Left) + CountLeaves(node.Right);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 5, null, 6)], (6, 3)),
        new([TreeNode.FromLevelOrder(1)], (1, 1)),
        new([null], (0, 0)),
    ];
}
