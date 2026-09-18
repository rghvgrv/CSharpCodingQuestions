namespace CSharpCodingQuestions.Questions.Dsa.DynamicProgramming;

[Question(Order = 7, Title = "Edit Distance", Level = Hard, Problem = """
    Find the fewest single-letter edits (insert, delete or replace a letter) needed to turn `from` into `to`.
    `"horse"` → `"ros"` takes `3` edits. Spell checkers use this to suggest words.
    """)]
public static class EditDistance
{
    [Approach(Name = "Plain Recursion", Time = "O(3^(m + n))", Space = "O(m + n)", Idea = """
        Look at the first letters:

        - Equal → no edit needed, continue with the rest of both words.
        - Different → try all three edits and take the cheapest:
          **replace** (skip a letter in both), **delete** (skip a letter in `from`), **insert** (skip a letter in `to`).
        """)]
    public static int DistanceRecursive(string from, string to)
    {
        return Distance(from, to, 0, 0);
    }

    private static int Distance(string from, string to, int i, int j)
    {
        if (i == from.Length)
        {
            return to.Length - j;    // insert the rest
        }
        if (j == to.Length)
        {
            return from.Length - i;  // delete the rest
        }
        if (from[i] == to[j])
        {
            return Distance(from, to, i + 1, j + 1);
        }

        int replace = Distance(from, to, i + 1, j + 1);
        int delete = Distance(from, to, i + 1, j);
        int insert = Distance(from, to, i, j + 1);
        return 1 + Math.Min(replace, Math.Min(delete, insert));
    }

    [Approach(Name = "Table (Bottom-Up)", Time = "O(m · n)", Space = "O(m · n)", Idea = """
        `table[i, j]` = the edits needed to turn the first `i` letters of `from` into the first `j` letters of `to`.

        - First row and column: turning a word into the empty word (or back) takes as many edits as it has letters.
        - Letters equal → copy the diagonal cell.
        - Different → `1 + min(diagonal, above, left)` = replace, delete, insert.
        """)]
    public static int DistanceTable(string from, string to)
    {
        int[,] table = new int[from.Length + 1, to.Length + 1];
        for (int i = 0; i <= from.Length; i++)
        {
            table[i, 0] = i;
        }
        for (int j = 0; j <= to.Length; j++)
        {
            table[0, j] = j;
        }

        for (int i = 1; i <= from.Length; i++)
        {
            for (int j = 1; j <= to.Length; j++)
            {
                if (from[i - 1] == to[j - 1])
                {
                    table[i, j] = table[i - 1, j - 1];
                }
                else
                {
                    table[i, j] = 1 + Math.Min(table[i - 1, j - 1], Math.Min(table[i - 1, j], table[i, j - 1]));
                }
            }
        }
        return table[from.Length, to.Length];
    }

    public static Example[] Examples =>
    [
        new(["horse", "ros"], 3),
        new(["kitten", "sitting"], 3),
        new(["", "abc"], 3),
    ];
}
