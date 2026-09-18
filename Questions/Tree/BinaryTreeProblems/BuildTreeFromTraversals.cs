namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 10, Title = "Build a Tree From Preorder and Inorder", Level = Medium, Problem = """
    You get a tree's preorder and inorder traversals (all values are different). Rebuild the tree.
    `preorder = [3, 9, 20, 15, 7]`, `inorder = [9, 3, 15, 20, 7]` → `[3, 9, 20, null, null, 15, 7]`.
    """)]
public static class BuildTreeFromTraversals
{
    [Approach(Name = "Search Inorder Each Time", Time = "O(n²)", Space = "O(n)", Idea = """
        - Preorder always lists the **root first**.
        - In inorder, everything **left** of the root belongs to the left subtree, and everything **right** of it to the right subtree.

        So: take the next preorder value as the root, find it in the inorder range by scanning, then build the left part and the right part the same way.
        The scan makes it `O(n)` per node.
        """)]
    public static TreeNode? BuildWithSearch(int[] preorder, int[] inorder)
    {
        int nextRoot = 0;
        return BuildSearching(preorder, inorder, 0, inorder.Length - 1, ref nextRoot);
    }

    private static TreeNode? BuildSearching(int[] preorder, int[] inorder, int low, int high, ref int nextRoot)
    {
        if (low > high)
        {
            return null;
        }
        var root = new TreeNode(preorder[nextRoot]);
        nextRoot++;

        int middle = low;
        while (inorder[middle] != root.Value)
        {
            middle++;
        }
        root.Left = BuildSearching(preorder, inorder, low, middle - 1, ref nextRoot);
        root.Right = BuildSearching(preorder, inorder, middle + 1, high, ref nextRoot);
        return root;
    }

    [Approach(Name = "Dictionary of Inorder Positions", Time = "O(n)", Space = "O(n)", Idea = """
        Same idea, but first store every value's inorder position in a dictionary, so finding the root is instant.
        """)]
    public static TreeNode? BuildWithDictionary(int[] preorder, int[] inorder)
    {
        var positionOf = new Dictionary<int, int>();
        for (int i = 0; i < inorder.Length; i++)
        {
            positionOf[inorder[i]] = i;
        }
        int nextRoot = 0;
        return BuildFast(preorder, positionOf, 0, inorder.Length - 1, ref nextRoot);
    }

    private static TreeNode? BuildFast(int[] preorder, Dictionary<int, int> positionOf, int low, int high, ref int nextRoot)
    {
        if (low > high)
        {
            return null;
        }
        var root = new TreeNode(preorder[nextRoot]);
        nextRoot++;

        int middle = positionOf[root.Value];
        root.Left = BuildFast(preorder, positionOf, low, middle - 1, ref nextRoot);
        root.Right = BuildFast(preorder, positionOf, middle + 1, high, ref nextRoot);
        return root;
    }

    public static Example[] Examples =>
    [
        new([new[] { 3, 9, 20, 15, 7 }, new[] { 9, 3, 15, 20, 7 }], TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7)),
        new([new[] { 1, 2, 4, 5, 3, 6 }, new[] { 4, 2, 5, 1, 3, 6 }], TreeNode.FromLevelOrder(1, 2, 3, 4, 5, null, 6)),
    ];
}
