namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_11, "Union-Find (Disjoint Set Union)", Medium,
"Build a disjoint-set structure with Find and Union. Use it to count connected components and to find the redundant edge that creates a cycle.")]
public static class UnionFind
{
    // Path compression + union by size → nearly O(1) per operation (inverse Ackermann).
    public class Dsu
    {
        readonly int[] parent, size;
        public int Components { get; private set; }

        public Dsu(int n)
        {
            parent = Enumerable.Range(0, n).ToArray();
            size = Enumerable.Repeat(1, n).ToArray();
            Components = n;
        }

        public int Find(int x) => parent[x] == x ? x : parent[x] = Find(parent[x]); // compress while returning

        public bool Union(int a, int b)
        {
            int ra = Find(a), rb = Find(b);
            if (ra == rb) return false; // already connected
            if (size[ra] < size[rb]) (ra, rb) = (rb, ra);
            parent[rb] = ra; // attach the smaller tree under the bigger
            size[ra] += size[rb];
            Components--;
            return true;
        }
    }

    public static int[] RedundantEdge(int[][] edges)
    {
        var dsu = new Dsu(edges.Length + 1);
        foreach (var e in edges)
            if (!dsu.Union(e[0], e[1])) return e;
        return [];
    }

    public static void Run()
    {
        var dsu = new Dsu(6);
        dsu.Union(0, 1); dsu.Union(1, 2); dsu.Union(3, 4);
        Check("Components after 0-1, 1-2, 3-4", dsu.Components, 3);
        Check("Connected(0, 2)", dsu.Find(0) == dsu.Find(2), true);
        Check("Connected(0, 3)", dsu.Find(0) == dsu.Find(3), false);
        Check("RedundantEdge([[1,2],[1,3],[2,3]])", RedundantEdge([[1, 2], [1, 3], [2, 3]]), [2, 3]);
        Check("RedundantEdge([[1,2],[2,3],[3,4],[1,4],[1,5]])", RedundantEdge([[1, 2], [2, 3], [3, 4], [1, 4], [1, 5]]), [1, 4]);
    }
}
