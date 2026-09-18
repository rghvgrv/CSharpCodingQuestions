namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 2, Title = "Insert Into a BST", Level = Easy, Problem = """
    Insert `value` (not already in the tree) so it's still a valid binary search tree, and return the root.
    `[4, 2, 7, 1, 3]` + `5` → `[4, 2, 7, 1, 3, 5]`.
    """)]
public static class InsertIntoBst
{
    [Approach(Name = "Recursion", Time = "O(h)", Space = "O(h)", Idea = """
        Walk down as if searching for `value`. The empty spot where the search ends is exactly where it belongs.
        Each call returns the (possibly new) subtree, so the parent's link gets updated automatically.
        """)]
    public static TreeNode InsertRecursive(TreeNode? node, int value)
    {
        if (node == null)
        {
            return new TreeNode(value);
        }
        if (value < node.Value)
        {
            node.Left = InsertRecursive(node.Left, value);
        }
        else
        {
            node.Right = InsertRecursive(node.Right, value);
        }
        return node;
    }

    [Approach(Name = "Loop", Time = "O(h)", Space = "O(1)", Idea = """
        Walk down with a loop until the child in the right direction is empty, then attach the new node there.
        """)]
    public static TreeNode InsertLoop(TreeNode? root, int value)
    {
        var newNode = new TreeNode(value);
        if (root == null)
        {
            return newNode;
        }

        TreeNode current = root;
        while (true)
        {
            if (value < current.Value)
            {
                if (current.Left == null)
                {
                    current.Left = newNode;
                    return root;
                }
                current = current.Left;
            }
            else
            {
                if (current.Right == null)
                {
                    current.Right = newNode;
                    return root;
                }
                current = current.Right;
            }
        }
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(4, 2, 7, 1, 3), 5], TreeNode.FromLevelOrder(4, 2, 7, 1, 3, 5)),
        new([null, 10], TreeNode.FromLevelOrder(10)),
    ];
}
