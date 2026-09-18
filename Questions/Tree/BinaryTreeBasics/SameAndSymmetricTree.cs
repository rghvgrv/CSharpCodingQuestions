namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_06, "Same Tree, Symmetric Tree & Subtree", Easy,
"(1) Are two trees identical? (2) Is a tree a mirror of itself? (3) Is tree t a subtree of tree s?")]
public static class SameAndSymmetricTree
{
    public static bool Same(TreeNode? a, TreeNode? b) =>
        a == null || b == null ? a == b : a.Val == b.Val && Same(a.Left, b.Left) && Same(a.Right, b.Right);

    // Mirror: compare the outside pair (a.Left, b.Right) and the inside pair (a.Right, b.Left).
    static bool Mirror(TreeNode? a, TreeNode? b) =>
        a == null || b == null ? a == b : a.Val == b.Val && Mirror(a.Left, b.Right) && Mirror(a.Right, b.Left);

    public static bool Symmetric(TreeNode? root) => Mirror(root?.Left, root?.Right);

    // Try matching t at every node of s. Time O(m·n)
    public static bool IsSubtree(TreeNode? s, TreeNode? t) =>
        s != null && (Same(s, t) || IsSubtree(s.Left, t) || IsSubtree(s.Right, t));

    public static void Run()
    {
        Check("Same([1,2,3], [1,2,3])", Same(TreeNode.From(1, 2, 3), TreeNode.From(1, 2, 3)), true);
        Check("Same([1,2], [1,null,2])", Same(TreeNode.From(1, 2), TreeNode.From(1, null, 2)), false);
        Check("Symmetric([1,2,2,3,4,4,3])", Symmetric(TreeNode.From(1, 2, 2, 3, 4, 4, 3)), true);
        Check("Symmetric([1,2,2,null,3,null,3])", Symmetric(TreeNode.From(1, 2, 2, null, 3, null, 3)), false);
        Check("IsSubtree([3,4,5,1,2], [4,1,2])", IsSubtree(TreeNode.From(3, 4, 5, 1, 2), TreeNode.From(4, 1, 2)), true);
    }
}
