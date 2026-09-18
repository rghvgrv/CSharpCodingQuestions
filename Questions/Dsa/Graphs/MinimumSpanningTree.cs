namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 13, Title = "Minimum Spanning Tree", Level = Hard, Problem = """
    Towns `0..n-1` and possible two-way roads `[a, b, cost]`. Choose roads so every town is connected, with the smallest total cost.
    Return that cost.
    """)]
public static class MinimumSpanningTree
{
    [Approach(Name = "Prim's Algorithm", Time = "O(E log E)", Space = "O(V + E)", Idea = """
        Grow one connected tree from town 0. At every step, add the **cheapest road that leaves the tree** to a new town.
        A min-heap holds the candidate roads. Roads that lead back into the tree are skipped.
        """)]
    public static int TotalCostPrim(int townCount, int[][] roads)
    {
        var neighbors = new List<(int Town, int Cost)>[townCount];
        for (int town = 0; town < townCount; town++)
        {
            neighbors[town] = new List<(int Town, int Cost)>();
        }
        foreach (int[] road in roads)
        {
            neighbors[road[0]].Add((road[1], road[2]));
            neighbors[road[1]].Add((road[0], road[2]));
        }

        var inTree = new bool[townCount];
        var heap = new PriorityQueue<int, int>();
        heap.Enqueue(0, 0);
        int total = 0;

        while (heap.TryDequeue(out int town, out int cost))
        {
            if (inTree[town])
            {
                continue;
            }
            inTree[town] = true;
            total += cost;
            foreach (var (next, roadCost) in neighbors[town])
            {
                if (!inTree[next])
                {
                    heap.Enqueue(next, roadCost);
                }
            }
        }
        return total;
    }

    [Approach(Name = "Kruskal's Algorithm", Time = "O(E log E)", Space = "O(V)", Idea = """
        Sort all roads from cheapest to most expensive. Go through them and **keep a road if it connects two towns that aren't connected yet**.
        Union-Find (see *Connected Components*) answers "already connected?" instantly.
        Skipped roads would only create a loop. Same cost as Prim, and often simpler to write.
        """)]
    public static int TotalCostKruskal(int townCount, int[][] roads)
    {
        int[] leader = Enumerable.Range(0, townCount).ToArray();
        int Find(int town)
        {
            if (leader[town] != town)
            {
                leader[town] = Find(leader[town]);
            }
            return leader[town];
        }

        int total = 0;
        foreach (int[] road in roads.OrderBy(road => road[2]))
        {
            int a = Find(road[0]);
            int b = Find(road[1]);
            if (a != b)
            {
                leader[a] = b;
                total += road[2];
            }
        }
        return total;
    }

    public static Example[] Examples =>
    [
        new([4, new[] { new[] { 0, 1, 10 }, new[] { 0, 2, 6 }, new[] { 0, 3, 5 }, new[] { 1, 3, 15 }, new[] { 2, 3, 4 } }], 19),
        new([3, new[] { new[] { 0, 1, 1 }, new[] { 1, 2, 2 }, new[] { 0, 2, 3 } }], 3),
    ];
}
