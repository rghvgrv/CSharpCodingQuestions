namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_04, "Lowest Common Ancestor in a BST", Medium,
"Find the LCA of p and q in a BST, using the ordering to avoid searching both sides.")]
public static class BstLowestCommonAncestor
{
    // If both are smaller, go left; both bigger, go right. Otherwise they split here → this is the LCA. O(h), O(1) space
    public static TreeNode? Solve(TreeNode? n, int p, int q)
    {
        while (n != null)
        {
            if (p < n.Val && q < n.Val) n = n.Left;
            else if (p > n.Val && q > n.Val) n = n.Right;
            else return n;
        }
        return null;
    }

    public static void Run()
    {
        var root = TreeNode.From(6, 2, 8, 0, 4, 7, 9, null, null, 3, 5);
        Check("LCA(2, 8)", Solve(root, 2, 8)?.Val, 6);
        Check("LCA(2, 4)", Solve(root, 2, 4)?.Val, 2);
        Check("LCA(3, 5)", Solve(root, 3, 5)?.Val, 4);
    }
}
