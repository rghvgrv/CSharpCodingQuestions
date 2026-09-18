namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 6, Title = "Detect a Cycle in a Directed Graph", Level = Medium, Problem = """
    Edges are one-way: `[a, b]` means a → b. Return `true` if you can start at some node, follow the arrows, and come back to it.
    """)]
public static class DetectCycleDirected
{
    [Approach(Name = "Search From Every Node", Time = "O(V · (V + E))", Space = "O(V)", Idea = """
        For each node, run a DFS along the arrows and check whether it can get back to that same node.
        Correct, but the same parts of the graph are searched again for every starting node.
        """)]
    public static bool HasCycleBruteForce(int nodeCount, int[][] edges)
    {
        var next = new List<int>[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            next[node] = new List<int>();
        }
        foreach (int[] edge in edges)
        {
            next[edge[0]].Add(edge[1]);
        }

        for (int start = 0; start < nodeCount; start++)
        {
            var visited = new bool[nodeCount];
            var stack = new Stack<int>(next[start]);
            while (stack.Count > 0)
            {
                int node = stack.Pop();
                if (node == start)
                {
                    return true;
                }
                if (visited[node])
                {
                    continue;
                }
                visited[node] = true;
                foreach (int after in next[node])
                {
                    stack.Push(after);
                }
            }
        }
        return false;
    }

    [Approach(Name = "DFS With Three Colors", Time = "O(V + E)", Space = "O(V)", Idea = """
        Give every node a color:

        - **White** (0): not visited yet
        - **Gray** (1): on the current DFS path, still being explored
        - **Black** (2): fully explored, and no cycle goes through it

        Following an arrow to a **gray** node means you've come back to your own path: that's a cycle.
        Each node is fully explored only once.
        """)]
    public static bool HasCycleWithColors(int nodeCount, int[][] edges)
    {
        var next = new List<int>[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            next[node] = new List<int>();
        }
        foreach (int[] edge in edges)
        {
            next[edge[0]].Add(edge[1]);
        }

        int[] color = new int[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            if (color[node] == 0 && ReachesGray(node, next, color))
            {
                return true;
            }
        }
        return false;
    }

    private static bool ReachesGray(int node, List<int>[] next, int[] color)
    {
        color[node] = 1;
        foreach (int after in next[node])
        {
            if (color[after] == 1)
            {
                return true;
            }
            if (color[after] == 0 && ReachesGray(after, next, color))
            {
                return true;
            }
        }
        color[node] = 2;
        return false;
    }

    public static Example[] Examples =>
    [
        new([3, new[] { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 2, 0 } }], true),
        new([3, new[] { new[] { 0, 1 }, new[] { 0, 2 }, new[] { 1, 2 } }], false),
        new([4, new[] { new[] { 0, 1 }, new[] { 2, 3 }, new[] { 3, 2 } }], true),
    ];
}
