namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_09, "Bellman–Ford (Negative Weights)", Medium,
"Find shortest distances from a source when edges can be negative, and detect a negative cycle.")]
public static class BellmanFord
{
    // Relax every edge V-1 times (a shortest path has at most V-1 edges).
    // If a V-th round still improves something, there's a negative cycle. Time O(V·E)
    public static long[]? Solve(int n, (int From, int To, int Weight)[] edges, int source)
    {
        var dist = Enumerable.Repeat(long.MaxValue, n).ToArray();
        dist[source] = 0;
        for (int round = 0; round < n - 1; round++)
            foreach (var (u, v, w) in edges)
                if (dist[u] != long.MaxValue && dist[u] + w < dist[v]) dist[v] = dist[u] + w;

        foreach (var (u, v, w) in edges)
            if (dist[u] != long.MaxValue && dist[u] + w < dist[v]) return null; // negative cycle
        return dist;
    }

    public static void Run()
    {
        Check("with negative edges", Solve(5, [(0, 1, 6), (0, 2, 7), (1, 2, 8), (1, 3, 5), (1, 4, -4), (2, 3, -3), (2, 4, 9), (3, 1, -2), (4, 3, 7)], 0), [0L, 2, 7, 4, -2]);
        Check("negative cycle 1→2→1", Solve(3, [(0, 1, 1), (1, 2, -3), (2, 1, 1)], 0), null);
    }
}
