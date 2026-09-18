namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 21, Title = "Merge Two Sorted Arrays", Level = Easy, Problem = """
    Both arrays are sorted. Combine them into one sorted array.
    `[1, 3, 5]` and `[2, 4, 6, 8]` → `[1, 2, 3, 4, 5, 6, 8]`.
    """)]
public static class MergeSortedArrays
{
    [Approach(Name = "Join, Then Sort", Time = "O((m + n) log(m + n))", Space = "O(m + n)", Idea = """
        Put both arrays together and sort the result. Simple, but it ignores that the inputs are already sorted.
        """)]
    public static int[] MergeBySorting(int[] first, int[] second)
    {
        int[] combined = new int[first.Length + second.Length];
        first.CopyTo(combined, 0);
        second.CopyTo(combined, first.Length);
        Array.Sort(combined);
        return combined;
    }

    [Approach(Name = "Two Pointers", Time = "O(m + n)", Space = "O(m + n)", Idea = """
        Keep one pointer in each array. Compare the two current items, copy the smaller one to the result, and move that pointer.
        When one array runs out, copy whatever is left of the other.

        It's the same "merge" step that merge sort uses.
        """)]
    public static int[] MergeWithTwoPointers(int[] first, int[] second)
    {
        int[] result = new int[first.Length + second.Length];
        int i = 0;
        int j = 0;
        int k = 0;

        while (i < first.Length && j < second.Length)
        {
            if (first[i] <= second[j])
            {
                result[k] = first[i];
                i++;
            }
            else
            {
                result[k] = second[j];
                j++;
            }
            k++;
        }

        while (i < first.Length)
        {
            result[k] = first[i];
            i++;
            k++;
        }
        while (j < second.Length)
        {
            result[k] = second[j];
            j++;
            k++;
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 3, 5 }, new[] { 2, 4, 6, 8 }], new[] { 1, 2, 3, 4, 5, 6, 8 }),
        new([new[] { 1, 2, 3 }, Array.Empty<int>()], new[] { 1, 2, 3 }),
    ];
}
