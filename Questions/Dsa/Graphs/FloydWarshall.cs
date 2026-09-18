namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_10, "Floyd–Warshall (All-Pairs Shortest Paths)", Medium,
"Compute the shortest distance between every pair of nodes.")]
public static class FloydWarshall
{
    // dist[i, j] improves if going through k is shorter: dist[i, k] + dist[k, j]. Try every k as the middle. Time O(V³)
    const int Inf = int.MaxValue / 2; // halved so Inf + Inf doesn't overflow

    public static int[,] Solve(int n, (int From, int To, int Weight)[] edges)
    {
        var d = new int[n, n];
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++) d[i, j] = i == j ? 0 : Inf;
        foreach (var (u, v, w) in edges) d[u, v] = Math.Min(d[u, v], w);

        for (int k = 0; k < n; k++)
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    d[i, j] = Math.Min(d[i, j], d[i, k] + d[k, j]);
        return d;
    }

    public static void Run()
    {
        var d = Solve(4, [(0, 1, 3), (1, 0, 2), (0, 3, 5), (1, 3, 4), (3, 2, 2), (2, 1, 1)]);
        for (int i = 0; i < 4; i++)
            Console.WriteLine("  " + string.Join("\t", Enumerable.Range(0, 4).Select(j => d[i, j] >= Inf ? "∞" : d[i, j].ToString())));
        Check("row 0", Enumerable.Range(0, 4).Select(j => d[0, j]), [0, 3, 7, 5]);
        Check("row 2", Enumerable.Range(0, 4).Select(j => d[2, j]), [3, 1, 0, 5]);
    }
}
