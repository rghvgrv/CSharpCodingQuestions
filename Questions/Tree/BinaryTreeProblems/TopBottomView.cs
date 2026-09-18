namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_05, "Top View & Bottom View", Medium,
"Give each node a horizontal distance (root 0, left child -1, right child +1). Return the nodes seen from above and from below, left to right.")]
public static class TopBottomView
{
    // BFS (so higher levels come first). Top view keeps the FIRST node per column; bottom view keeps the LAST.
    public static (List<int> Top, List<int> Bottom) Solve(TreeNode? root)
    {
        var top = new SortedDictionary<int, int>();
        var bottom = new SortedDictionary<int, int>();
        var queue = new Queue<(TreeNode Node, int Col)>();
        if (root != null) queue.Enqueue((root, 0));
        while (queue.Count > 0)
        {
            var (node, col) = queue.Dequeue();
            top.TryAdd(col, node.Val);
            bottom[col] = node.Val;
            if (node.Left != null) queue.Enqueue((node.Left, col - 1));
            if (node.Right != null) queue.Enqueue((node.Right, col + 1));
        }
        return ([.. top.Values], [.. bottom.Values]);
    }

    public static void Run()
    {
        //          20
        //        /    \
        //       8      22
        //      / \    /  \
        //     5   3  4    25
        //        / \
        //       10  14
        var (topView, bottomView) = Solve(TreeNode.From(20, 8, 22, 5, 3, 4, 25, null, null, 10, 14));
        Check("Top view", topView, [5, 8, 20, 22, 25]);
        Check("Bottom view", bottomView, [5, 10, 4, 14, 25]);
    }
}
