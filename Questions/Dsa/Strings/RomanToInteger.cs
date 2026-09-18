namespace CodingQuestions.Dsa.Strings;

[Q(1_03_05, "Roman Numerals ⇄ Integer", Easy,
"Convert a Roman numeral to an integer, and an integer (1–3999) back to a Roman numeral.")]
public static class RomanToInteger
{
    static readonly Dictionary<char, int> Value = new() { ['I'] = 1, ['V'] = 5, ['X'] = 10, ['L'] = 50, ['C'] = 100, ['D'] = 500, ['M'] = 1000 };

    // If a symbol is smaller than the one after it, subtract it (IV = 4), otherwise add.
    public static int ToInt(string s)
    {
        int total = 0;
        for (int i = 0; i < s.Length; i++)
        {
            int v = Value[s[i]];
            total += i + 1 < s.Length && v < Value[s[i + 1]] ? -v : v;
        }
        return total;
    }

    // Greedy: take the biggest value that fits, repeat.
    static readonly (int Value, string Symbol)[] Table =
        [(1000, "M"), (900, "CM"), (500, "D"), (400, "CD"), (100, "C"), (90, "XC"), (50, "L"), (40, "XL"), (10, "X"), (9, "IX"), (5, "V"), (4, "IV"), (1, "I")];

    public static string ToRoman(int n)
    {
        var sb = new StringBuilder();
        foreach (var (value, symbol) in Table)
            for (; n >= value; n -= value) sb.Append(symbol);
        return sb.ToString();
    }

    public static void Run()
    {
        Check("ToInt(\"III\")", ToInt("III"), 3);
        Check("ToInt(\"LVIII\")", ToInt("LVIII"), 58);
        Check("ToInt(\"MCMXCIV\")", ToInt("MCMXCIV"), 1994);
        Check("ToRoman(1994)", ToRoman(1994), "MCMXCIV");
        Check("ToRoman(3749)", ToRoman(3749), "MMMDCCXLIX");
    }
}
