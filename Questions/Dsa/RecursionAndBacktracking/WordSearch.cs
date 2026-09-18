namespace CodingQuestions.Dsa.RecursionAndBacktracking;

[Q(1_08_06, "Word Search in a Grid", Medium,
"Return true if a word can be built from letters of adjacent cells (up/down/left/right) in a grid. Each cell may be used once.")]
public static class WordSearch
{
    // DFS from every cell. Mark the cell as visited (temporarily overwrite it), explore 4 neighbors, restore.
    public static bool Exists(char[][] board, string word)
    {
        int rows = board.Length, cols = board[0].Length;
        bool Dfs(int r, int c, int k)
        {
            if (k == word.Length) return true;
            if (r < 0 || c < 0 || r >= rows || c >= cols || board[r][c] != word[k]) return false;
            char saved = board[r][c];
            board[r][c] = '#';
            bool found = Dfs(r + 1, c, k + 1) || Dfs(r - 1, c, k + 1) || Dfs(r, c + 1, k + 1) || Dfs(r, c - 1, k + 1);
            board[r][c] = saved;
            return found;
        }
        for (int r = 0; r < rows; r++)
            for (int c = 0; c < cols; c++)
                if (Dfs(r, c, 0)) return true;
        return false;
    }

    public static void Run()
    {
        char[][] board = ["ABCE".ToCharArray(), "SFCS".ToCharArray(), "ADEE".ToCharArray()];
        Check("\"ABCCED\"", Exists(board, "ABCCED"), true);
        Check("\"SEE\"", Exists(board, "SEE"), true);
        Check("\"ABCB\" (would reuse B)", Exists(board, "ABCB"), false);
    }
}
