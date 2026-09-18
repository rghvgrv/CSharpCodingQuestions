namespace CSharpCodingQuestions.Questions.Dsa.StacksAndQueues;

[Question(Order = 8, Title = "Sliding Window Maximum", Level = Hard, Problem = """
    A window of size `k` slides across the array one step at a time. Return the maximum of each window.
    `[1, 3, -1, -3, 5, 3, 6, 7]`, `k = 3` → `[3, 3, 5, 5, 6, 7]`.
    """)]
public static class SlidingWindowMaximum
{
    [Approach(Name = "Scan Every Window", Time = "O(n · k)", Space = "O(1) extra", Idea = """
        For each window position, look at its `k` items and take the largest.
        """)]
    public static List<int> MaxBruteForce(int[] numbers, int k)
    {
        var result = new List<int>();
        for (int start = 0; start + k <= numbers.Length; start++)
        {
            int max = numbers[start];
            for (int i = start; i < start + k; i++)
            {
                max = Math.Max(max, numbers[i]);
            }
            result.Add(max);
        }
        return result;
    }

    [Approach(Name = "Monotonic Deque", Time = "O(n)", Space = "O(k)", Idea = """
        Keep a deque (double-ended queue) of **positions** whose values decrease from front to back. The front is the window's maximum.

        For each new number:

        1. Drop the front if it has slid out of the window.
        2. Drop positions from the back while their value is ≤ the new number. They can never be a maximum again, because a bigger and newer number is here.
        3. Add the new position at the back.

        Every position enters and leaves the deque once, so this is linear.
        """)]
    public static List<int> MaxWithDeque(int[] numbers, int k)
    {
        var result = new List<int>();
        var deque = new LinkedList<int>();

        for (int i = 0; i < numbers.Length; i++)
        {
            if (deque.Count > 0 && deque.First!.Value <= i - k)
            {
                deque.RemoveFirst();
            }
            while (deque.Count > 0 && numbers[deque.Last!.Value] <= numbers[i])
            {
                deque.RemoveLast();
            }
            deque.AddLast(i);

            if (i >= k - 1)
            {
                result.Add(numbers[deque.First!.Value]);
            }
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 3, -1, -3, 5, 3, 6, 7 }, 3], new[] { 3, 3, 5, 5, 6, 7 }),
        new([new[] { 9, 8, 7, 6 }, 2], new[] { 9, 8, 7 }),
        new([new[] { 4 }, 1], new[] { 4 }),
    ];
}
