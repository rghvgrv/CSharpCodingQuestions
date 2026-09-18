namespace CSharpCodingQuestions.Questions.Tree.BinarySearchTree;

[Question(Order = 3, Title = "Delete From a BST", Level = Medium, Problem = """
    Remove `value` from the binary search tree and return the root, keeping it a valid BST.
    """)]
public static class DeleteFromBst
{
    [Approach(Name = "Three Cases", Time = "O(h)", Space = "O(h)", Idea = """
        First find the node (smaller → go left, bigger → go right). Then:

        1. **No children**: just remove it.
        2. **One child**: replace the node with that child.
        3. **Two children**: copy in the **next bigger value** (the smallest value in the right subtree), then delete that value from the right subtree.
           It has no left child, so deleting it falls into case 1 or 2.
        """)]
    public static TreeNode? Delete(TreeNode? node, int value)
    {
        if (node == null)
        {
            return null;
        }

        if (value < node.Value)
        {
            node.Left = Delete(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = Delete(node.Right, value);
        }
        else
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            if (node.Right == null)
            {
                return node.Left;
            }

            TreeNode smallestOnRight = node.Right;
            while (smallestOnRight.Left != null)
            {
                smallestOnRight = smallestOnRight.Left;
            }
            node.Value = smallestOnRight.Value;
            node.Right = Delete(node.Right, smallestOnRight.Value);
        }
        return node;
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(50, 30, 70, 20, 40, 60, 80), 20], TreeNode.FromLevelOrder(50, 30, 70, null, 40, 60, 80)),
        new([TreeNode.FromLevelOrder(50, 30, 70, null, 40, 60, 80), 30], TreeNode.FromLevelOrder(50, 40, 70, null, null, 60, 80)),
        new([TreeNode.FromLevelOrder(50, 40, 70, null, null, 60, 80), 50], TreeNode.FromLevelOrder(60, 40, 70, null, null, null, 80)),
    ];
}
