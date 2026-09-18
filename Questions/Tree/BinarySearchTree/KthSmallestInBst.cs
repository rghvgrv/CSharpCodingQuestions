namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_03, "Kth Smallest Element in a BST", Medium,
"Return the kth smallest value (1-indexed) in a BST.")]
public static class KthSmallestInBst
{
    // Inorder traversal of a BST is sorted, so stop at the kth visited node. Time O(h + k)
    public static int Solve(TreeNode? root, int k)
    {
        var stack = new Stack<TreeNode>();
        var cur = root;
        while (true)
        {
            for (; cur != null; cur = cur.Left) stack.Push(cur);
            cur = stack.Pop();
            if (--k == 0) return cur.Val;
            cur = cur.Right;
        }
    }

    public static void Run()
    {
        Check("[3,1,4,null,2], k=1", Solve(TreeNode.From(3, 1, 4, null, 2), 1), 1);
        Check("[5,3,6,2,4,null,null,1], k=3", Solve(TreeNode.From(5, 3, 6, 2, 4, null, null, 1), 3), 3);
        Check("[5,3,6,2,4,null,null,1], k=6", Solve(TreeNode.From(5, 3, 6, 2, 4, null, null, 1), 6), 6);
    }
}
