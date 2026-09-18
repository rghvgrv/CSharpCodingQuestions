namespace CSharpCodingQuestions.Questions.Dsa.RecursionAndBacktracking;

[Question(Order = 6, Title = "Word Search in a Grid", Level = Medium, Problem = """
    Can the word be spelled by moving between neighboring cells (up, down, left, right) of the letter grid?
    Each cell may be used only once.
    """)]
public static class WordSearch
{
    [Approach(Name = "Backtracking From Every Cell", Time = "O(cells · 3ᴸ)", Space = "O(L)", Idea = """
        Try to start the word at every cell. From a cell whose letter matches, try all 4 neighbors for the next letter.

        - **Mark** the cell as used (temporarily replace it with `#`) so the path can't come back through it.
        - **Restore** the letter when backing out, so other paths can use it.

        `L` is the word length. After the first step, each step has at most 3 new directions.
        """)]
    public static bool Exists(string[] rows, string word)
    {
        char[][] grid = rows.Select(row => row.ToCharArray()).ToArray();
        for (int row = 0; row < grid.Length; row++)
        {
            for (int column = 0; column < grid[0].Length; column++)
            {
                if (Search(grid, word, 0, row, column))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static bool Search(char[][] grid, string word, int index, int row, int column)
    {
        if (index == word.Length)
        {
            return true;
        }
        if (row < 0 || column < 0 || row >= grid.Length || column >= grid[0].Length || grid[row][column] != word[index])
        {
            return false;
        }

        char saved = grid[row][column];
        grid[row][column] = '#';
        bool found = Search(grid, word, index + 1, row + 1, column)
                  || Search(grid, word, index + 1, row - 1, column)
                  || Search(grid, word, index + 1, row, column + 1)
                  || Search(grid, word, index + 1, row, column - 1);
        grid[row][column] = saved;
        return found;
    }

    public static Example[] Examples =>
    [
        new([new[] { "ABCE", "SFCS", "ADEE" }, "ABCCED"], true),
        new([new[] { "ABCE", "SFCS", "ADEE" }, "SEE"], true),
        new([new[] { "ABCE", "SFCS", "ADEE" }, "ABCB"], false),
    ];
}
