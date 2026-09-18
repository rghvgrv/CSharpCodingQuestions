namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 1, Title = "Search in a BST", Level = Easy, Problem = """
    Return `true` if `target` is in the binary search tree.
    """)]
public static class SearchBst
{
    [Approach(Name = "Check Every Node", Time = "O(n)", Space = "O(h)", Idea = """
        Search the whole tree like any binary tree, ignoring the BST ordering. Correct, but it wastes the ordering.
        """)]
    public static bool ContainsAnyTree(TreeNode? node, int target)
    {
        if (node == null)
        {
            return false;
        }
        return node.Value == target || ContainsAnyTree(node.Left, target) || ContainsAnyTree(node.Right, target);
    }

    [Approach(Name = "Follow the Ordering (Recursive)", Time = "O(h)", Space = "O(h)", Idea = """
        At each node, the ordering tells you which way to go: smaller → left, bigger → right.
        Only one path from the root is ever visited.
        """)]
    public static bool ContainsRecursive(TreeNode? node, int target)
    {
        if (node == null)
        {
            return false;
        }
        if (target == node.Value)
        {
            return true;
        }
        return target < node.Value
            ? ContainsRecursive(node.Left, target)
            : ContainsRecursive(node.Right, target);
    }

    [Approach(Name = "Follow the Ordering (Loop)", Time = "O(h)", Space = "O(1)", Idea = """
        The same walk as a simple loop, so it needs no call stack at all.
        """)]
    public static bool ContainsLoop(TreeNode? node, int target)
    {
        while (node != null)
        {
            if (target == node.Value)
            {
                return true;
            }
            node = target < node.Value ? node.Left : node.Right;
        }
        return false;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(8, 3, 10, 1, 6, null, 14), 6], true),
        new([TreeNode.FromLevelOrder(8, 3, 10, 1, 6, null, 14), 7], false),
    ];
}
