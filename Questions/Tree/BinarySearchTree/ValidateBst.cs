namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 4, Title = "Is It a Valid BST?", Level = Medium, Problem = """
    Return `true` if the tree is a valid binary search tree: for **every** node, all values in its left subtree are smaller
    and all values in its right subtree are bigger.

    Watch out: checking only a node's direct children isn't enough. In `[5, 4, 7, null, 6]`, 6 is bigger than its parent 4,
    but it sits in 5's left subtree, so the tree is **not** valid.
    """)]
public static class ValidateBst
{
    [Approach(Name = "Inorder Must Be Sorted", Time = "O(n)", Space = "O(n)", Idea = """
        An inorder traversal of a valid BST lists the values in strictly increasing order.
        Collect them in a list and check that each value is bigger than the one before.
        """)]
    public static bool IsValidByInorder(TreeNode? root)
    {
        var values = new List<int>();
        CollectInorder(root, values);
        for (int i = 1; i < values.Count; i++)
        {
            if (values[i] <= values[i - 1])
            {
                return false;
            }
        }
        return true;
    }

    private static void CollectInorder(TreeNode? node, List<int> values)
    {
        if (node == null)
        {
            return;
        }
        CollectInorder(node.Left, values);
        values.Add(node.Value);
        CollectInorder(node.Right, values);
    }

    [Approach(Name = "Pass Down the Allowed Range", Time = "O(n)", Space = "O(h)", Idea = """
        Every node must fit inside a range `(min, max)` set by its ancestors:

        - Going **left** means the value must stay below the current node, so `max` becomes the node's value.
        - Going **right** means `min` becomes the node's value.

        `long` limits make even `int.MinValue` and `int.MaxValue` values work.
        """)]
    public static bool IsValidByRange(TreeNode? root)
    {
        return FitsRange(root, long.MinValue, long.MaxValue);
    }

    private static bool FitsRange(TreeNode? node, long min, long max)
    {
        if (node == null)
        {
            return true;
        }
        if (node.Value <= min || node.Value >= max)
        {
            return false;
        }
        return FitsRange(node.Left, min, node.Value) && FitsRange(node.Right, node.Value, max);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(2, 1, 3)], true),
        new([TreeNode.FromLevelOrder(5, 4, 7, null, 6)], false),
        new([TreeNode.FromLevelOrder(5, 1, 4, null, null, 3, 6)], false),
    ];
}
