namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 10, Title = "Product of Array Except Self", Level = Medium, Problem = """
    Return an array where each position holds the product of **all the other** numbers. Don't use division.
    `[1, 2, 3, 4]` → `[24, 12, 8, 6]` (for example `24 = 2 × 3 × 4`).
    """)]
public static class ProductExceptSelf
{
    [Approach(Name = "Multiply Everything Else", Time = "O(n²)", Space = "O(1) extra", Idea = """
        For each position, loop over the whole array and multiply every number except the one at that position.
        """)]
    public static int[] ProductBruteForce(int[] numbers)
    {
        int[] answer = new int[numbers.Length];
        for (int i = 0; i < numbers.Length; i++)
        {
            int product = 1;
            for (int j = 0; j < numbers.Length; j++)
            {
                if (j != i)
                {
                    product *= numbers[j];
                }
            }
            answer[i] = product;
        }
        return answer;
    }

    [Approach(Name = "Left and Right Products", Time = "O(n)", Space = "O(n)", Idea = """
        The answer at `i` = (product of everything **left** of `i`) × (product of everything **right** of `i`).

        1. `left[i]`: fill from left to right, `left[i] = left[i - 1] × numbers[i - 1]`.
        2. `right[i]`: fill from right to left the same way.
        3. Multiply them together.
        """)]
    public static int[] ProductWithTwoArrays(int[] numbers)
    {
        int n = numbers.Length;
        int[] left = new int[n];
        int[] right = new int[n];

        left[0] = 1;
        for (int i = 1; i < n; i++)
        {
            left[i] = left[i - 1] * numbers[i - 1];
        }

        right[n - 1] = 1;
        for (int i = n - 2; i >= 0; i--)
        {
            right[i] = right[i + 1] * numbers[i + 1];
        }

        int[] answer = new int[n];
        for (int i = 0; i < n; i++)
        {
            answer[i] = left[i] * right[i];
        }
        return answer;
    }

    [Approach(Name = "One Output Array", Time = "O(n)", Space = "O(1) extra", Idea = """
        Same idea without the two helper arrays: store the left products directly in the answer,
        then walk back from the right, keeping the right product in a single variable.
        """)]
    public static int[] ProductOptimized(int[] numbers)
    {
        int n = numbers.Length;
        int[] answer = new int[n];

        answer[0] = 1;
        for (int i = 1; i < n; i++)
        {
            answer[i] = answer[i - 1] * numbers[i - 1];
        }

        int rightProduct = 1;
        for (int i = n - 1; i >= 0; i--)
        {
            answer[i] *= rightProduct;
            rightProduct *= numbers[i];
        }
        return answer;
    }

    public static Example[] Examples =>
    [
        new([new[] { 1, 2, 3, 4 }], new[] { 24, 12, 8, 6 }),
        new([new[] { -1, 1, 0, -3, 3 }], new[] { 0, 0, 9, 0, 0 }),
    ];
}
