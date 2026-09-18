namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 6, Title = "Lowest Common Ancestor in a BST", Level = Medium, Problem = """
    Find the lowest node that has both `p` and `q` below it (a node counts as below itself), using the BST ordering.
    """)]
public static class BstLowestCommonAncestor
{
    [Approach(Name = "General Binary Tree Method", Time = "O(n)", Space = "O(h)", Idea = """
        The method from *Binary Tree Problems* works on any tree: search both subtrees, and the node where `p` and `q` split is the answer.
        But it may visit the whole tree.
        """)]
    public static int LcaAnyTree(TreeNode root, int p, int q)
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
        return left != null && right != null ? node : left ?? right;
    }

    [Approach(Name = "Walk Down Using the Ordering", Time = "O(h)", Space = "O(1)", Idea = """
        Start at the root:

        - both values **smaller** than the node → the answer is in the left subtree
        - both values **bigger** → it's in the right subtree
        - otherwise they split here (or one of them *is* this node), so this node is the answer
        """)]
    public static int LcaBst(TreeNode root, int p, int q)
    {
        TreeNode? node = root;
        while (node != null)
        {
            if (p < node.Value && q < node.Value)
            {
                node = node.Left;
            }
            else if (p > node.Value && q > node.Value)
            {
                node = node.Right;
            }
            else
            {
                return node.Value;
            }
        }
        return -1;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(6, 2, 8, 0, 4, 7, 9, null, null, 3, 5), 2, 8], 6),
        new([TreeNode.FromLevelOrder(6, 2, 8, 0, 4, 7, 9, null, null, 3, 5), 2, 4], 2),
        new([TreeNode.FromLevelOrder(6, 2, 8, 0, 4, 7, 9, null, null, 3, 5), 3, 5], 4),
    ];
}
