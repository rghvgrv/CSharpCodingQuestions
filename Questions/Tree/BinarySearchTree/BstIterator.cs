namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_07, "BST Iterator & C# yield return", Medium,
"Design an iterator with Next() and HasNext() that returns BST values in ascending order, using O(h) memory. Then do the same with C#'s yield return.")]
public static class BstIterator
{
    // Controlled inorder traversal: the stack holds the path of "left turns" still to visit.
    // Next() is amortized O(1) (each node is pushed and popped once).
    public class Iterator
    {
        readonly Stack<TreeNode> stack = new();
        public Iterator(TreeNode? root) => PushLeft(root);

        void PushLeft(TreeNode? n) { for (; n != null; n = n.Left) stack.Push(n); }

        public bool HasNext() => stack.Count > 0;

        public int Next()
        {
            var n = stack.Pop();
            PushLeft(n.Right);
            return n.Val;
        }
    }

    // The C# way: the compiler builds the state machine. Lazy, so `foreach` + `break` stops early.
    public static IEnumerable<int> InOrder(TreeNode? n)
    {
        if (n == null) yield break;
        foreach (int v in InOrder(n.Left)) yield return v;
        yield return n.Val;
        foreach (int v in InOrder(n.Right)) yield return v;
    }

    public static void Run()
    {
        var root = TreeNode.From(7, 3, 15, null, null, 9, 20);
        var it = new Iterator(root);
        var values = new List<int>();
        while (it.HasNext()) values.Add(it.Next());
        Check("Iterator", values, [3, 7, 9, 15, 20]);
        Check("yield return, first 3", InOrder(root).Take(3), [3, 7, 9]);
    }
}
