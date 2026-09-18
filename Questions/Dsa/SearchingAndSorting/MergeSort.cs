namespace CSharpCodingQuestions.Questions.Dsa.SearchingAndSorting;

[Question(Order = 9, Title = "Merge Sort", Level = Medium, Problem = """
    Sort the array using merge sort: split it in half, sort each half, then merge the two sorted halves.
    """)]
public static class MergeSort
{
    [Approach(Name = "Divide and Conquer", Time = "O(n log n)", Space = "O(n)", Idea = """
        1. **Divide**: split the array into a left half and a right half.
        2. **Conquer**: sort each half the same way (recursion). An array of 1 item is already sorted.
        3. **Combine**: merge the two sorted halves by repeatedly taking the smaller front item.

        The array is halved `log n` times and each level of merging touches all `n` items, so it's `O(n log n)`, even in the worst case.
        """)]
    public static int[] MergeSortArray(int[] numbers)
    {
        if (numbers.Length <= 1)
        {
            return numbers;
        }

        int middle = numbers.Length / 2;
        int[] left = MergeSortArray(numbers[..middle]);
        int[] right = MergeSortArray(numbers[middle..]);
        return Merge(left, right);
    }

    private static int[] Merge(int[] left, int[] right)
    {
        int[] result = new int[left.Length + right.Length];
        int i = 0;
        int j = 0;
        int k = 0;

        while (i < left.Length && j < right.Length)
        {
            if (left[i] <= right[j])
            {
                result[k++] = left[i++];
            }
            else
            {
                result[k++] = right[j++];
            }
        }
        while (i < left.Length)
        {
            result[k++] = left[i++];
        }
        while (j < right.Length)
        {
            result[k++] = right[j++];
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { 38, 27, 43, 3, 9, 82, 10 }], new[] { 3, 9, 10, 27, 38, 43, 82 }),
        new([new[] { 5, 2, 5, 1 }], new[] { 1, 2, 5, 5 }),
    ];
}
