namespace CSharpCodingQuestions.Questions.Dsa.Graphs;

[Question(Order = 7, Title = "Course Schedule (Topological Sort)", Level = Medium, Problem = """
    There are `courseCount` courses. A pair `[a, b]` means "take `b` before `a`".
    Return an order in which you can take every course, or an empty list if it's impossible (the prerequisites form a loop).
    """)]
public static class CourseSchedule
{
    [Approach(Name = "Repeatedly Scan for a Free Course", Time = "O(V · (V + E))", Space = "O(V)", Idea = """
        Over and over, scan all courses for one that isn't taken yet and whose prerequisites are all taken.
        Take it and scan again. If a full scan finds nothing, there's a loop.
        """)]
    public static List<int> OrderByScanning(int courseCount, int[][] prerequisites)
    {
        var taken = new bool[courseCount];
        var order = new List<int>();

        while (order.Count < courseCount)
        {
            int found = -1;
            for (int course = 0; course < courseCount && found == -1; course++)
            {
                if (taken[course])
                {
                    continue;
                }
                bool ready = true;
                foreach (int[] pair in prerequisites)
                {
                    if (pair[0] == course && !taken[pair[1]])
                    {
                        ready = false;
                        break;
                    }
                }
                if (ready)
                {
                    found = course;
                }
            }

            if (found == -1)
            {
                return new List<int>();
            }
            taken[found] = true;
            order.Add(found);
        }
        return order;
    }

    [Approach(Name = "Kahn's Algorithm", Time = "O(V + E)", Space = "O(V + E)", Idea = """
        Count each course's missing prerequisites (its **in-degree**).

        1. Put every course with 0 missing prerequisites in a queue.
        2. Take a course from the queue and add it to the order. For every course that depends on it, reduce its count;
           when a count reaches 0, that course is ready, so queue it.
        3. If the order ends up shorter than the number of courses, some courses were stuck in a loop.
        """)]
    public static List<int> OrderWithKahn(int courseCount, int[][] prerequisites)
    {
        var unlocks = new List<int>[courseCount];
        for (int course = 0; course < courseCount; course++)
        {
            unlocks[course] = new List<int>();
        }
        int[] missing = new int[courseCount];
        foreach (int[] pair in prerequisites)
        {
            unlocks[pair[1]].Add(pair[0]);
            missing[pair[0]]++;
        }

        var ready = new Queue<int>();
        for (int course = 0; course < courseCount; course++)
        {
            if (missing[course] == 0)
            {
                ready.Enqueue(course);
            }
        }

        var order = new List<int>();
        while (ready.Count > 0)
        {
            int course = ready.Dequeue();
            order.Add(course);
            foreach (int next in unlocks[course])
            {
                missing[next]--;
                if (missing[next] == 0)
                {
                    ready.Enqueue(next);
                }
            }
        }
        return order.Count == courseCount ? order : new List<int>();
    }

    public static Example[] Examples =>
    [
        new([2, new[] { new[] { 1, 0 } }], new[] { 0, 1 }),
        new([4, new[] { new[] { 1, 0 }, new[] { 2, 0 }, new[] { 3, 1 }, new[] { 3, 2 } }], new[] { 0, 1, 2, 3 }),
        new([2, new[] { new[] { 1, 0 }, new[] { 0, 1 } }], Array.Empty<int>()),
    ];
}
