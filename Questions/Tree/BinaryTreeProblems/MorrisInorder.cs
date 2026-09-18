namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_13, "Morris Inorder Traversal (O(1) Space)", Hard,
"Do an inorder traversal with no recursion and no stack, using O(1) extra memory.")]
public static class MorrisInorder
{
    // Temporarily thread each node's inorder predecessor back to it (predecessor.Right = node),
    // so we can climb back up without a stack. The thread is removed on the second visit.
    public static List<int> Solve(TreeNode? root)
    {
        var res = new List<int>();
        var cur = root;
        while (cur != null)
        {
            if (cur.Left == null)
            {
                res.Add(cur.Val);
                cur = cur.Right;
                continue;
            }
            var pred = cur.Left;
            while (pred.Right != null && pred.Right != cur) pred = pred.Right;
            if (pred.Right == null)
            {
                pred.Right = cur; // create thread, go left
                cur = cur.Left;
            }
            else
            {
                pred.Right = null; // left side done: remove thread, visit, go right
                res.Add(cur.Val);
                cur = cur.Right;
            }
        }
        return res;
    }

    public static void Run()
    {
        var root = TreeNode.From(4, 2, 6, 1, 3, 5, 7);
        Check("Inorder of [4,2,6,1,3,5,7]", Solve(root), [1, 2, 3, 4, 5, 6, 7]);
        Check("Tree restored afterwards", root!.ToString(), "[4, 2, 6, 1, 3, 5, 7]");
    }
}
