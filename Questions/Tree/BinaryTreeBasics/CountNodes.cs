namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_05, "Count Nodes, Leaves, Sum & Max", Easy,
"Write recursive functions for the total node count, the leaf count, the sum of all values and the maximum value.")]
public static class CountNodes
{
    // Same pattern every time: combine the answers of the left and right subtrees.
    public static int Count(TreeNode? n) => n == null ? 0 : 1 + Count(n.Left) + Count(n.Right);
    public static int Leaves(TreeNode? n) => n == null ? 0 : n.Left == null && n.Right == null ? 1 : Leaves(n.Left) + Leaves(n.Right);
    public static int Sum(TreeNode? n) => n == null ? 0 : n.Val + Sum(n.Left) + Sum(n.Right);
    public static int Max(TreeNode? n) => n == null ? int.MinValue : Math.Max(n.Val, Math.Max(Max(n.Left), Max(n.Right)));

    public static void Run()
    {
        var root = TreeNode.From(1, 2, 3, 4, 5, null, 9);
        Check("Count", Count(root), 6);
        Check("Leaves", Leaves(root), 3);
        Check("Sum", Sum(root), 24);
        Check("Max", Max(root), 9);
    }
}
