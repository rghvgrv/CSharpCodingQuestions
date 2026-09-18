namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 3, Title = "Shortest Path (Fewest Edges)", Level = Medium, Problem = """
    In an undirected graph, return the fewest edges needed to go from `from` to `to`, or `-1` if it's impossible.
    """)]
public static class ShortestPathUnweighted
{
    [Approach(Name = "Try Every Path (DFS)", Time = "Exponential", Space = "O(V)", Idea = """
        Explore every simple path from `from` with DFS, un-marking nodes when backing up so other paths can use them.
        Keep the shortest path that reaches `to`. The number of paths can explode.
        """)]
    public static int ShortestByTryingAllPaths(int nodeCount, int[][] edges, int from, int to)
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

        int best = int.MaxValue;
        var onPath = new bool[nodeCount];
        void Explore(int node, int length)
        {
            if (node == to)
            {
                best = Math.Min(best, length);
                return;
            }
            onPath[node] = true;
            foreach (int next in neighbors[node])
            {
                if (!onPath[next])
                {
                    Explore(next, length + 1);
                }
            }
            onPath[node] = false;
        }

        Explore(from, 0);
        return best == int.MaxValue ? -1 : best;
    }

    [Approach(Name = "BFS With Distances", Time = "O(V + E)", Space = "O(V)", Idea = """
        BFS visits nodes in order of distance: first everything 1 edge away, then 2 edges away, and so on.
        So the **first time** BFS reaches a node, it has found the shortest way there.
        Store `distance[next] = distance[node] + 1` when a node is first reached.
        """)]
    public static int ShortestWithBfs(int nodeCount, int[][] edges, int from, int to)
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

        int[] distance = new int[nodeCount];
        Array.Fill(distance, -1);
        distance[from] = 0;
        var queue = new Queue<int>();
        queue.Enqueue(from);

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            if (node == to)
            {
                return distance[node];
            }
            foreach (int next in neighbors[node])
            {
                if (distance[next] == -1)
                {
                    distance[next] = distance[node] + 1;
                    queue.Enqueue(next);
                }
            }
        }
        return -1;
    }

    public static Example[] Examples =>
    [
        new([6, new[] { new[] { 0, 1 }, new[] { 0, 2 }, new[] { 1, 3 }, new[] { 1, 4 }, new[] { 2, 4 }, new[] { 3, 5 }, new[] { 4, 5 } }, 0, 5], 3),
        new([4, new[] { new[] { 0, 1 }, new[] { 2, 3 } }, 0, 3], -1),
    ];
}
