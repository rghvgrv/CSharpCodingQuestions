namespace CodingQuestions.Tree.BinaryTreeProblems;

[Q(2_02_06, "Vertical Order Traversal", Hard,
"Group nodes by column (left to right). Within a column, order by row (top to bottom); nodes at the same row and column are sorted by value.")]
public static class VerticalOrderTraversal
{
    // Collect (col, row, value) for every node, sort by all three, then group by column.
    public static List<List<int>> Solve(TreeNode? root)
    {
        var cells = new List<(int Col, int Row, int Val)>();
        void Dfs(TreeNode? n, int row, int col)
        {
            if (n == null) return;
            cells.Add((col, row, n.Val));
            Dfs(n.Left, row + 1, col - 1);
            Dfs(n.Right, row + 1, col + 1);
        }
        Dfs(root, 0, 0);
        return cells.Order()
                    .GroupBy(c => c.Col)
                    .Select(g => g.Select(c => c.Val).ToList())
                    .ToList();
    }

    public static void Run()
    {
        Check("[3,9,20,null,null,15,7]", Solve(TreeNode.From(3, 9, 20, null, null, 15, 7)), [[9], [3, 15], [20], [7]]);
        Check("[1,2,3,4,5,6,7] (5 and 6 share a cell)", Solve(TreeNode.From(1, 2, 3, 4, 5, 6, 7)), [[4], [2], [1, 5, 6], [3], [7]]);
    }
}
