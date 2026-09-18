namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 19, Title = "Spiral Order of a Matrix", Level = Medium, Problem = """
    Return all items of a grid in spiral order: along the top row, down the right side, back along the bottom, up the left side, and repeat inward.
    `[[1, 2, 3], [4, 5, 6], [7, 8, 9]]` → `[1, 2, 3, 6, 9, 8, 7, 4, 5]`.
    """)]
public static class SpiralMatrix
{
    [Approach(Name = "Shrinking Boundaries", Time = "O(rows × columns)", Space = "O(1) extra", Idea = """
        Keep four borders: `top`, `bottom`, `left`, `right`. Walk one side, then move that border inward:

        1. Top row, left → right, then `top++`
        2. Right column, top → bottom, then `right--`
        3. Bottom row, right → left, then `bottom--`
        4. Left column, bottom → top, then `left++`

        Stop when the borders cross. The two `if` checks stop a single leftover row or column from being read twice.
        """)]
    public static List<int> SpiralOrder(int[][] matrix)
    {
        var result = new List<int>();
        int top = 0;
        int bottom = matrix.Length - 1;
        int left = 0;
        int right = matrix[0].Length - 1;

        while (top <= bottom && left <= right)
        {
            for (int column = left; column <= right; column++)
            {
                result.Add(matrix[top][column]);
            }
            top++;

            for (int row = top; row <= bottom; row++)
            {
                result.Add(matrix[row][right]);
            }
            right--;

            if (top <= bottom)
            {
                for (int column = right; column >= left; column--)
                {
                    result.Add(matrix[bottom][column]);
                }
                bottom--;
            }

            if (left <= right)
            {
                for (int row = bottom; row >= top; row--)
                {
                    result.Add(matrix[row][left]);
                }
                left++;
            }
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } }], new[] { 1, 2, 3, 6, 9, 8, 7, 4, 5 }),
        new([new[] { new[] { 1, 2, 3, 4 }, new[] { 5, 6, 7, 8 }, new[] { 9, 10, 11, 12 } }], new[] { 1, 2, 3, 4, 8, 12, 11, 10, 9, 5, 6, 7 }),
    ];
}
