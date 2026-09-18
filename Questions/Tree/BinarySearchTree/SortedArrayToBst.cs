namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 7, Title = "Sorted Array to a Balanced BST", Level = Easy, Problem = """
    Build a **height-balanced** binary search tree from a sorted array.
    `[-10, -3, 0, 5, 9]` → `[0, -10, 5, null, -3, null, 9]`.
    """)]
public static class SortedArrayToBst
{
    [Approach(Name = "Middle Element as the Root", Time = "O(n)", Space = "O(log n)", Idea = """
        The middle element becomes the root: half the values are smaller (they build the left subtree) and half are bigger (the right subtree).
        Do the same for each half. The sides always differ by at most one node, so the tree stays balanced, with a height of about `log₂ n`.

        (Inserting the values one by one in sorted order would instead create a chain as tall as `n`.)
        """)]
    public static TreeNode? BuildBalanced(int[] sorted)
    {
        return Build(sorted, 0, sorted.Length - 1);
    }

    private static TreeNode? Build(int[] sorted, int low, int high)
    {
        if (low > high)
        {
            return null;
        }
        int middle = low + (high - low) / 2;
        var root = new TreeNode(sorted[middle]);
        root.Left = Build(sorted, low, middle - 1);
        root.Right = Build(sorted, middle + 1, high);
        return root;
    }

    public static Example[] Examples =>
    [
        new([new[] { -10, -3, 0, 5, 9 }], TreeNode.FromLevelOrder(0, -10, 5, null, -3, null, 9)),
        new([new[] { 1, 2, 3, 4, 5, 6, 7 }], TreeNode.FromLevelOrder(4, 2, 6, 1, 3, 5, 7)),
    ];
}
