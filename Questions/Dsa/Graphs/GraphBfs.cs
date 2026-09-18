namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_01, "Graph Representation & BFS", Easy,
"Store an undirected graph as an adjacency list. Traverse it breadth-first and find the shortest path (fewest edges) between two nodes.")]
public static class GraphBfs
{
    // Adjacency list: node → neighbors. Space O(V + E). (Adjacency matrix is O(V²).)
    public static Dictionary<int, List<int>> Build(int[][] edges)
    {
        var g = new Dictionary<int, List<int>>();
        foreach (var e in edges)
        {
            (g.TryGetValue(e[0], out var a) ? a : g[e[0]] = []).Add(e[1]);
            (g.TryGetValue(e[1], out var b) ? b : g[e[1]] = []).Add(e[0]);
        }
        return g;
    }

    // BFS visits nodes in order of distance from the start: a queue plus a visited set. Time O(V + E)
    public static List<int> Bfs(Dictionary<int, List<int>> g, int start)
    {
        var order = new List<int>();
        var visited = new HashSet<int> { start };
        var queue = new Queue<int>([start]);
        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            order.Add(node);
            foreach (int next in g[node])
                if (visited.Add(next)) queue.Enqueue(next);
        }
        return order;
    }

    // BFS reaches each node first by a shortest route. Remember each node's parent, then walk back from the target.
    public static List<int> ShortestPath(Dictionary<int, List<int>> g, int from, int to)
    {
        var parent = new Dictionary<int, int> { [from] = from };
        var queue = new Queue<int>([from]);
        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            if (node == to) break;
            foreach (int next in g[node])
                if (parent.TryAdd(next, node)) queue.Enqueue(next);
        }
        if (!parent.ContainsKey(to)) return [];
        var path = new List<int> { to };
        while (path[^1] != from) path.Add(parent[path[^1]]);
        path.Reverse();
        return path;
    }

    public static void Run()
    {
        //   0 — 1 — 3
        //   |   |   |
        //   2 — 4 — 5
        var g = Build([[0, 1], [0, 2], [1, 3], [1, 4], [2, 4], [3, 5], [4, 5]]);
        Check("Bfs(0)", Bfs(g, 0), [0, 1, 2, 3, 4, 5]);
        Check("ShortestPath(0 → 5)", ShortestPath(g, 0, 5), [0, 1, 3, 5]);
        Check("ShortestPath(2 → 3)", ShortestPath(g, 2, 3), [2, 0, 1, 3]);
    }
}
