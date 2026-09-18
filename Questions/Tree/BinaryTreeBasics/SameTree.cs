namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 7, Title = "Are Two Trees the Same?", Level = Easy, Problem = """
    Return `true` if both trees have exactly the same shape and the same values.
    """)]
public static class SameTree
{
    [Approach(Name = "Compare Their Text Forms", Time = "O(n)", Space = "O(n)", Idea = """
        Write each tree out as text (preorder, with `#` for every missing child) and compare the two strings.
        The `#` markers matter: without them, different shapes could produce the same text.
        """)]
    public static bool IsSameBySerializing(TreeNode? first, TreeNode? second)
    {
        return Serialize(first) == Serialize(second);
    }

    private static string Serialize(TreeNode? node)
    {
        if (node == null)
        {
            return "#";
        }
        return $"{node.Value},{Serialize(node.Left)},{Serialize(node.Right)}";
    }

    [Approach(Name = "Compare Node by Node", Time = "O(n)", Space = "O(h)", Idea = """
        Two trees are the same if:

        - both are empty, **or**
        - both roots have the same value, **and** the left subtrees are the same, **and** the right subtrees are the same.

        It stops at the first difference and builds no text.
        """)]
    public static bool IsSameRecursive(TreeNode? first, TreeNode? second)
    {
        if (first == null && second == null)
        {
            return true;
        }
        if (first == null || second == null || first.Value != second.Value)
        {
            return false;
        }
        return IsSameRecursive(first.Left, second.Left) && IsSameRecursive(first.Right, second.Right);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 3), TreeNode.FromLevelOrder(1, 2, 3)], true),
        new([TreeNode.FromLevelOrder(1, 2), TreeNode.FromLevelOrder(1, null, 2)], false),
        new([TreeNode.FromLevelOrder(1, 2, 1), TreeNode.FromLevelOrder(1, 1, 2)], false),
    ];
}
