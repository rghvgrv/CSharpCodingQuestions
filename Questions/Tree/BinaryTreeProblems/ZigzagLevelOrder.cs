namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 3, Title = "Zigzag Level Order", Level = Medium, Problem = """
    Return the values level by level, alternating direction: left → right, then right → left, and so on.
    `[3, 9, 20, null, null, 15, 7]` → `[[3], [20, 9], [15, 7]]`.
    """)]
public static class ZigzagLevelOrder
{
    [Approach(Name = "BFS, Reverse Every Other Level", Time = "O(n)", Space = "O(w)", Idea = """
        Do a normal level-by-level BFS and flip a `leftToRight` flag after each level.
        When the flag is off, reverse that level's list before adding it.
        """)]
    public static List<List<int>> Zigzag(TreeNode? root)
    {
        var result = new List<List<int>>();
        if (root == null)
        {
            return result;
        }

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        bool leftToRight = true;

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

            if (!leftToRight)
            {
                level.Reverse();
            }
            result.Add(level);
            leftToRight = !leftToRight;
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7)], new[] { new[] { 3 }, new[] { 20, 9 }, new[] { 15, 7 } }),
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 5, 6, 7)], new[] { new[] { 1 }, new[] { 3, 2 }, new[] { 4, 5, 6, 7 } }),
    ];
}
