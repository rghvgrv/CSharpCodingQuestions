namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 9, Title = "Dijkstra's Shortest Paths", Level = Medium, Problem = """
    One-way roads `[from, to, cost]` connect nodes, and costs are never negative. Return the cheapest cost from `source` to every node
    (`-1` if unreachable).
    """)]
public static class DijkstraShortestPath
{
    [Approach(Name = "Dijkstra With an Array Scan", Time = "O(V² + E)", Space = "O(V + E)", Idea = """
        Keep a best-known cost for every node (∞ at first, 0 for the source). Repeat:

        1. Pick the unfinished node with the **smallest** known cost. Its cost is now final,
           because every other route would have to go through a more expensive node.
        2. **Relax** its roads: if `cost[node] + road` beats a neighbor's cost, update the neighbor.

        Finding the smallest by scanning all nodes costs `O(V)` each time.
        """)]
    public static long[] ShortestWithScan(int nodeCount, int[][] roads, int source)
    {
        var outgoing = BuildRoads(nodeCount, roads);
        long[] cost = new long[nodeCount];
        Array.Fill(cost, long.MaxValue);
        cost[source] = 0;
        var done = new bool[nodeCount];

        for (int round = 0; round < nodeCount; round++)
        {
            int node = -1;
            for (int candidate = 0; candidate < nodeCount; candidate++)
            {
                if (!done[candidate] && cost[candidate] != long.MaxValue && (node == -1 || cost[candidate] < cost[node]))
                {
                    node = candidate;
                }
            }
            if (node == -1)
            {
                break;
            }

            done[node] = true;
            foreach (var (to, roadCost) in outgoing[node])
            {
                cost[to] = Math.Min(cost[to], cost[node] + roadCost);
            }
        }
        return cost.Select(value => value == long.MaxValue ? -1 : value).ToArray();
    }

    private static List<(int To, int Cost)>[] BuildRoads(int nodeCount, int[][] roads)
    {
        var outgoing = new List<(int To, int Cost)>[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            outgoing[node] = new List<(int To, int Cost)>();
        }
        foreach (int[] road in roads)
        {
            outgoing[road[0]].Add((road[1], road[2]));
        }
        return outgoing;
    }

    [Approach(Name = "Dijkstra With a Priority Queue", Time = "O((V + E) log V)", Space = "O(V + E)", Idea = """
        Same algorithm, but a **min-heap** hands out the cheapest unfinished node in `O(log V)`.
        When a node's cost improves, push it again. Older, more expensive copies are skipped when they come out.
        """)]
    public static long[] ShortestWithHeap(int nodeCount, int[][] roads, int source)
    {
        var outgoing = new List<(int To, int Cost)>[nodeCount];
        for (int node = 0; node < nodeCount; node++)
        {
            outgoing[node] = new List<(int To, int Cost)>();
        }
        foreach (int[] road in roads)
        {
            outgoing[road[0]].Add((road[1], road[2]));
        }

        long[] cost = new long[nodeCount];
        Array.Fill(cost, long.MaxValue);
        cost[source] = 0;
        var heap = new PriorityQueue<int, long>();
        heap.Enqueue(source, 0);

        while (heap.TryDequeue(out int node, out long costSoFar))
        {
            if (costSoFar > cost[node])
            {
                continue;   // an outdated copy
            }
            foreach (var (to, roadCost) in outgoing[node])
            {
                long newCost = costSoFar + roadCost;
                if (newCost < cost[to])
                {
                    cost[to] = newCost;
                    heap.Enqueue(to, newCost);
                }
            }
        }
        return cost.Select(value => value == long.MaxValue ? -1 : value).ToArray();
    }

    public static Example[] Examples =>
    [
        new([5, new[] { new[] { 0, 1, 4 }, new[] { 0, 2, 1 }, new[] { 2, 1, 2 }, new[] { 1, 3, 1 }, new[] { 2, 3, 5 }, new[] { 3, 4, 3 } }, 0], new long[] { 0, 3, 1, 4, 7 }),
        new([3, new[] { new[] { 0, 1, 7 } }, 0], new long[] { 0, 7, -1 }),
    ];
}
