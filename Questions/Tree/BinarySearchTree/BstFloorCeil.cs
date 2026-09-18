namespace CodingQuestions.Tree.BinarySearchTree;

[Q(2_03_06, "Floor & Ceil in a BST", Easy,
"Floor(x) = largest value <= x; Ceil(x) = smallest value >= x. Return null if none exists.")]
public static class BstFloorCeil
{
    // Walk down: whenever a node qualifies, remember it and move toward a tighter answer.
    public static int? Floor(TreeNode? n, int x)
    {
        int? best = null;
        while (n != null)
        {
            if (n.Val == x) return x;
            if (n.Val < x) { best = n.Val; n = n.Right; }
            else n = n.Left;
        }
        return best;
    }

    public static int? Ceil(TreeNode? n, int x)
    {
        int? best = null;
        while (n != null)
        {
            if (n.Val == x) return x;
            if (n.Val > x) { best = n.Val; n = n.Left; }
            else n = n.Right;
        }
        return best;
    }

    public static void Run()
    {
        var root = TreeNode.From(8, 4, 12, 2, 6, 10, 14);
        Check("Floor(5)", Floor(root, 5), 4);
        Check("Ceil(5)", Ceil(root, 5), 6);
        Check("Floor(11)", Floor(root, 11), 10);
        Check("Ceil(15)", Ceil(root, 15), null);
        Check("Floor(1)", Floor(root, 1), null);
    }
}
