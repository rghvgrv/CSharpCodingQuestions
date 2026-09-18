namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_07, "Is Graph Bipartite?", Medium,
"Can the nodes be split into two groups so every edge connects the two groups? (Equivalently: can it be 2-colored?)")]
public static class BipartiteGraph
{
    // BFS coloring: give each neighbor the opposite color. A neighbor with the same color → not bipartite.
    // A graph is bipartite exactly when it has no odd-length cycle.
    public static bool Solve(int[][] graph)
    {
        var color = new int[graph.Length]; // 0 = uncolored, 1 / -1 = the two groups
        for (int start = 0; start < graph.Length; start++)
        {
            if (color[start] != 0) continue;
            color[start] = 1;
            var queue = new Queue<int>([start]);
            while (queue.Count > 0)
            {
                int node = queue.Dequeue();
                foreach (int next in graph[node])
                {
                    if (color[next] == color[node]) return false;
                    if (color[next] == 0) { color[next] = -color[node]; queue.Enqueue(next); }
                }
            }
        }
        return true;
    }

    public static void Run()
    {
        Check("square 0-1-2-3-0", Solve([[1, 3], [0, 2], [1, 3], [0, 2]]), true);
        Check("with a triangle", Solve([[1, 2, 3], [0, 2], [0, 1, 3], [0, 2]]), false);
    }
}
