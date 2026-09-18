namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_02, "Iterative Traversals (Using a Stack)", Medium,
"Do preorder, inorder and postorder traversal without recursion.")]
public static class IterativeTraversals
{
    // Preorder: pop, visit, push right then left (so left is processed first).
    public static List<int> PreOrder(TreeNode? root)
    {
        var res = new List<int>();
        var stack = new Stack<TreeNode>();
        if (root != null) stack.Push(root);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            res.Add(n.Val);
            if (n.Right != null) stack.Push(n.Right);
            if (n.Left != null) stack.Push(n.Left);
        }
        return res;
    }

    // Inorder: go left as far as possible, visit, then turn right.
    public static List<int> InOrder(TreeNode? root)
    {
        var res = new List<int>();
        var stack = new Stack<TreeNode>();
        var cur = root;
        while (cur != null || stack.Count > 0)
        {
            for (; cur != null; cur = cur.Left) stack.Push(cur);
            cur = stack.Pop();
            res.Add(cur.Val);
            cur = cur.Right;
        }
        return res;
    }

    // Postorder: do "root, right, left" (mirror of preorder), then reverse → "left, right, root".
    public static List<int> PostOrder(TreeNode? root)
    {
        var res = new List<int>();
        var stack = new Stack<TreeNode>();
        if (root != null) stack.Push(root);
        while (stack.Count > 0)
        {
            var n = stack.Pop();
            res.Add(n.Val);
            if (n.Left != null) stack.Push(n.Left);
            if (n.Right != null) stack.Push(n.Right);
        }
        res.Reverse();
        return res;
    }

    public static void Run()
    {
        var root = TreeNode.From(1, 2, 3, 4, 5, null, 6);
        Check("PreOrder", PreOrder(root), [1, 2, 4, 5, 3, 6]);
        Check("InOrder", InOrder(root), [4, 2, 5, 1, 3, 6]);
        Check("PostOrder", PostOrder(root), [4, 5, 2, 6, 3, 1]);
    }
}
