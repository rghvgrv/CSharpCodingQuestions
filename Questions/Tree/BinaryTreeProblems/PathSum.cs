namespace CSharpCodingQuestions.Questions.Tree.BinaryTreeProblems;

[Question(Order = 7, Title = "Root-to-Leaf Paths With a Given Sum", Level = Medium, Problem = """
    Return every path from the root down to a **leaf** whose values add up to `target`.
    """)]
public static class PathSum
{
    [Approach(Name = "DFS With Backtracking", Time = "O(n · h)", Space = "O(h)", Idea = """
        Walk down from the root, adding each node to the current path and subtracting its value from what's `remaining`.

        - At a leaf with `remaining == 0`, save a **copy** of the path.
        - After exploring a node's children, remove it from the path (backtrack) so the path is correct for the next branch.

        Copying each found path costs up to `h`, which is where the `n · h` comes from.
        """)]
    public static List<List<int>> FindPaths(TreeNode? root, int target)
    {
        var result = new List<List<int>>();
        Explore(root, target, new List<int>(), result);
        return result;
    }

    private static void Explore(TreeNode? node, int remaining, List<int> path, List<List<int>> result)
    {
        if (node == null)
        {
            return;
        }

        path.Add(node.Value);
        remaining -= node.Value;
        bool isLeaf = node.Left == null && node.Right == null;
        if (isLeaf && remaining == 0)
        {
            result.Add(new List<int>(path));
        }

        Explore(node.Left, remaining, path, result);
        Explore(node.Right, remaining, path, result);
        path.RemoveAt(path.Count - 1);
    }

    public static Example[] Examples =>
    [
        new([TreeNode.FromLevelOrder(5, 4, 8, 11, null, 13, 4, 7, 2, null, null, 5, 1), 22], new[] { new[] { 5, 4, 11, 2 }, new[] { 5, 8, 4, 5 } }),
        new([TreeNode.FromLevelOrder(1, 2, 3), 5], Array.Empty<int[]>()),
    ];
}
