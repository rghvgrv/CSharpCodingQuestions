namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 12, Title = "Connected Components (Union-Find)", Level = Medium, Problem = """
    Count the separate groups in an undirected graph, where nodes in a group are connected directly or through others.
    This also introduces **Union-Find**, which answers "are these two connected?" almost instantly while edges keep being added.
    """)]
public static class ConnectedComponents
{
    [Approach(Name = "Union-Find Without Tricks", Time = "O(V · E) worst", Space = "O(V)", Idea = """
        Every node points to a **parent**; a node pointing to itself is the **leader** (root) of its group.

        - `Find(x)`: follow parents up to the leader.
        - `Union(a, b)`: if the leaders differ, make one leader point to the other. Two groups became one.

        Count the unions that actually merged: `groups = nodes - merges`.
        Without tricks, the parent chains can get long, and `Find` walks them every time.
        """)]
    public static int CountGroupsSimple(int nodeCount, int[][] edges)
    {
        int[] parent = Enumerable.Range(0, nodeCount).ToArray();
        int groups = nodeCount;
        foreach (int[] edge in edges)
        {
            int leaderA = FindSimple(parent, edge[0]);
            int leaderB = FindSimple(parent, edge[1]);
            if (leaderA != leaderB)
            {
                parent[leaderA] = leaderB;
                groups--;
            }
        }
        return groups;
    }

    private static int FindSimple(int[] parent, int node)
    {
        while (parent[node] != node)
        {
            node = parent[node];
        }
        return node;
    }

    [Approach(Name = "DFS Over Each Group", Time = "O(V + E)", Space = "O(V + E)", Idea = """
        Build an adjacency list. Each time you find an unvisited node, that's a new group:
        count it and visit its whole group with DFS. Great when you have all the edges up front.
        """)]
    public static int CountGroupsWithDfs(int nodeCount, int[][] edges)
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

        var visited = new bool[nodeCount];
        int groups = 0;
        for (int start = 0; start < nodeCount; start++)
        {
            if (visited[start])
            {
                continue;
            }
            groups++;
            var stack = new Stack<int>();
            stack.Push(start);
            visited[start] = true;
            while (stack.Count > 0)
            {
                foreach (int next in neighbors[stack.Pop()])
                {
                    if (!visited[next])
                    {
                        visited[next] = true;
                        stack.Push(next);
                    }
                }
            }
        }
        return groups;
    }

    [Approach(Name = "Union-Find With Path Compression and Union by Size", Time = "Almost O(1) per edge", Space = "O(V)", Idea = """
        Two small tricks keep the parent chains extremely short:

        1. **Path compression**: after `Find` walks up to the leader, point every node on the way **directly** at the leader.
        2. **Union by size**: always attach the **smaller** group under the bigger one, so trees stay flat.

        Together, each operation is practically constant time. It also works when edges arrive one by one,
        which is where DFS would have to start over each time.
        """)]
    public static int CountGroupsFast(int nodeCount, int[][] edges)
    {
        int[] parent = Enumerable.Range(0, nodeCount).ToArray();
        int[] size = Enumerable.Repeat(1, nodeCount).ToArray();
        int groups = nodeCount;

        foreach (int[] edge in edges)
        {
            int leaderA = Find(parent, edge[0]);
            int leaderB = Find(parent, edge[1]);
            if (leaderA == leaderB)
            {
                continue;
            }
            if (size[leaderA] < size[leaderB])
            {
                (leaderA, leaderB) = (leaderB, leaderA);   // make leaderA the bigger group
            }
            parent[leaderB] = leaderA;
            size[leaderA] += size[leaderB];
            groups--;
        }
        return groups;
    }

    private static int Find(int[] parent, int node)
    {
        if (parent[node] != node)
        {
            parent[node] = Find(parent, parent[node]);   // path compression
        }
        return parent[node];
    }

    public static Example[] Examples =>
    [
        new([5, new[] { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 3, 4 } }], 2),
        new([5, new[] { new[] { 0, 1 }, new[] { 1, 2 }, new[] { 2, 3 }, new[] { 3, 4 } }], 1),
        new([4, Array.Empty<int[]>()], 4),
    ];
}
