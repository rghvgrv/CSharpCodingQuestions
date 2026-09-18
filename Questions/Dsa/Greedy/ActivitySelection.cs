namespace CodingQuestions.Dsa.Greedy;

[Q(1_11_01, "Activity Selection / Meeting Rooms", Medium,
"(1) Choose the maximum number of non-overlapping meetings. (2) Find the minimum number of rooms needed to hold all meetings.")]
public static class ActivitySelection
{
    // Greedy: always pick the meeting that ends first. It leaves the most room for the rest.
    public static int MaxMeetings((int Start, int End)[] meetings)
    {
        int count = 0, freeAt = int.MinValue;
        foreach (var m in meetings.OrderBy(m => m.End))
        {
            if (m.Start < freeAt) continue;
            count++;
            freeAt = m.End;
        }
        return count;
    }

    // Min-heap of end times of rooms in use. A room frees up if its meeting ended before this one starts.
    public static int MinRooms((int Start, int End)[] meetings)
    {
        var endTimes = new PriorityQueue<int, int>();
        foreach (var m in meetings.OrderBy(m => m.Start))
        {
            if (endTimes.Count > 0 && endTimes.Peek() <= m.Start) endTimes.Dequeue();
            endTimes.Enqueue(m.End, m.End);
        }
        return endTimes.Count;
    }

    public static void Run()
    {
        Check("MaxMeetings((1,2),(3,4),(0,6),(5,7),(8,9),(5,9))", MaxMeetings([(1, 2), (3, 4), (0, 6), (5, 7), (8, 9), (5, 9)]), 4);
        Check("MinRooms((0,30),(5,10),(15,20))", MinRooms([(0, 30), (5, 10), (15, 20)]), 2);
        Check("MinRooms((7,10),(2,4))", MinRooms([(7, 10), (2, 4)]), 1);
    }
}
