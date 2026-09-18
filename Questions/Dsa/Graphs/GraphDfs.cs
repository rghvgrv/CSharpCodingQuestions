namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_02, "DFS & Connected Components", Easy,
"Traverse a graph depth-first (recursive and with an explicit stack) and count its connected components.")]
public static class GraphDfs
{
    public static List<int>[] Build(int n, int[][] edges)
    {
        var g = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
        foreach (var e in edges) { g[e[0]].Add(e[1]); g[e[1]].Add(e[0]); }
        return g;
    }

    // Go as deep as possible before backtracking. Time O(V + E)
    public static void DfsRecursive(List<int>[] g, int node, bool[] visited, List<int> order)
    {
        visited[node] = true;
        order.Add(node);
        foreach (int next in g[node])
            if (!visited[next]) DfsRecursive(g, next, visited, order);
    }

    // Same order with an explicit stack (no stack overflow on huge graphs). Push neighbors in reverse.
    public static List<int> DfsIterative(List<int>[] g, int start)
    {
        var order = new List<int>();
        var visited = new bool[g.Length];
        var stack = new Stack<int>([start]);
        while (stack.Count > 0)
        {
            int node = stack.Pop();
            if (visited[node]) continue;
            visited[node] = true;
            order.Add(node);
            for (int i = g[node].Count - 1; i >= 0; i--)
                if (!visited[g[node][i]]) stack.Push(g[node][i]);
        }
        return order;
    }

    // Each DFS started from an unvisited node discovers one whole component.
    public static int CountComponents(List<int>[] g)
    {
        var visited = new bool[g.Length];
        int count = 0;
        for (int v = 0; v < g.Length; v++)
            if (!visited[v]) { count++; DfsRecursive(g, v, visited, []); }
        return count;
    }

    public static void Run()
    {
        // 0 — 1 — 2    3 — 4    5
        //  \  |
        //    6
        var g = Build(7, [[0, 1], [1, 2], [0, 6], [1, 6], [3, 4]]);
        var order = new List<int>();
        DfsRecursive(g, 0, new bool[7], order);
        Check("DfsRecursive(0)", order, [0, 1, 2, 6]);
        Check("DfsIterative(0)", DfsIterative(g, 0), [0, 1, 2, 6]);
        Check("CountComponents", CountComponents(g), 3);
    }
}
