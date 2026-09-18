namespace CSharpCodingQuestions.Questions.Dsa.Strings;

[Question(Order = 5, Title = "Roman Numeral to Number", Level = Easy, Problem = """
    Convert a Roman numeral to a number. Symbols: `I=1, V=5, X=10, L=50, C=100, D=500, M=1000`.
    A smaller symbol **before** a bigger one is subtracted: `IV = 4`, `IX = 9`, `XC = 90`.
    `"MCMXCIV"` → `1994`.
    """)]
public static class RomanToInteger
{
    [Approach(Name = "Look at the Next Symbol", Time = "O(n)", Space = "O(1)", Idea = """
        Go through the symbols from left to right:

        - If the symbol is **smaller** than the one after it, subtract it (the `I` in `IV`).
        - Otherwise, add it.
        """)]
    public static int RomanToNumber(string roman)
    {
        var values = new Dictionary<char, int>
        {
            ['I'] = 1, ['V'] = 5, ['X'] = 10, ['L'] = 50,
            ['C'] = 100, ['D'] = 500, ['M'] = 1000,
        };

        int total = 0;
        for (int i = 0; i < roman.Length; i++)
        {
            int value = values[roman[i]];
            bool nextIsBigger = i + 1 < roman.Length && values[roman[i + 1]] > value;
            if (nextIsBigger)
            {
                total -= value;
            }
            else
            {
                total += value;
            }
        }
        return total;
    }

    public static Example[] Examples =>
    [
        new(["III"], 3),
        new(["LVIII"], 58),
        new(["MCMXCIV"], 1994),
    ];
}
