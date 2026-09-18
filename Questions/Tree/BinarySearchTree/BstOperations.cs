namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_01, "BST: Insert, Search, Delete", Medium,
"Implement a Binary Search Tree: for every node, left subtree < node < right subtree. Support Insert, Search, Delete, Min and Max.")]
public static class BstOperations
{
    // Every operation walks one root-to-leaf path: O(h). h = log n if balanced, n if degenerate (sorted inserts).
    public static TreeNode Insert(TreeNode? n, int v)
    {
        if (n == null) return new TreeNode(v);
        if (v < n.Val) n.Left = Insert(n.Left, v);
        else if (v > n.Val) n.Right = Insert(n.Right, v);
        return n;
    }

    public static bool Search(TreeNode? n, int v)
    {
        while (n != null && n.Val != v) n = v < n.Val ? n.Left : n.Right;
        return n != null;
    }

    public static int Min(TreeNode n) { while (n.Left != null) n = n.Left; return n.Val; }
    public static int Max(TreeNode n) { while (n.Right != null) n = n.Right; return n.Val; }

    // Delete: 0 or 1 child → replace by the child. 2 children → copy the inorder successor
    // (smallest in the right subtree) into this node, then delete the successor from the right subtree.
    public static TreeNode? Delete(TreeNode? n, int v)
    {
        if (n == null) return null;
        if (v < n.Val) n.Left = Delete(n.Left, v);
        else if (v > n.Val) n.Right = Delete(n.Right, v);
        else
        {
            if (n.Left == null) return n.Right;
            if (n.Right == null) return n.Left;
            n.Val = Min(n.Right);
            n.Right = Delete(n.Right, n.Val);
        }
        return n;
    }

    static List<int> InOrder(TreeNode? n) => n == null ? [] : [.. InOrder(n.Left), n.Val, .. InOrder(n.Right)];

    public static void Run()
    {
        TreeNode? root = null;
        foreach (int v in new[] { 50, 30, 70, 20, 40, 60, 80 }) root = Insert(root, v);
        Check("Tree after inserts", root!.ToString(), "[50, 30, 70, 20, 40, 60, 80]");
        Check("InOrder is sorted", InOrder(root), [20, 30, 40, 50, 60, 70, 80]);
        Check("Search(60)", Search(root, 60), true);
        Check("Search(65)", Search(root, 65), false);
        Check("Min, Max", (Min(root), Max(root)), (20, 80));

        root = Delete(root, 20); // leaf
        root = Delete(root, 30); // one child
        root = Delete(root, 50); // two children → replaced by 60
        Check("After deleting 20, 30, 50", root?.ToString(), "[60, 40, 70, null, null, null, 80]");
    }
}
