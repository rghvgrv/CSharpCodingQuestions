namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 11, Title = "Heap Sort", Level = Medium, Problem = """
    Sort the array in place using a **max-heap**: a tree stored in the array where every parent is bigger than its children.
    (See the *Heaps* topic for more about heaps.)
    """)]
public static class HeapSort
{
    [Approach(Name = "Build a Max-Heap, Then Extract", Time = "O(n log n)", Space = "O(1)", Idea = """
        The children of index `i` are at `2i + 1` and `2i + 2`.

        1. **Build the heap**: fix every parent from the bottom up, so the biggest item ends up at index 0.
        2. **Extract**: swap the biggest item (index 0) to the end of the array, shrink the heap by one, and let the new top **sink down** to its place.
        3. Repeat until the heap is empty. The end of the array fills up with the largest items in order.
        """)]
    public static int[] HeapSortArray(int[] numbers)
    {
        int n = numbers.Length;
        for (int parent = n / 2 - 1; parent >= 0; parent--)
        {
            SinkDown(numbers, parent, n);
        }

        for (int end = n - 1; end > 0; end--)
        {
            int temp = numbers[0];
            numbers[0] = numbers[end];
            numbers[end] = temp;
            SinkDown(numbers, 0, end);
        }
        return numbers;
    }

    private static void SinkDown(int[] numbers, int index, int heapSize)
    {
        while (true)
        {
            int largest = index;
            int left = 2 * index + 1;
            int right = 2 * index + 2;
            if (left < heapSize && numbers[left] > numbers[largest])
            {
                largest = left;
            }
            if (right < heapSize && numbers[right] > numbers[largest])
            {
                largest = right;
            }
            if (largest == index)
            {
                return;
            }

            int temp = numbers[index];
            numbers[index] = numbers[largest];
            numbers[largest] = temp;
            index = largest;
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 12, 11, 13, 5, 6, 7 }], new[] { 5, 6, 7, 11, 12, 13 }),
        new([new[] { 4, 10, 3, 5, 1 }], new[] { 1, 3, 4, 5, 10 }),
    ];
}
