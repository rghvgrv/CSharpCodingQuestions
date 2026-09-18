namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 8, Title = "Lowest Common Ancestor", Level = Medium, Problem = """
    Find the **lowest** (deepest) node that has both values `p` and `q` below it. A node counts as being below itself.
    Values are unique and both exist in the tree.
    """)]
public static class LowestCommonAncestor
{
    [Approach(Name = "Compare the Two Root Paths", Time = "O(n)", Space = "O(h)", Idea = """
        1. Find the path from the root to `p`, and the path from the root to `q`.
        2. Walk both paths together from the root. The last node they share is the answer.
        """)]
    public static int LcaWithPaths(TreeNode root, int p, int q)
    {
        var pathToP = new List<int>();
        var pathToQ = new List<int>();
        FindPath(root, p, pathToP);
        FindPath(root, q, pathToQ);

        int i = 0;
        while (i < pathToP.Count && i < pathToQ.Count && pathToP[i] == pathToQ[i])
        {
            i++;
        }
        return pathToP[i - 1];
    }

    private static bool FindPath(TreeNode? node, int target, List<int> path)
    {
        if (node == null)
        {
            return false;
        }
        path.Add(node.Value);
        if (node.Value == target || FindPath(node.Left, target, path) || FindPath(node.Right, target, path))
        {
            return true;
        }
        path.RemoveAt(path.Count - 1);
        return false;
    }

    [Approach(Name = "One Recursive Pass", Time = "O(n)", Space = "O(h)", Idea = """
        Ask each subtree: "did you find `p` or `q`?"

        - If this node **is** `p` or `q`, return it.
        - If the left side found one **and** the right side found the other, this node is where they split, so it's the answer.
        - Otherwise, pass up whichever side found something.
        """)]
    public static int LcaRecursive(TreeNode root, int p, int q)
    {
        return Find(root, p, q)!.Value;
    }

    private static TreeNode? Find(TreeNode? node, int p, int q)
    {
        if (node == null || node.Value == p || node.Value == q)
        {
            return node;
        }
        TreeNode? left = Find(node.Left, p, q);
        TreeNode? right = Find(node.Right, p, q);
        if (left != null && right != null)
        {
            return node;
        }
        return left ?? right;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 5, 1, 6, 2, 0, 8, null, null, 7, 4), 5, 1], 3),
        new([TreeNode.FromLevelOrder(3, 5, 1, 6, 2, 0, 8, null, null, 7, 4), 5, 4], 5),
        new([TreeNode.FromLevelOrder(3, 5, 1, 6, 2, 0, 8, null, null, 7, 4), 7, 8], 3),
    ];
}
