namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 11, Title = "Floyd–Warshall (All Pairs)", Level = Medium, Problem = """
    Return a table with the cheapest cost between **every pair** of nodes (`-1` where there's no route). Roads are one-way `[from, to, cost]`.
    """)]
public static class FloydWarshall
{
    [Approach(Name = "Try Every Node as a Stopover", Time = "O(V³)", Space = "O(V²)", Idea = """
        Start with the direct road costs. Then, for each node `k`, ask for every pair `(i, j)`:
        *is going `i → k → j` cheaper than the best route found so far?*

        After trying every node as the stopover, the table holds the best routes.
        Three nested loops and only a few lines of code, which makes it great for small graphs.
        """)]
    public static int[][] AllPairsCheapest(int nodeCount, int[][] roads)
    {
        const int Unreachable = int.MaxValue / 2;   // halved, so adding two of them can't overflow
        int[][] cost = new int[nodeCount][];
        for (int i = 0; i < nodeCount; i++)
        {
            cost[i] = new int[nodeCount];
            for (int j = 0; j < nodeCount; j++)
            {
                cost[i][j] = i == j ? 0 : Unreachable;
            }
        }
        foreach (int[] road in roads)
        {
            cost[road[0]][road[1]] = Math.Min(cost[road[0]][road[1]], road[2]);
        }

        for (int stop = 0; stop < nodeCount; stop++)
        {
            for (int i = 0; i < nodeCount; i++)
            {
                for (int j = 0; j < nodeCount; j++)
                {
                    cost[i][j] = Math.Min(cost[i][j], cost[i][stop] + cost[stop][j]);
                }
            }
        }

        foreach (int[] row in cost)
        {
            for (int j = 0; j < nodeCount; j++)
            {
                if (row[j] >= Unreachable)
                {
                    row[j] = -1;
                }
            }
        }
        return cost;
    }

    public static Example[] Examples =>
    [
        new([4, new[] { new[] { 0, 1, 3 }, new[] { 1, 0, 2 }, new[] { 0, 3, 5 }, new[] { 1, 3, 4 }, new[] { 3, 2, 2 }, new[] { 2, 1, 1 } }],
            new[] { new[] { 0, 3, 7, 5 }, new[] { 2, 0, 6, 4 }, new[] { 3, 1, 0, 5 }, new[] { 5, 3, 2, 0 } }),
        new([2, Array.Empty<int[]>()], new[] { new[] { 0, -1 }, new[] { -1, 0 } }),
    ];
}
