namespace CodingQuestions.Dsa.Graphs;

[Q(1_13_06, "Topological Sort / Course Schedule", Medium,
"There are n courses; prerequisites [a, b] mean take b before a. Return a valid order to take all courses, or an empty list if impossible (a cycle).")]
public static class TopologicalSort
{
    // Kahn's algorithm: repeatedly take a course with no remaining prerequisites (in-degree 0).
    // If some courses are never freed, there's a cycle. Time O(V + E)
    public static List<int> Solve(int n, int[][] prerequisites)
    {
        var next = Enumerable.Range(0, n).Select(_ => new List<int>()).ToArray();
        var inDegree = new int[n];
        foreach (var p in prerequisites)
        {
            next[p[1]].Add(p[0]);
            inDegree[p[0]]++;
        }

        var queue = new Queue<int>(Enumerable.Range(0, n).Where(v => inDegree[v] == 0));
        var order = new List<int>();
        while (queue.Count > 0)
        {
            int course = queue.Dequeue();
            order.Add(course);
            foreach (int c in next[course])
                if (--inDegree[c] == 0) queue.Enqueue(c);
        }
        return order.Count == n ? order : [];
    }

    public static void Run()
    {
        Check("n=2, [[1,0]]", Solve(2, [[1, 0]]), [0, 1]);
        Check("n=4, [[1,0],[2,0],[3,1],[3,2]]", Solve(4, [[1, 0], [2, 0], [3, 1], [3, 2]]), [0, 1, 2, 3]);
        Check("n=2, [[1,0],[0,1]] (cycle)", Solve(2, [[1, 0], [0, 1]]), []);
    }
}
