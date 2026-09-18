namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 6, Title = "Vertical Order Traversal", Level = Hard, Problem = """
    Group the nodes by column (root = 0, left = -1, right = +1), from the leftmost column to the rightmost.
    Inside a column, list nodes from top to bottom; nodes at the same row and column are sorted by value.
    `[3, 9, 20, null, null, 15, 7]` → `[[9], [3, 15], [20], [7]]`.
    """)]
public static class VerticalOrder
{
    [Approach(Name = "Record Positions, Then Sort", Time = "O(n log n)", Space = "O(n)", Idea = """
        1. Walk the tree and record `(column, row, value)` for every node.
        2. Sort by column, then row, then value.
        3. Group neighboring entries that share a column.
        """)]
    public static List<List<int>> Vertical(TreeNode? root)
    {
        var cells = new List<(int Column, int Row, int Value)>();
        Record(root, 0, 0, cells);
        cells.Sort();

        var result = new List<List<int>>();
        for (int i = 0; i < cells.Count; i++)
        {
            if (i == 0 || cells[i].Column != cells[i - 1].Column)
            {
                result.Add(new List<int>());
            }
            result[^1].Add(cells[i].Value);
        }
        return result;
    }

    private static void Record(TreeNode? node, int row, int column, List<(int Column, int Row, int Value)> cells)
    {
        if (node == null)
        {
            return;
        }
        cells.Add((column, row, node.Value));
        Record(node.Left, row + 1, column - 1, cells);
        Record(node.Right, row + 1, column + 1, cells);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(3, 9, 20, null, null, 15, 7)], new[] { new[] { 9 }, new[] { 3, 15 }, new[] { 20 }, new[] { 7 } }),
        new([TreeNode.FromLevelOrder(1, 2, 3, 4, 6, 5, 7)], new[] { new[] { 4 }, new[] { 2 }, new[] { 1, 5, 6 }, new[] { 3 }, new[] { 7 } }),
    ];
}
