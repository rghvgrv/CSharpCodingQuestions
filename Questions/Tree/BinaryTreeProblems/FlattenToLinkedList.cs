namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 12, Title = "Flatten a Tree Into a Linked List", Level = Medium, Problem = """
    Rearrange the tree in place into a chain that uses only `Right` links, in **preorder** order. Every `Left` becomes `null`.
    `[1, 2, 5, 3, 4, null, 6]` → `1 → 2 → 3 → 4 → 5 → 6` (all right children).
    """)]
public static class FlattenToLinkedList
{
    [Approach(Name = "Collect in Preorder, Then Relink", Time = "O(n)", Space = "O(n)", Idea = """
        Put all nodes in a list in preorder, then link each node's `Right` to the next node in the list and clear its `Left`.
        """)]
    public static TreeNode? FlattenWithList(TreeNode? root)
    {
        var nodes = new List<TreeNode>();
        Collect(root, nodes);
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Left = null;
            nodes[i].Right = i + 1 < nodes.Count ? nodes[i + 1] : null;
        }
        return root;
    }

    private static void Collect(TreeNode? node, List<TreeNode> nodes)
    {
        if (node == null)
        {
            return;
        }
        nodes.Add(node);
        Collect(node.Left, nodes);
        Collect(node.Right, nodes);
    }

    [Approach(Name = "Rewire In Place", Time = "O(n)", Space = "O(1)", Idea = """
        Walk down the right side. At each node that has a left subtree:

        1. Find the **rightmost** node of that left subtree (the last one visited before `Right` in preorder).
        2. Hang the node's current right subtree onto it.
        3. Move the left subtree over to the right, and clear `Left`.
        """)]
    public static TreeNode? FlattenInPlace(TreeNode? root)
    {
        TreeNode? current = root;
        while (current != null)
        {
            if (current.Left != null)
            {
                TreeNode rightmost = current.Left;
                while (rightmost.Right != null)
                {
                    rightmost = rightmost.Right;
                }
                rightmost.Right = current.Right;
                current.Right = current.Left;
                current.Left = null;
            }
            current = current.Right;
        }
        return root;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 5, 3, 4, null, 6)], TreeNode.FromLevelOrder(1, null, 2, null, 3, null, 4, null, 5, null, 6)),
        new([TreeNode.FromLevelOrder(0)], TreeNode.FromLevelOrder(0)),
    ];
}
