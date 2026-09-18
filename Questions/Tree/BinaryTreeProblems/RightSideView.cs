namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 4, Title = "Right Side View", Level = Medium, Problem = """
    Imagine standing to the right of the tree. Return the values you can see, top to bottom: the **last** node of every level.
    `[1, 2, 3, null, 5, null, 4]` → `[1, 3, 4]`.
    """)]
public static class RightSideView
{
    [Approach(Name = "BFS, Keep the Last of Each Level", Time = "O(n)", Space = "O(w)", Idea = """
        Walk level by level with a queue. The last node taken from the queue in each level is the one you'd see.
        """)]
    public static List<int> RightViewWithBfs(TreeNode? root)
    {
        var view = new List<int>();
        if (root == null)
        {
            return view;
        }

        var queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        while (queue.Count > 0)
        {
            int levelSize = queue.Count;
            for (int i = 0; i < levelSize; i++)
            {
                TreeNode node = queue.Dequeue();
                if (i == levelSize - 1)
                {
                    view.Add(node.Value);
                }
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
        return view;
    }

    [Approach(Name = "DFS, Right Child First", Time = "O(n)", Space = "O(h)", Idea = """
        Go down the tree visiting the **right** child before the left. The first node reached at each new depth is the rightmost one there.
        `depth == view.Count` means "this depth hasn't been seen yet".
        Swap the order of the two calls and you get the **left** side view.
        """)]
    public static List<int> RightViewWithDfs(TreeNode? root)
    {
        var view = new List<int>();
        Visit(root, 0, view);
        return view;
    }

    private static void Visit(TreeNode? node, int depth, List<int> view)
    {
        if (node == null)
        {
            return;
        }
        if (depth == view.Count)
        {
            view.Add(node.Value);
        }
        Visit(node.Right, depth + 1, view);
        Visit(node.Left, depth + 1, view);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3, null, 5, null, 4)], new[] { 1, 3, 4 }),
        new([TreeNode.FromLevelOrder(1, 2, 3, 4)], new[] { 1, 3, 4 }),
        new([null], Array.Empty<int>()),
    ];
}
