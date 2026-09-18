namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 5, Title = "K-th Smallest Value in a BST", Level = Medium, Problem = """
    Return the `k`-th smallest value in the binary search tree (`k = 1` is the smallest).
    """)]
public static class KthSmallestInBst
{
    [Approach(Name = "Full Inorder List", Time = "O(n)", Space = "O(n)", Idea = """
        Inorder lists a BST's values in sorted order. Collect them all and return item `k - 1`.
        """)]
    public static int KthSmallestWithList(TreeNode root, int k)
    {
        var sorted = new List<int>();
        Collect(root, sorted);
        return sorted[k - 1];
    }

    private static void Collect(TreeNode? node, List<int> sorted)
    {
        if (node == null)
        {
            return;
        }
        Collect(node.Left, sorted);
        sorted.Add(node.Value);
        Collect(node.Right, sorted);
    }

    [Approach(Name = "Inorder With a Stack, Stop at k", Time = "O(h + k)", Space = "O(h)", Idea = """
        Do the inorder walk step by step with a stack, and **stop as soon as** `k` values have been visited.
        For small `k`, only a tiny part of the tree is touched.
        """)]
    public static int KthSmallestEarlyStop(TreeNode root, int k)
    {
        var stack = new Stack<TreeNode>();
        TreeNode? current = root;
        while (true)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }
            current = stack.Pop();
            k--;
            if (k == 0)
            {
                return current.Value;
            }
            current = current.Right;
        }
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 1, 4, null, 2), 1], 1),
        new([TreeNode.FromLevelOrder(5, 3, 6, 2, 4, null, null, 1), 3], 3),
        new([TreeNode.FromLevelOrder(5, 3, 6, 2, 4, null, null, 1), 6], 6),
    ];
}
