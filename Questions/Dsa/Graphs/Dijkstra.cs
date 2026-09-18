namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_08, "Dijkstra's Shortest Path", Medium,
"Find the shortest distance from a source to every node in a graph with non-negative edge weights, and rebuild the path to one target.")]
public static class Dijkstra
{
    // Always expand the closest unfinished node (min-heap by distance).
    // Stale heap entries are skipped instead of decreased. Time O((V + E) log V)
    public static (long[] Dist, int[] Prev) Solve(int n, (int From, int To, int Weight)[] edges, int source)
    {
        var g = Enumerable.Range(0, n).Select(_ => new List<(int To, int W)>()).ToArray();
        foreach (var (u, v, w) in edges) g[u].Add((v, w));

        var dist = Enumerable.Repeat(long.MaxValue, n).ToArray();
        var prev = Enumerable.Repeat(-1, n).ToArray();
        var heap = new PriorityQueue<int, long>();
        dist[source] = 0;
        heap.Enqueue(source, 0);

        while (heap.TryDequeue(out int u, out long d))
        {
            if (d > dist[u]) continue; // stale entry
            foreach (var (v, w) in g[u])
            {
                if (d + w >= dist[v]) continue;
                dist[v] = d + w;
                prev[v] = u;
                heap.Enqueue(v, dist[v]);
            }
        }
        return (dist, prev);
    }

    public static void Run()
    {
        // Directed edges (from, to, weight). 0→1 costs 4 directly but only 3 via 2.
        (int, int, int)[] edges = [(0, 1, 4), (0, 2, 1), (2, 1, 2), (1, 3, 1), (2, 3, 5), (3, 4, 3)];
        var (dist, prev) = Solve(5, edges, 0);
        Check("dist from 0", dist, [0L, 3, 1, 4, 7]);

        var path = new List<int>();
        for (int v = 4; v != -1; v = prev[v]) path.Insert(0, v);
        Check("path 0 → 4", path, [0, 2, 1, 3, 4]);
    }
}
