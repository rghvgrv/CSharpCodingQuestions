namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeBasics;

[Question(Order = 8, Title = "Symmetric Tree", Level = Easy, Problem = """
    Return `true` if the tree is a mirror image of itself around its center.
    `[1, 2, 2, 3, 4, 4, 3]` → `true`.
    """)]
public static class SymmetricTree
{
    [Approach(Name = "Recursion on Mirror Pairs", Time = "O(n)", Space = "O(h)", Idea = """
        Compare the left and right subtrees as **mirrors**: two nodes mirror each other if their values are equal,
        and the **outer** children match (`left.Left` with `right.Right`) and the **inner** children match (`left.Right` with `right.Left`).
        """)]
    public static bool IsSymmetricRecursive(TreeNode? root)
    {
        return root == null || AreMirrors(root.Left, root.Right);
    }

    private static bool AreMirrors(TreeNode? left, TreeNode? right)
    {
        if (left == null && right == null)
        {
            return true;
        }
        if (left == null || right == null || left.Value != right.Value)
        {
            return false;
        }
        return AreMirrors(left.Left, right.Right) && AreMirrors(left.Right, right.Left);
    }

    [Approach(Name = "Queue of Pairs", Time = "O(n)", Space = "O(w)", Idea = """
        The same check without recursion: keep a queue of node **pairs** that should mirror each other.
        Take a pair, compare it, then queue its outer pair and its inner pair.
        """)]
    public static bool IsSymmetricWithQueue(TreeNode? root)
    {
        if (root == null)
        {
            return true;
        }

        var pairs = new Queue<(TreeNode? Left, TreeNode? Right)>();
        pairs.Enqueue((root.Left, root.Right));
        while (pairs.Count > 0)
        {
            var (left, right) = pairs.Dequeue();
            if (left == null && right == null)
            {
                continue;
            }
            if (left == null || right == null || left.Value != right.Value)
            {
                return false;
            }
            pairs.Enqueue((left.Left, right.Right));
            pairs.Enqueue((left.Right, right.Left));
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(1, 2, 2, 3, 4, 4, 3)], true),
        new([TreeNode.FromLevelOrder(1, 2, 2, null, 3, null, 3)], false),
    ];
}
