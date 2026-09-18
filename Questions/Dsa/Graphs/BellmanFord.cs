namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 10, Title = "Bellman–Ford (Negative Costs)", Level = Medium, Problem = """
    Like Dijkstra, but some road costs can be **negative** (think of a road that pays you). Return the cheapest cost from `source` to every node,
    or `null` if there's a **negative cycle**, a loop you could go around forever to make the cost smaller and smaller.
    """)]
public static class BellmanFord
{
    [Approach(Name = "Relax Every Road V − 1 Times", Time = "O(V · E)", Space = "O(V)", Idea = """
        Dijkstra's "smallest is final" rule breaks with negative costs. Bellman–Ford is more patient:

        1. Go through **every** road and relax it (improve the destination if possible). Repeat this `V - 1` times.
           A cheapest path uses at most `V - 1` roads, and each round fixes at least one more of them.
        2. Do one more round. If anything still improves, there's a negative cycle.
        """)]
    public static long[]? CheapestCosts(int nodeCount, int[][] roads, int source)
    {
        long[] cost = new long[nodeCount];
        Array.Fill(cost, long.MaxValue);
        cost[source] = 0;

        for (int round = 0; round < nodeCount - 1; round++)
        {
            foreach (int[] road in roads)
            {
                int from = road[0];
                int to = road[1];
                if (cost[from] != long.MaxValue && cost[from] + road[2] < cost[to])
                {
                    cost[to] = cost[from] + road[2];
                }
            }
        }

        foreach (int[] road in roads)
        {
            if (cost[road[0]] != long.MaxValue && cost[road[0]] + road[2] < cost[road[1]])
            {
                return null;
            }
        }
        return cost;
    }

    public static Example[] Examples =>
    [
        new([5, new[] { new[] { 0, 1, 6 }, new[] { 0, 2, 7 }, new[] { 1, 2, 8 }, new[] { 1, 3, 5 }, new[] { 1, 4, -4 }, new[] { 2, 3, -3 }, new[] { 2, 4, 9 }, new[] { 3, 1, -2 }, new[] { 4, 3, 7 } }, 0], new long[] { 0, 2, 7, 4, -2 }),
        new([3, new[] { new[] { 0, 1, 1 }, new[] { 1, 2, -3 }, new[] { 2, 1, 1 } }, 0], null),
    ];
}
