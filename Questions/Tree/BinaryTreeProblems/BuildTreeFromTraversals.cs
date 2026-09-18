namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_10, "Construct Tree from Preorder & Inorder", Medium,
"Rebuild the binary tree from its preorder and inorder traversals (values are unique).")]
public static class BuildTreeFromTraversals
{
    // Preorder's next value is the root of the current subtree. Its position in inorder splits
    // left subtree (before it) from right subtree (after it). A dictionary makes the lookup O(1). Time O(n)
    public static TreeNode? Solve(int[] preorder, int[] inorder)
    {
        var index = inorder.Select((v, i) => (v, i)).ToDictionary(x => x.v, x => x.i);
        int next = 0;
        TreeNode? Build(int lo, int hi)
        {
            if (lo > hi) return null;
            var root = new TreeNode(preorder[next++]);
            int mid = index[root.Val];
            root.Left = Build(lo, mid - 1);
            root.Right = Build(mid + 1, hi);
            return root;
        }
        return Build(0, inorder.Length - 1);
    }

    public static void Run()
    {
        Check("pre=[3,9,20,15,7], in=[9,3,15,20,7]", Solve([3, 9, 20, 15, 7], [9, 3, 15, 20, 7])?.ToString(), "[3, 9, 20, null, null, 15, 7]");
        Check("pre=[1,2,4,5,3,6], in=[4,2,5,1,3,6]", Solve([1, 2, 4, 5, 3, 6], [4, 2, 5, 1, 3, 6])?.ToString(), "[1, 2, 3, 4, 5, null, 6]");
    }
}
