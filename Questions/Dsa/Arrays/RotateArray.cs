namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 4, Title = "Rotate an Array", Level = Medium, Problem = """
    Rotate the array to the right by `k` steps. Items that fall off the end come back at the start.
    `[1, 2, 3, 4, 5, 6, 7]`, `k = 3` → `[5, 6, 7, 1, 2, 3, 4]`.
    """)]
public static class RotateArray
{
    [Approach(Name = "Rotate One Step, k Times", Time = "O(n · k)", Space = "O(1)", Idea = """
        Move the last item to the front, shifting every other item one place right. Repeat `k` times.
        Rotating by `n` gives back the same array, so first reduce `k` with `k % n`.
        """)]
    public static int[] RotateStepByStep(int[] numbers, int k)
    {
        k %= numbers.Length;
        for (int step = 0; step < k; step++)
        {
            int last = numbers[numbers.Length - 1];
            for (int i = numbers.Length - 1; i > 0; i--)
            {
                numbers[i] = numbers[i - 1];
            }
            numbers[0] = last;
        }
        return numbers;
    }

    [Approach(Name = "Extra Array", Time = "O(n)", Space = "O(n)", Idea = """
        Each item at index `i` belongs at index `(i + k) % n`. Place every item directly into a new array.
        """)]
    public static int[] RotateWithExtraArray(int[] numbers, int k)
    {
        int n = numbers.Length;
        int[] rotated = new int[n];
        for (int i = 0; i < n; i++)
        {
            rotated[(i + k) % n] = numbers[i];
        }
        return rotated;
    }

    [Approach(Name = "Three Reversals", Time = "O(n)", Space = "O(1)", Idea = """
        A neat trick that needs no extra array:

        1. Reverse the whole array: `[7, 6, 5, 4, 3, 2, 1]`
        2. Reverse the first `k` items: `[5, 6, 7, | 4, 3, 2, 1]`
        3. Reverse the rest: `[5, 6, 7, | 1, 2, 3, 4]`
        """)]
    public static int[] RotateByReversing(int[] numbers, int k)
    {
        k %= numbers.Length;
        Reverse(numbers, 0, numbers.Length - 1);
        Reverse(numbers, 0, k - 1);
        Reverse(numbers, k, numbers.Length - 1);
        return numbers;
    }

    private static void Reverse(int[] numbers, int left, int right)
    {
        while (left < right)
        {
            int temp = numbers[left];
            numbers[left] = numbers[right];
            numbers[right] = temp;
            left++;
            right--;
        }
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 4, 5, 6, 7 }, 3], new[] { 5, 6, 7, 1, 2, 3, 4 }),
        new([new[] { -1, -100, 3, 99 }, 2], new[] { 3, 99, -1, -100 }),
        new([new[] { 1, 2 }, 5], new[] { 2, 1 }),
    ];
}
