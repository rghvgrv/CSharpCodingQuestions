namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 3, Title = "Postorder Traversal", Level = Easy, Problem = """
    Visit every node in **postorder**: the left subtree, then the right subtree, then the node itself.
    For the tree `[1, 2, 3, 4, 5, null, 6]` that's `[4, 5, 2, 6, 3, 1]`.
    (Useful when children must be handled before their parent, like deleting a tree.)
    """)]
public static class PostorderTraversal
{
    [Approach(Name = "Recursion", Time = "O(n)", Space = "O(h)", Idea = """
        Traverse left, traverse right, then record the node.
        """)]
    public static List<int> PostorderRecursive(TreeNode? root)
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
        Visit(node.Right, result);
        result.Add(node.Value);
    }

    [Approach(Name = "Stack, Then Reverse", Time = "O(n)", Space = "O(h)", Idea = """
        A neat trick: visit in the order **node, right, left** (like preorder with the children swapped), then reverse the result.
        Reversing "node, right, left" gives "left, right, node", which is postorder.
        """)]
    public static List<int> PostorderWithStack(TreeNode? root)
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
            if (node.Left != null)
            {
                stack.Push(node.Left);
            }
            if (node.Right != null)
            {
                stack.Push(node.Right);
            }
        }
        result.Reverse();
        return result;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 5, null, 6)], new[] { 4, 5, 2, 6, 3, 1 }),
        new([TreeNode.FromLevelOrder(1, null, 2, 3)], new[] { 3, 2, 1 }),
    ];
}
