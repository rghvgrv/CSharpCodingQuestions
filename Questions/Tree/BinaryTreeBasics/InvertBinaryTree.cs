namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 9, Title = "Invert (Mirror) a Binary Tree", Level = Easy, Problem = """
    Swap the left and right child of **every** node, so the tree becomes its mirror image.
    `[4, 2, 7, 1, 3, 6, 9]` → `[4, 7, 2, 9, 6, 3, 1]`.
    """)]
public static class InvertBinaryTree
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(h)", Idea = """
        Invert the left subtree, invert the right subtree, then swap them.
        """)]
    public static TreeNode? InvertRecursive(TreeNode? node)
    {
        if (node == null)
        {
            return null;
        }
        TreeNode? invertedLeft = InvertRecursive(node.Left);
        TreeNode? invertedRight = InvertRecursive(node.Right);
        node.Left = invertedRight;
        node.Right = invertedLeft;
        return node;
    }

    [Approach(Name = "Queue (Level by Level)", Time = "O(n)", Space = "O(w)", Idea = """
        Visit every node with a queue and swap its two children. The order of visiting doesn't matter,
        as long as every node gets its children swapped exactly once.
        """)]
    public static TreeNode? InvertWithQueue(TreeNode? root)
    {
        if (root == null)
        {
            return null;
        }

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            TreeNode node = queue.Dequeue();
            TreeNode? temp = node.Left;
            node.Left = node.Right;
            node.Right = temp;

            if (node.Left != null)
            {
                queue.Enqueue(node.Left);
            }
            if (node.Right != null)
            {
                queue.Enqueue(node.Right);
            }
        }
        return root;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(4, 2, 7, 1, 3, 6, 9)], TreeNode.FromLevelOrder(4, 7, 2, 9, 6, 3, 1)),
        new([TreeNode.FromLevelOrder(2, 1, 3)], TreeNode.FromLevelOrder(2, 3, 1)),
    ];
}
