namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_12, "Flatten Binary Tree to Linked List", Medium,
"Flatten the tree in place into a \"linked list\" that uses Right pointers, in preorder. All Left pointers become null.")]
public static class FlattenToLinkedList
{
    // For each node with a left child: find the rightmost node of the left subtree,
    // hang the right subtree there, move left to right. O(n) time, O(1) space.
    public static void Solve(TreeNode? root)
    {
        for (var cur = root; cur != null; cur = cur.Right)
        {
            if (cur.Left == null) continue;
            var rightmost = cur.Left;
            while (rightmost.Right != null) rightmost = rightmost.Right;
            rightmost.Right = cur.Right;
            cur.Right = cur.Left;
            cur.Left = null;
        }
    }

    public static void Run()
    {
        var root = TreeNode.From(1, 2, 5, 3, 4, null, 6);
        Solve(root);
        var values = new List<int>();
        for (var n = root; n != null; n = n.Right)
        {
            values.Add(n.Val);
            if (n.Left != null) values.Add(-999); // should never happen
        }
        Check("[1,2,5,3,4,null,6] flattened", values, [1, 2, 3, 4, 5, 6]);
    }
}
