namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 1, Title = "Preorder Traversal", Level = Easy, Problem = """
    Visit every node in **preorder**: the node first, then its left subtree, then its right subtree.
    For the tree `[1, 2, 3, 4, 5, null, 6]` that's `[1, 2, 4, 5, 3, 6]`.
    """)]
public static class PreorderTraversal
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(h)", Idea = """
        Write the definition directly: record the node, then traverse the left subtree, then the right subtree.
        `h` is the height of the tree (the depth of the recursion).
        """)]
    public static List<int> PreorderRecursive(TreeNode? root)
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
        result.Add(node.Value);
        Visit(node.Left, result);
        Visit(node.Right, result);
    }

    [Approach(Name = "Stack", Time = "O(n)", Space = "O(h)", Idea = """
        Do the recursion's job with your own stack: pop a node, record it, then push its **right** child and then its **left** child.
        The left child is pushed last, so it's popped first.
        """)]
    public static List<int> PreorderWithStack(TreeNode? root)
    {
        var result = new List<int>();
        var stack = new Stack<TreeNode>();
        if (root != null)
        {
            stack.Push(root);
        }

        while (stack.Count > 0)
        {
            TreeNode node = stack.Pop();
            result.Add(node.Value);
            if (node.Right != null)
            {
                stack.Push(node.Right);
            }
            if (node.Left != null)
            {
                stack.Push(node.Left);
            }
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 5, null, 6)], new[] { 1, 2, 4, 5, 3, 6 }),
        new([TreeNode.FromLevelOrder(1, null, 2, 3)], new[] { 1, 2, 3 }),
        new([null], Array.Empty<int>()),
    ];
}
