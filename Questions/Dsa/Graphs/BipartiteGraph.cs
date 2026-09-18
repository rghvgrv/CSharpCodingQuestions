namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 8, Title = "Is the Graph Bipartite?", Level = Medium, Problem = """
    Can the nodes be split into two teams so that every edge connects nodes of **different** teams?
    (`neighbors[i]` lists the nodes connected to node `i`.)
    """)]
public static class BipartiteGraph
{
    [Approach(Name = "Two-Coloring With BFS", Time = "O(V + E)", Space = "O(V)", Idea = """
        Color the start node team A. All its neighbors must be team B, their neighbors team A, and so on (BFS).
        If you ever find an edge whose two ends already have the **same** color, it's impossible.
        Start again from any uncolored node, because the graph may have separate pieces.

        (A graph is bipartite exactly when it has no cycle of odd length, like a triangle.)
        """)]
    public static bool IsBipartite(int[][] neighbors)
    {
        int[] team = new int[neighbors.Length];   // 0 = no team yet, 1 = team A, -1 = team B
        for (int start = 0; start < neighbors.Length; start++)
        {
            if (team[start] != 0)
            {
                continue;
            }

            team[start] = 1;
            var queue = new Queue<int>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                int node = queue.Dequeue();
                foreach (int next in neighbors[node])
                {
                    if (team[next] == team[node])
                    {
                        return false;
                    }
                    if (team[next] == 0)
                    {
                        team[next] = -team[node];
                        queue.Enqueue(next);
                    }
                }
            }
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new([new[] { new[] { 1, 3 }, new[] { 0, 2 }, new[] { 1, 3 }, new[] { 0, 2 } }], true),
        new([new[] { new[] { 1, 2, 3 }, new[] { 0, 2 }, new[] { 0, 1, 3 }, new[] { 0, 2 } }], false),
    ];
}
