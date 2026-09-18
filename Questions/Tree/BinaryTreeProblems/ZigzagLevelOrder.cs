namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_03, "Zigzag Level Order Traversal", Medium,
"Return level order values, alternating left→right and right→left on each level.")]
public static class ZigzagLevelOrder
{
    // Normal BFS; reverse every other level.
    public static List<List<int>> Solve(TreeNode? root)
    {
        var result = new List<List<int>>();
        if (root == null) return result;
        var queue = new Queue<TreeNode>([root]);
        for (bool leftToRight = true; queue.Count > 0; leftToRight = !leftToRight)
        {
            var level = new List<int>();
            for (int n = queue.Count; n > 0; n--)
            {
                var node = queue.Dequeue();
                level.Add(node.Val);
                if (node.Left != null) queue.Enqueue(node.Left);
                if (node.Right != null) queue.Enqueue(node.Right);
            }
            if (!leftToRight) level.Reverse();
            result.Add(level);
        }
        return result;
    }

    public static void Run()
    {
        Check("[3,9,20,null,null,15,7]", Solve(TreeNode.From(3, 9, 20, null, null, 15, 7)), [[3], [20, 9], [15, 7]]);
        Check("[1,2,3,4,5,6,7]", Solve(TreeNode.From(1, 2, 3, 4, 5, 6, 7)), [[1], [3, 2], [4, 5, 6, 7]]);
    }
}
