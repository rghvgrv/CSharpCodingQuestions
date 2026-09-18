namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 1, Title = "Breadth-First Search (BFS)", Level = Easy, Problem = """
    An undirected graph has nodes `0` to `nodeCount - 1`, and `edges` like `[0, 1]` connect two nodes.
    Starting from `start`, list the nodes in **BFS order**: the start, then all its neighbors, then their neighbors, and so on.
    """)]
public static class BreadthFirstSearch
{
    [Approach(Name = "Queue + Visited", Time = "O(V + E)", Space = "O(V)", Idea = """
        1. Build an **adjacency list**: for every node, the list of its neighbors.
        2. Put `start` in a queue and mark it visited.
        3. Repeatedly take the next node from the queue, record it, and add its unvisited neighbors to the queue (marking them visited).

        The queue makes nodes come out in "rings" of distance 0, 1, 2, …
        Marking nodes when they're **added** (not when they're taken out) stops the same node from being queued twice.
        """)]
    public static List<int> BfsOrder(int nodeCount, int[][] edges, int start)
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

        var order = new List<int>();
        var visited = new bool[nodeCount];
        var queue = new Queue<int>();
        queue.Enqueue(start);
        visited[start] = true;

        while (queue.Count > 0)
        {
            int node = queue.Dequeue();
            order.Add(node);
            foreach (int next in neighbors[node])
            {
                if (!visited[next])
                {
                    visited[next] = true;
                    queue.Enqueue(next);
                }
            }
        }
        return order;
    }

    public static Example[] Examples =>
    [
        // 0 — 1 — 3
        // |   |   |
        // 2 — 4 — 5
        new([6, new[] { new[] { 0, 1 }, new[] { 0, 2 }, new[] { 1, 3 }, new[] { 1, 4 }, new[] { 2, 4 }, new[] { 3, 5 }, new[] { 4, 5 } }, 0], new[] { 0, 1, 2, 3, 4, 5 }),
        new([4, new[] { new[] { 0, 1 }, new[] { 1, 2 } }, 2], new[] { 2, 1, 0 }),
    ];
}
