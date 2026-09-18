namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_04, "Maximum & Minimum Depth", Easy,
"Find the height of a binary tree (nodes on the longest root-to-leaf path) and the minimum depth (nodes on the shortest root-to-leaf path).")]
public static class TreeHeight
{
    // Height = 1 + taller subtree. Time O(n)
    public static int MaxDepth(TreeNode? n) => n == null ? 0 : 1 + Math.Max(MaxDepth(n.Left), MaxDepth(n.Right));

    // Careful: a node with one child is not a leaf, so don't take the min with the missing side.
    public static int MinDepth(TreeNode? n) => n switch
    {
        null => 0,
        { Left: null } => 1 + MinDepth(n.Right),
        { Right: null } => 1 + MinDepth(n.Left),
        _ => 1 + Math.Min(MinDepth(n.Left), MinDepth(n.Right))
    };

    public static void Run()
    {
        Check("MaxDepth([3,9,20,null,null,15,7])", MaxDepth(TreeNode.From(3, 9, 20, null, null, 15, 7)), 3);
        Check("MinDepth([3,9,20,null,null,15,7])", MinDepth(TreeNode.From(3, 9, 20, null, null, 15, 7)), 2);
        Check("MinDepth([2,null,3,null,4])", MinDepth(TreeNode.From(2, null, 3, null, 4)), 3);
        Check("MaxDepth(empty)", MaxDepth(null), 0);
    }
}
