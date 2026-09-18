namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 3, Title = "Reverse an Array", Level = Easy, Problem = """
    Reverse the order of the items: `[1, 2, 3, 4, 5]` → `[5, 4, 3, 2, 1]`.
    """)]
public static class ReverseArray
{
    [Approach(Name = "Copy Into a New Array", Time = "O(n)", Space = "O(n)", Idea = """
        Create a new array of the same size and fill it from the back: the first item goes to the last slot, and so on.
        """)]
    public static int[] ReverseIntoNewArray(int[] numbers)
    {
        int[] reversed = new int[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            reversed[numbers.Length - 1 - i] = numbers[i];
        }
        return reversed;
    }

    [Approach(Name = "Two Pointers In Place", Time = "O(n)", Space = "O(1)", Idea = """
        Swap the first and last items, then the second and second-last, moving inward until the pointers meet.
        The same array is changed, so no extra memory is needed.
        """)]
    public static int[] ReverseInPlace(int[] numbers)
    {
        int left = 0;
        int right = numbers.Length - 1;
        while (left < right)
        {
            int temp = numbers[left];
            numbers[left] = numbers[right];
            numbers[right] = temp;
            left++;
            right--;
        }
        return numbers;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 4, 5 }], new[] { 5, 4, 3, 2, 1 }),
        new([new[] { 10, 20 }], new[] { 20, 10 }),
    ];
}
