namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 5, Title = "Top View of a Tree", Level = Medium, Problem = """
    Give the root column 0, every left child `column - 1` and every right child `column + 1`.
    Looking down from above, you see the **highest** node in each column. Return them from the leftmost column to the rightmost.
    `[20, 8, 22, 5, 3, 4, 25, null, null, 10, 14]` → `[5, 8, 20, 22, 25]`.
    """)]
public static class TopView
{
    [Approach(Name = "BFS With Column Numbers", Time = "O(n log n)", Space = "O(n)", Idea = """
        BFS reaches nodes in order from top to bottom, so the **first** node seen in each column is the one visible from above.
        Store it in a `SortedDictionary` (column → value), which also keeps the columns in left-to-right order.

        Using the **last** node of each column instead gives the *bottom view*.
        """)]
    public static List<int> TopViewValues(TreeNode? root)
    {
        var firstInColumn = new SortedDictionary<int, int>();
        var queue = new Queue<(TreeNode Node, int Column)>();
        if (root != null)
        {
            queue.Enqueue((root, 0));
        }

        while (queue.Count > 0)
        {
            var (node, column) = queue.Dequeue();
            if (!firstInColumn.ContainsKey(column))
            {
                firstInColumn[column] = node.Value;
            }
            if (node.Left != null)
            {
                queue.Enqueue((node.Left, column - 1));
            }
            if (node.Right != null)
            {
                queue.Enqueue((node.Right, column + 1));
            }
        }
        return firstInColumn.Values.ToList();
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(20, 8, 22, 5, 3, 4, 25, null, null, 10, 14)], new[] { 5, 8, 20, 22, 25 }),
        new([TreeNode.FromLevelOrder(1, 2, 3, null, 4, null, null, null, 5)], new[] { 2, 1, 3 }),
    ];
}
