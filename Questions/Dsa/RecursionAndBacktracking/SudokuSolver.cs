namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 8, Title = "Sudoku Solver", Level = Hard, Problem = """
    Fill the empty cells (`0`) of a 9×9 Sudoku so every row, every column and every 3×3 box contains the digits 1–9 exactly once.
    """)]
public static class SudokuSolver
{
    [Approach(Name = "Backtracking", Time = "Exponential (fast in practice)", Space = "O(81)", Idea = """
        1. Find the first empty cell.
        2. Try the digits 1 to 9. A digit is allowed if it isn't already in that row, column or 3×3 box.
        3. Place it and recurse to fill the rest. If the rest can't be solved, erase it and try the next digit.
        4. If no digit fits, return `false` so the previous cell tries its next digit.

        Checking the rules before going deeper (**pruning**) is what makes this fast.
        """)]
    public static int[][] Solve(int[][] board)
    {
        Fill(board);
        return board;
    }

    private static bool Fill(int[][] board)
    {
        for (int row = 0; row < 9; row++)
        {
            for (int column = 0; column < 9; column++)
            {
                if (board[row][column] != 0)
                {
                    continue;
                }

                for (int digit = 1; digit <= 9; digit++)
                {
                    if (CanPlace(board, row, column, digit))
                    {
                        board[row][column] = digit;
                        if (Fill(board))
                        {
                            return true;
                        }
                        board[row][column] = 0;
                    }
                }
                return false;   // nothing fits here: go back
            }
        }
        return true;   // no empty cells left
    }

    private static bool CanPlace(int[][] board, int row, int column, int digit)
    {
        int boxRow = row / 3 * 3;
        int boxColumn = column / 3 * 3;
        for (int i = 0; i < 9; i++)
        {
            if (board[row][i] == digit || board[i][column] == digit || board[boxRow + i / 3][boxColumn + i % 3] == digit)
            {
                return false;
            }
        }
        return true;
    }

    public static Example[] Examples =>
    [
        new(
            [new[]
            {
                new[] { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
                new[] { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
                new[] { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
                new[] { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
                new[] { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
                new[] { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
                new[] { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
                new[] { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
                new[] { 0, 0, 0, 0, 8, 0, 0, 7, 9 },
            }],
            new[]
            {
                new[] { 5, 3, 4, 6, 7, 8, 9, 1, 2 },
                new[] { 6, 7, 2, 1, 9, 5, 3, 4, 8 },
                new[] { 1, 9, 8, 3, 4, 2, 5, 6, 7 },
                new[] { 8, 5, 9, 7, 6, 1, 4, 2, 3 },
                new[] { 4, 2, 6, 8, 5, 3, 7, 9, 1 },
                new[] { 7, 1, 3, 9, 2, 4, 8, 5, 6 },
                new[] { 9, 6, 1, 5, 3, 7, 2, 8, 4 },
                new[] { 2, 8, 7, 4, 1, 9, 6, 3, 5 },
                new[] { 3, 4, 5, 2, 8, 6, 1, 7, 9 },
            }),
    ];
}
