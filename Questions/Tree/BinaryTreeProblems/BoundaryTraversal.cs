namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_14, "Boundary Traversal", Medium,
"Print the boundary anticlockwise: root, the left boundary (top-down, no leaves), all leaves (left to right), then the right boundary (bottom-up, no leaves).")]
public static class BoundaryTraversal
{
    static bool IsLeaf(TreeNode n) => n.Left == null && n.Right == null;

    public static List<int> Solve(TreeNode? root)
    {
        var res = new List<int>();
        if (root == null) return res;
        if (!IsLeaf(root)) res.Add(root.Val);

        // Left boundary: keep going left (or right if no left), skipping leaves.
        for (var n = root.Left; n != null && !IsLeaf(n); n = n.Left ?? n.Right) res.Add(n.Val);

        void Leaves(TreeNode? n)
        {
            if (n == null) return;
            if (IsLeaf(n)) { res.Add(n.Val); return; }
            Leaves(n.Left);
            Leaves(n.Right);
        }
        Leaves(root);

        // Right boundary, collected top-down then reversed.
        var right = new List<int>();
        for (var n = root.Right; n != null && !IsLeaf(n); n = n.Right ?? n.Left) right.Add(n.Val);
        right.Reverse();
        res.AddRange(right);
        return res;
    }

    public static void Run()
    {
        //          20
        //        /    \
        //       8      22
        //      / \       \
        //     4   12      25
        //        /  \
        //       10  14
        Check("tree", Solve(TreeNode.From(20, 8, 22, 4, 12, null, 25, null, null, 10, 14)), [20, 8, 4, 10, 14, 25, 22]);
        Check("[1,null,2,3,4]", Solve(TreeNode.From(1, null, 2, 3, 4)), [1, 3, 4, 2]);
    }
}
