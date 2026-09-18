namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 2, Title = "Depth-First Search (DFS)", Level = Easy, Problem = """
    List the nodes of an undirected graph in **DFS order** from `start`: go as deep as possible along one path before backing up to try the next.
    """)]
public static class DepthFirstSearch
{
    [Approach(Name = "Recursion", Time = "O(V + E)", Space = "O(V)", Idea = """
        Visit a node: mark it, record it, then visit each unvisited neighbor, one after another.
        The recursion itself remembers the way back. Very deep graphs could overflow the call stack.
        """)]
    public static List<int> DfsRecursive(int nodeCount, int[][] edges, int start)
    {
        List<int>[] neighbors = BuildNeighbors(nodeCount, edges);
        var order = new List<int>();
        Visit(start, neighbors, new bool[nodeCount], order);
        return order;
    }

    private static void Visit(int node, List<int>[] neighbors, bool[] visited, List<int> order)
    {
        visited[node] = true;
        order.Add(node);
        foreach (int next in neighbors[node])
        {
            if (!visited[next])
            {
                Visit(next, neighbors, visited, order);
            }
        }
    }

    private static List<int>[] BuildNeighbors(int nodeCount, int[][] edges)
    {
        var neighbors = new List<int>[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            neighbors[node] = new List<int>();
        }
        foreach (int[] edge in edges)
        {
            neighbors[edge[0]].Add(edge[1]);
            neighbors[edge[1]].Add(edge[0]);
        }
        return neighbors;
    }

    [Approach(Name = "Explicit Stack", Time = "O(V + E)", Space = "O(V + E)", Idea = """
        Replace the recursion with your own `Stack`, so huge graphs can't overflow the call stack.
        Push the neighbors in **reverse** order, so the first neighbor is popped first and the order matches the recursive version.
        A node can be pushed more than once, so skip it if it's already visited when popped.
        """)]
    public static List<int> DfsWithStack(int nodeCount, int[][] edges, int start)
    {
        var neighbors = new List<int>[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            neighbors[node] = new List<int>();
        }
        foreach (int[] edge in edges)
        {
            neighbors[edge[0]].Add(edge[1]);
            neighbors[edge[1]].Add(edge[0]);
        }

        var order = new List<int>();
        var visited = new bool[nodeCount];
        var stack = new Stack<int>();
        stack.Push(start);

        while (stack.Count > 0)
        {
            int node = stack.Pop();
            if (visited[node])
            {
                continue;
            }
            visited[node] = true;
            order.Add(node);

            for (int i = neighbors[node].Count - 1; i >= 0; i--)
            {
                if (!visited[neighbors[node][i]])
                {
                    stack.Push(neighbors[node][i]);
                }
            }
        }
        return order;
    }

    public static Example[] Examples =>
    [
        // 0 — 1 — 3
        // |   |   |
        // 2 — 4 — 5
        new([6, new[] { new[] { 0, 1 }, new[] { 0, 2 }, new[] { 1, 3 }, new[] { 1, 4 }, new[] { 2, 4 }, new[] { 3, 5 }, new[] { 4, 5 } }, 0], new[] { 0, 1, 3, 5, 4, 2 }),
        new([3, new[] { new[] { 0, 1 }, new[] { 0, 2 } }, 1], new[] { 1, 0, 2 }),
    ];
}
