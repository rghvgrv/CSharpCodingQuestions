namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 2, Title = "Inorder Traversal", Level = Easy, Problem = """
    Visit every node in **inorder**: the left subtree, then the node, then the right subtree.
    For the tree `[1, 2, 3, 4, 5, null, 6]` that's `[4, 2, 5, 1, 3, 6]`.
    (On a binary search tree, inorder gives the values in sorted order.)
    """)]
public static class InorderTraversal
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(h)", Idea = """
        Traverse the left subtree, record the node, traverse the right subtree.
        """)]
    public static List<int> InorderRecursive(TreeNode? root)
    {
        var result = new List<int>();
        Visit(root, result);
        return result;
    }

    private static void Visit(TreeNode? node, List<int> result)
    {
        if (node == null)
        {
            return;
        }
        Visit(node.Left, result);
        result.Add(node.Value);
        Visit(node.Right, result);
    }

    [Approach(Name = "Stack", Time = "O(n)", Space = "O(h)", Idea = """
        1. Go left as far as possible, pushing every node on the way.
        2. Pop a node: nothing is left of it, so record it.
        3. Move to its right child and repeat.
        """)]
    public static List<int> InorderWithStack(TreeNode? root)
    {
        var result = new List<int>();
        var stack = new Stack<TreeNode>();
        TreeNode? current = root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }
            current = stack.Pop();
            result.Add(current.Value);
            current = current.Right;
        }
        return result;
    }

    [Approach(Name = "Morris Traversal (No Stack)", Time = "O(n)", Space = "O(1)", Idea = """
        Uses no stack at all. It **temporarily re-wires** the tree so you can climb back up:

        - Before going left, find the node that comes just before `current` in inorder (the rightmost node of the left subtree)
          and point its empty `Right` back to `current`: a temporary "thread".
        - When you reach `current` again through that thread, the left side is done: remove the thread, record `current`, and go right.

        The tree is back to normal at the end.
        """)]
    public static List<int> InorderMorris(TreeNode? root)
    {
        var result = new List<int>();
        TreeNode? current = root;

        while (current != null)
        {
            if (current.Left == null)
            {
                result.Add(current.Value);
                current = current.Right;
                continue;
            }

            TreeNode before = current.Left;
            while (before.Right != null && before.Right != current)
            {
                before = before.Right;
            }

            if (before.Right == null)
            {
                before.Right = current;   // make the thread, then go left
                current = current.Left;
            }
            else
            {
                before.Right = null;      // left side finished: remove the thread
                result.Add(current.Value);
                current = current.Right;
            }
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 5, null, 6)], new[] { 4, 2, 5, 1, 3, 6 }),
        new([TreeNode.FromLevelOrder(4, 2, 6, 1, 3, 5, 7)], new[] { 1, 2, 3, 4, 5, 6, 7 }),
    ];
}
