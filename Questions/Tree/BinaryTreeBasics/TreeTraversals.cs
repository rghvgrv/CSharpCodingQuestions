namespace CodingQuestions.Tree.BinaryTreeBasics;

[Q(2_01_01, "Build a Tree & DFS Traversals (Pre / In / Post-order)", Easy,
"Create a binary tree node class, build a tree by hand, and print its preorder (root-left-right), inorder (left-root-right) and postorder (left-right-root) traversals recursively.")]
public static class TreeTraversals
{
    // A binary tree node: a value and up to two children. (The shared TreeNode in Lib.cs is the same thing.)
    public class Node(int val, Node? left = null, Node? right = null)
    {
        public int Val = val;
        public Node? Left = left, Right = right;
    }

    public static void PreOrder(Node? n, List<int> res) { if (n == null) return; res.Add(n.Val); PreOrder(n.Left, res); PreOrder(n.Right, res); }
    public static void InOrder(Node? n, List<int> res) { if (n == null) return; InOrder(n.Left, res); res.Add(n.Val); InOrder(n.Right, res); }
    public static void PostOrder(Node? n, List<int> res) { if (n == null) return; PostOrder(n.Left, res); PostOrder(n.Right, res); res.Add(n.Val); }

    public static void Run()
    {
        //        1
        //       / \
        //      2   3
        //     / \   \
        //    4   5   6
        var root = new Node(1,
            new Node(2, new Node(4), new Node(5)),
            new Node(3, null, new Node(6)));

        List<int> pre = [], ino = [], post = [];
        PreOrder(root, pre); InOrder(root, ino); PostOrder(root, post);
        Check("PreOrder  (root, L, R)", pre, [1, 2, 4, 5, 3, 6]);
        Check("InOrder   (L, root, R)", ino, [4, 2, 5, 1, 3, 6]);
        Check("PostOrder (L, R, root)", post, [4, 5, 2, 6, 3, 1]);

        // From here on, questions build trees from level-order arrays: TreeNode.From(1, 2, 3, 4, 5, null, 6)
        Check("TreeNode.From(1,2,3,4,5,null,6)", TreeNode.From(1, 2, 3, 4, 5, null, 6)!.ToString(), "[1, 2, 3, 4, 5, null, 6]");
    }
}
