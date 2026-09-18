namespace CSharpCodingQuestions.Core;

/// <summary>A node of a binary tree: a value and up to two children.</summary>
public class TreeNode(int value, TreeNode? left = null, TreeNode? right = null)
{
    public int Value { get; set; } = value;
    public TreeNode? Left { get; set; } = left;
    public TreeNode? Right { get; set; } = right;

    /// <summary>
    /// Builds a tree from its level-order values, top to bottom and left to right.
    /// null means "no node here": (1, 2, 3, null, 4) → 1 has children 2 and 3; 2 has only a right child 4.
    /// </summary>
    public static TreeNode? FromLevelOrder(params int?[] values)
    {
        if (values.Length == 0 || values[0] == null)
        {
            return null;
        }

        var root = new TreeNode(values[0]!.Value);
        var waiting = new Queue<TreeNode>();
        waiting.Enqueue(root);
        int index = 1;
        while (index < values.Length)
        {
            TreeNode parent = waiting.Dequeue();
            if (values[index] is int leftValue)
            {
                parent.Left = new TreeNode(leftValue);
                waiting.Enqueue(parent.Left);
            }
            index++;

            if (index < values.Length && values[index] is int rightValue)
            {
                parent.Right = new TreeNode(rightValue);
                waiting.Enqueue(parent.Right);
            }
            index++;
        }
        return root;
    }

    /// <summary>Level-order text like [1, 2, 3, null, 4].</summary>
    public override string ToString()
    {
        var parts = new List<string>();
        var queue = new Queue<TreeNode?>();
        queue.Enqueue(this);
        while (queue.Count > 0)
        {
            TreeNode? node = queue.Dequeue();
            parts.Add(node == null ? "null" : node.Value.ToString());
            if (node != null)
            {
                queue.Enqueue(node.Left);
                queue.Enqueue(node.Right);
            }
        }

        while (parts[^1] == "null")
        {
            parts.RemoveAt(parts.Count - 1);
        }
        return "[" + string.Join(", ", parts) + "]";
    }
}
