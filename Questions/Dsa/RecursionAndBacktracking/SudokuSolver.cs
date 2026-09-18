namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_08, "Sudoku Solver", Hard,
"Fill a 9×9 Sudoku (0 = empty) so every row, column and 3×3 box contains 1–9 exactly once.")]
public static class SudokuSolver
{
    // Find an empty cell, try 1..9 where valid, recurse; undo if the rest can't be solved.
    public static bool Solve(int[,] g)
    {
        for (int r = 0; r < 9; r++)
            for (int c = 0; c < 9; c++)
            {
                if (g[r, c] != 0) continue;
                for (int v = 1; v <= 9; v++)
                {
                    if (!CanPlace(g, r, c, v)) continue;
                    g[r, c] = v;
                    if (Solve(g)) return true;
                    g[r, c] = 0;
                }
                return false; // nothing fits here → backtrack
            }
        return true; // no empty cells
    }

    static bool CanPlace(int[,] g, int r, int c, int v)
    {
        for (int i = 0; i < 9; i++)
            if (g[r, i] == v || g[i, c] == v || g[r / 3 * 3 + i / 3, c / 3 * 3 + i % 3] == v) return false;
        return true;
    }

    public static void Run()
    {
        int[,] g =
        {
            { 5, 3, 0, 0, 7, 0, 0, 0, 0 },
            { 6, 0, 0, 1, 9, 5, 0, 0, 0 },
            { 0, 9, 8, 0, 0, 0, 0, 6, 0 },
            { 8, 0, 0, 0, 6, 0, 0, 0, 3 },
            { 4, 0, 0, 8, 0, 3, 0, 0, 1 },
            { 7, 0, 0, 0, 2, 0, 0, 0, 6 },
            { 0, 6, 0, 0, 0, 0, 2, 8, 0 },
            { 0, 0, 0, 4, 1, 9, 0, 0, 5 },
            { 0, 0, 0, 0, 8, 0, 0, 7, 9 },
        };
        Check("Solved", Solve(g), true);
        for (int r = 0; r < 9; r++)
            Console.WriteLine("  " + string.Join(" ", Enumerable.Range(0, 9).Select(c => g[r, c])));
        Check("First row", Enumerable.Range(0, 9).Select(c => g[0, c]), [5, 3, 4, 6, 7, 8, 9, 1, 2]);
    }
}
