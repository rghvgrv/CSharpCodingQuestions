namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_07, "N-Queens", Hard,
"Place n queens on an n×n chessboard so no two attack each other. Count all solutions and print one.")]
public static class NQueens
{
    // One queen per row. Track used columns and both diagonals (r - c and r + c are constant along them).
    public static List<string[]> Solve(int n)
    {
        var solutions = new List<string[]>();
        var queenCol = new int[n];
        var cols = new bool[n];
        var diag = new bool[2 * n];  // r - c + n
        var anti = new bool[2 * n];  // r + c

        void Place(int r)
        {
            if (r == n)
            {
                solutions.Add(queenCol.Select(c => new string('.', c) + "Q" + new string('.', n - c - 1)).ToArray());
                return;
            }
            for (int c = 0; c < n; c++)
            {
                if (cols[c] || diag[r - c + n] || anti[r + c]) continue;
                cols[c] = diag[r - c + n] = anti[r + c] = true;
                queenCol[r] = c;
                Place(r + 1);
                cols[c] = diag[r - c + n] = anti[r + c] = false;
            }
        }
        Place(0);
        return solutions;
    }

    public static void Run()
    {
        var four = Solve(4);
        Check("n=4 solutions", four.Count, 2);
        foreach (var row in four[0]) Console.WriteLine("  " + row);
        Check("n=8 solutions", Solve(8).Count, 92);
    }
}
