namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_12, "Minimum Spanning Tree (Kruskal & Prim)", Hard,
"Connect all nodes of a weighted undirected graph with minimum total edge weight. Solve with Kruskal's and with Prim's algorithm.")]
public static class MinimumSpanningTree
{
    // Kruskal: sort edges by weight; take an edge if it joins two different components (Union-Find). O(E log E)
    public static (int Cost, List<(int, int, int)> Edges) Kruskal(int n, (int U, int V, int W)[] edges)
    {
        var parent = Enumerable.Range(0, n).ToArray();
        int Find(int x) => parent[x] == x ? x : parent[x] = Find(parent[x]);

        var chosen = new List<(int, int, int)>();
        int cost = 0;
        foreach (var e in edges.OrderBy(e => e.W))
        {
            int a = Find(e.U), b = Find(e.V);
            if (a == b) continue; // would create a cycle
            parent[a] = b;
            chosen.Add(e);
            cost += e.W;
        }
        return (cost, chosen);
    }

    // Prim: grow one tree from node 0; always add the cheapest edge leaving the tree (min-heap). O(E log V)
    public static int Prim(int n, (int U, int V, int W)[] edges)
    {
        var g = Enumerable.Range(0, n).Select(_ => new List<(int To, int W)>()).ToArray();
        foreach (var (u, v, w) in edges) { g[u].Add((v, w)); g[v].Add((u, w)); }

        var inTree = new bool[n];
        var heap = new PriorityQueue<int, int>();
        heap.Enqueue(0, 0);
        int cost = 0;
        while (heap.TryDequeue(out int node, out int w))
        {
            if (inTree[node]) continue;
            inTree[node] = true;
            cost += w;
            foreach (var (next, nw) in g[node])
                if (!inTree[next]) heap.Enqueue(next, nw);
        }
        return cost;
    }

    public static void Run()
    {
        (int, int, int)[] edges = [(0, 1, 10), (0, 2, 6), (0, 3, 5), (1, 3, 15), (2, 3, 4)];
        var (cost, chosen) = Kruskal(4, edges);
        Check("Kruskal cost", cost, 19);
        Check("Kruskal edges (u, v, w)", chosen, [(2, 3, 4), (0, 3, 5), (0, 1, 10)]);
        Check("Prim cost", Prim(4, edges), 19);
    }
}
