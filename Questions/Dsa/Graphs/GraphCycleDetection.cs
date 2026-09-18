namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_05, "Cycle Detection (Undirected & Directed)", Medium,
"Detect whether a graph has a cycle, for an undirected graph and for a directed graph.")]
public static class GraphCycleDetection
{
    // Undirected: during DFS, reaching a visited node that isn't our parent means a cycle.
    public static bool UndirectedHasCycle(int n, int[][] edges)
    {
        var g = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
        foreach (var e in edges) { g[e[0]].Add(e[1]); g[e[1]].Add(e[0]); }
        var visited = new bool[n];

        bool Dfs(int node, int parent)
        {
            visited[node] = true;
            foreach (int next in g[node])
                if (!visited[next] ? Dfs(next, node) : next != parent) return true;
            return false;
        }
        return Enumerable.Range(0, n).Any(v => !visited[v] && Dfs(v, -1));
    }

    // Directed: three colors. White = unvisited, Gray = on the current DFS path, Black = done.
    // An edge to a Gray node is a back edge → cycle.
    public static bool DirectedHasCycle(int n, int[][] edges)
    {
        var g = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
        foreach (var e in edges) g[e[0]].Add(e[1]);
        var color = new int[n]; // 0 white, 1 gray, 2 black

        bool Dfs(int node)
        {
            color[node] = 1;
            foreach (int next in g[node])
                if (color[next] == 1 || (color[next] == 0 && Dfs(next))) return true;
            color[node] = 2;
            return false;
        }
        return Enumerable.Range(0, n).Any(v => color[v] == 0 && Dfs(v));
    }

    public static void Run()
    {
        Check("Undirected 0-1-2-0", UndirectedHasCycle(3, [[0, 1], [1, 2], [2, 0]]), true);
        Check("Undirected tree 0-1, 1-2, 1-3", UndirectedHasCycle(4, [[0, 1], [1, 2], [1, 3]]), false);
        Check("Directed 0→1→2→0", DirectedHasCycle(3, [[0, 1], [1, 2], [2, 0]]), true);
        Check("Directed 0→1, 0→2, 1→2 (diamond, no cycle)", DirectedHasCycle(3, [[0, 1], [0, 2], [1, 2]]), false);
    }
}
