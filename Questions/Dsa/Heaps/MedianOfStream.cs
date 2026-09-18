namespace CSharpCodingQuestions.Questions.Dsa.Heaps;

[Question(Order = 5, Title = "Running Median of a Stream", Level = Hard, Problem = """
    Numbers arrive one at a time. After each one, report the median of everything so far:
    the middle value, or the average of the two middle values.
    """)]
public static class MedianOfStream
{
    [Approach(Name = "Keep a Sorted List", Time = "Add O(n), Median O(1)", Space = "O(n)", Idea = """
        Insert each number at its sorted position (binary search finds it, but inserting still shifts items). The median is in the middle of the list.
        """)]
    public class SortedListMedian
    {
        private readonly List<int> sorted = new();

        public void Add(int number)
        {
            int index = sorted.BinarySearch(number);
            if (index < 0)
            {
                index = ~index;   // BinarySearch returns the insert position as a negative number
            }
            sorted.Insert(index, number);
        }

        public double Median()
        {
            int middle = sorted.Count / 2;
            return sorted.Count % 2 == 1 ? sorted[middle] : (sorted[middle - 1] + sorted[middle]) / 2.0;
        }
    }

    [Approach(Name = "Two Heaps", Time = "Add O(log n), Median O(1)", Space = "O(n)", Idea = """
        Split the numbers into two halves:

        - `lowerHalf`: a **max-heap** of the smaller half (its top is the biggest of the small numbers)
        - `upperHalf`: a **min-heap** of the larger half (its top is the smallest of the big numbers)

        Keep the sizes equal, or `lowerHalf` one bigger. Then the median is right at the tops.
        To add: push into `lowerHalf`, move its top to `upperHalf` (keeping the order right), and if `upperHalf` got bigger, move its top back.
        """)]
    public class TwoHeapMedian
    {
        private readonly PriorityQueue<int, int> lowerHalf = new(Comparer<int>.Create((a, b) => b.CompareTo(a)));
        private readonly PriorityQueue<int, int> upperHalf = new();

        public void Add(int number)
        {
            lowerHalf.Enqueue(number, number);
            int biggestLow = lowerHalf.Dequeue();
            upperHalf.Enqueue(biggestLow, biggestLow);

            if (upperHalf.Count > lowerHalf.Count)
            {
                int smallestHigh = upperHalf.Dequeue();
                lowerHalf.Enqueue(smallestHigh, smallestHigh);
            }
        }

        public double Median()
        {
            if (lowerHalf.Count > upperHalf.Count)
            {
                return lowerHalf.Peek();
            }
            return (lowerHalf.Peek() + upperHalf.Peek()) / 2.0;
        }
    }

    public static void Demo()
    {
        var fast = new TwoHeapMedian();
        var simple = new SortedListMedian();
        var medians = new List<double>();
        foreach (int number in new[] { 5, 15, 1, 3, 8, 7 })
        {
            fast.Add(number);
            simple.Add(number);
            medians.Add(fast.Median());
        }
        Print("Add 5, 15, 1, 3, 8, 7 → median after each", medians, expected: new[] { 5.0, 10, 5, 4, 5, 6 });
        Print("SortedListMedian gives the same final median", simple.Median(), expected: 6.0);
    }
}
