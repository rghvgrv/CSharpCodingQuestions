namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_08, "Lowest Common Ancestor (Binary Tree)", Medium,
"Find the lowest node that has both p and q as descendants (a node counts as its own descendant).")]
public static class LowestCommonAncestor
{
    // If p and q are found in different subtrees, this node is the LCA.
    // Otherwise the LCA is whichever side found something. Time O(n)
    public static TreeNode? Solve(TreeNode? n, int p, int q)
    {
        if (n == null || n.Val == p || n.Val == q) return n;
        var left = Solve(n.Left, p, q);
        var right = Solve(n.Right, p, q);
        return left != null && right != null ? n : left ?? right;
    }

    public static void Run()
    {
        //          3
        //       /     \
        //      5       1
        //     / \     / \
        //    6   2   0   8
        //       / \
        //      7   4
        var root = TreeNode.From(3, 5, 1, 6, 2, 0, 8, null, null, 7, 4);
        Check("LCA(5, 1)", Solve(root, 5, 1)?.Val, 3);
        Check("LCA(5, 4)", Solve(root, 5, 4)?.Val, 5);
        Check("LCA(7, 8)", Solve(root, 7, 8)?.Val, 3);
        Check("LCA(6, 4)", Solve(root, 6, 4)?.Val, 5);
    }
}
