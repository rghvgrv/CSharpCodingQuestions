namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_04, "Left View & Right View", Medium,
"Return the nodes visible when looking at the tree from the right side (last node of each level), and from the left side (first node of each level).")]
public static class LeftRightView
{
    // DFS visiting the preferred side first: the first node we reach at each new depth is the visible one.
    static void Dfs(TreeNode? n, int depth, bool rightFirst, List<int> view)
    {
        if (n == null) return;
        if (depth == view.Count) view.Add(n.Val);
        Dfs(rightFirst ? n.Right : n.Left, depth + 1, rightFirst, view);
        Dfs(rightFirst ? n.Left : n.Right, depth + 1, rightFirst, view);
    }

    public static List<int> RightView(TreeNode? root) { var v = new List<int>(); Dfs(root, 0, true, v); return v; }
    public static List<int> LeftView(TreeNode? root) { var v = new List<int>(); Dfs(root, 0, false, v); return v; }

    public static void Run()
    {
        //      1
        //     / \
        //    2   3
        //     \   \
        //      5   4
        //     /
        //    7
        var root = TreeNode.From(1, 2, 3, null, 5, null, 4, 7);
        Check("RightView", RightView(root), [1, 3, 4, 7]);
        Check("LeftView", LeftView(root), [1, 2, 5, 7]);
    }
}
