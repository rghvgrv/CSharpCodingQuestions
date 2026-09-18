namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 20, Title = "Rotate a Matrix 90°", Level = Medium, Problem = """
    Rotate a square grid 90 degrees clockwise.
    `[[1, 2, 3], [4, 5, 6], [7, 8, 9]]` → `[[7, 4, 1], [8, 5, 2], [9, 6, 3]]`.
    """)]
public static class RotateImage
{
    [Approach(Name = "Copy Into a New Matrix", Time = "O(n²)", Space = "O(n²)", Idea = """
        Look at where each cell goes: the item at row `r`, column `c` moves to row `c`, column `n - 1 - r`.
        Write every item into its new place in a fresh grid.
        """)]
    public static int[][] RotateWithCopy(int[][] matrix)
    {
        int n = matrix.Length;
        int[][] rotated = new int[n][];
        for (int row = 0; row < n; row++)
        {
            rotated[row] = new int[n];
        }

        for (int row = 0; row < n; row++)
        {
            for (int column = 0; column < n; column++)
            {
                rotated[column][n - 1 - row] = matrix[row][column];
            }
        }
        return rotated;
    }

    [Approach(Name = "Transpose, Then Reverse Each Row", Time = "O(n²)", Space = "O(1)", Idea = """
        Two simple steps in place give a clockwise rotation:

        1. **Transpose**: swap across the diagonal, so rows become columns (`matrix[r][c]` ↔ `matrix[c][r]`).
        2. **Reverse** every row.
        """)]
    public static int[][] RotateInPlace(int[][] matrix)
    {
        int n = matrix.Length;
        for (int row = 0; row < n; row++)
        {
            for (int column = row + 1; column < n; column++)
            {
                int temp = matrix[row][column];
                matrix[row][column] = matrix[column][row];
                matrix[column][row] = temp;
            }
        }

        foreach (int[] row in matrix)
        {
            Array.Reverse(row);
        }
        return matrix;
    }

    public static Example[] Examples =>
    [
        new([new[] { new[] { 1, 2, 3 }, new[] { 4, 5, 6 }, new[] { 7, 8, 9 } }], new[] { new[] { 7, 4, 1 }, new[] { 8, 5, 2 }, new[] { 9, 6, 3 } }),
        new([new[] { new[] { 1, 2 }, new[] { 3, 4 } }], new[] { new[] { 3, 1 }, new[] { 4, 2 } }),
    ];
}
