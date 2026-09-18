namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 4, Title = "Level Order Traversal", Level = Easy, Problem = """
    Return the values level by level, top to bottom and left to right.
    `[3, 9, 20, null, null, 15, 7]` → `[[3], [9, 20], [15, 7]]`.
    """)]
public static class LevelOrderTraversal
{
    [Approach(Name = "DFS, Passing the Depth", Time = "O(n)", Space = "O(h)", Idea = """
        Walk the tree recursively (left before right) and pass along each node's depth.
        Add the value to the list for that depth, creating the list the first time a depth is reached.
        """)]
    public static List<List<int>> LevelsWithDfs(TreeNode? root)
    {
        var levels = new List<List<int>>();
        AddNode(root, 0, levels);
        return levels;
    }

    private static void AddNode(TreeNode? node, int depth, List<List<int>> levels)
    {
        if (node == null)
        {
            return;
        }
        if (depth == levels.Count)
        {
            levels.Add(new List<int>());
        }
        levels[depth].Add(node.Value);
        AddNode(node.Left, depth + 1, levels);
        AddNode(node.Right, depth + 1, levels);
    }

    [Approach(Name = "BFS With a Queue", Time = "O(n)", Space = "O(w)", Idea = """
        The natural way: a queue processes nodes in the order they were found, which is level by level.
        At the start of each level, `queue.Count` tells you exactly how many nodes belong to it.
        `w` is the widest level.
        """)]
    public static List<List<int>> LevelsWithBfs(TreeNode? root)
    {
        var levels = new List<List<int>>();
        if (root == null)
        {
            return levels;
        }

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            var level = new List<int>();
            for (int i = 0; i < levelSize; i++)
            {
                TreeNode node = queue.Dequeue();
                level.Add(node.Value);
                if (node.Left != null)
                {
                    queue.Enqueue(node.Left);
                }
                if (node.Right != null)
                {
                    queue.Enqueue(node.Right);
                }
            }
            levels.Add(level);
        }
        return levels;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7)], new[] { new[] { 3 }, new[] { 9, 20 }, new[] { 15, 7 } }),
        new([TreeNode.FromLevelOrder(1)], new[] { new[] { 1 } }),
        new([null], Array.Empty<int[]>()),
    ];
}
