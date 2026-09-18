namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 5, Title = "Maximum Depth (Height)", Level = Easy, Problem = """
    Return the number of nodes on the longest path from the root down to a leaf.
    `[3, 9, 20, null, null, 15, 7]` → `3`.
    """)]
public static class MaximumDepth
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(h)", Idea = """
        The depth of a tree is 1 (for the root) plus the depth of its **deeper** subtree. An empty tree has depth 0.
        """)]
    public static int DepthRecursive(TreeNode? node)
    {
        if (node == null)
        {
            return 0;
        }
        return 1 + Math.Max(DepthRecursive(node.Left), DepthRecursive(node.Right));
    }

    [Approach(Name = "Count Levels With BFS", Time = "O(n)", Space = "O(w)", Idea = """
        Walk the tree level by level with a queue and count the levels. There's no recursion, so a very deep tree can't overflow the stack.
        """)]
    public static int DepthWithBfs(TreeNode? root)
    {
        if (root == null)
        {
            return 0;
        }

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        int depth = 0;
        while (queue.Count > 0)
        {
            depth++;
            int levelSize = queue.Count;
            for (int i = 0; i < levelSize; i++)
            {
                TreeNode node = queue.Dequeue();
                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }
                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
            }
        }
        return depth;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7)], 3),
        new([TreeNode.FromLevelOrder(1, null, 2)], 2),
        new([null], 0),
    ];
}
