namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_03, "Level Order Traversal (BFS)", Easy,
"Return the node values level by level, from top to bottom and left to right.")]
public static class LevelOrderTraversal
{
    // BFS with a queue. Snapshot queue.Count at the start of each level to know where the level ends.
    public static List<List<int>> Solve(TreeNode? root)
    {
        var levels = new List<List<int>>();
        if (root == null) return levels;
        var queue = new Queue<TreeNode>([root]);
        while (queue.Count > 0)
        {
            var level = new List<int>();
            for (int n = queue.Count; n > 0; n--)
            {
                var node = queue.Dequeue();
                level.Add(node.Val);
                if (node.Left != null) queue.Enqueue(node.Left);
                if (node.Right != null) queue.Enqueue(node.Right);
            }
            levels.Add(level);
        }
        return levels;
    }

    public static void Run()
    {
        Check("[3,9,20,null,null,15,7]", Solve(TreeNode.From(3, 9, 20, null, null, 15, 7)), [[3], [9, 20], [15, 7]]);
        Check("[1]", Solve(TreeNode.From(1)), [[1]]);
        Check("[]", Solve(null), []);
    }
}
