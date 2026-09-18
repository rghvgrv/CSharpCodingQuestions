namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_07, "Root-to-Leaf Paths & Path Sum", Medium,
"(1) List every root-to-leaf path. (2) Return all root-to-leaf paths whose values sum to target.")]
public static class PathSum
{
    // Backtracking on a tree: add the node, recurse, remove it.
    public static List<string> AllPaths(TreeNode? root)
    {
        var result = new List<string>();
        var path = new List<int>();
        void Dfs(TreeNode? n)
        {
            if (n == null) return;
            path.Add(n.Val);
            if (n.Left == null && n.Right == null) result.Add(string.Join("→", path));
            Dfs(n.Left);
            Dfs(n.Right);
            path.RemoveAt(path.Count - 1);
        }
        Dfs(root);
        return result;
    }

    public static List<List<int>> WithSum(TreeNode? root, int target)
    {
        var result = new List<List<int>>();
        var path = new List<int>();
        void Dfs(TreeNode? n, int remaining)
        {
            if (n == null) return;
            path.Add(n.Val);
            remaining -= n.Val;
            if (n.Left == null && n.Right == null && remaining == 0) result.Add([.. path]);
            Dfs(n.Left, remaining);
            Dfs(n.Right, remaining);
            path.RemoveAt(path.Count - 1);
        }
        Dfs(root, target);
        return result;
    }

    public static void Run()
    {
        Check("AllPaths([1,2,3,null,5])", AllPaths(TreeNode.From(1, 2, 3, null, 5)), ["1→2→5", "1→3"]);
        var root = TreeNode.From(5, 4, 8, 11, null, 13, 4, 7, 2, null, null, 5, 1);
        Check("WithSum(target=22)", WithSum(root, 22), [[5, 4, 11, 2], [5, 8, 4, 5]]);
    }
}
