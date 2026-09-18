namespace CSharpCodingQuestions.Questions.Dsa.BitManipulation;

[Question(Order = 1, Title = "Count the 1 Bits", Level = Easy, Problem = """
    Count how many bits are `1` in the binary form of a non-negative number. `11` is `1011` in binary → `3`.
    """)]
public static class CountSetBits
{
    [Approach(Name = "Convert to a Binary String", Time = "O(log n)", Space = "O(log n)", Idea = """
        `Convert.ToString(number, 2)` gives the binary text, like `"1011"`. Count its `'1'` characters.
        Easy, but it creates a string just to count.
        """)]
    public static int CountWithString(int number)
    {
        string binary = Convert.ToString(number, 2);
        int count = 0;
        foreach (char digit in binary)
        {
            if (digit == '1')
            {
                count++;
            }
        }
        return count;
    }

    [Approach(Name = "Check the Last Bit, Then Shift", Time = "O(log n)", Space = "O(1)", Idea = """
        `number & 1` is the last bit (1 if odd). Add it to the count, then `number >>= 1` drops that bit.
        Repeat until the number is 0. Always one step per bit, including the 0 bits.
        """)]
    public static int CountWithShifting(int number)
    {
        int count = 0;
        while (number > 0)
        {
            count += number & 1;
            number >>= 1;
        }
        return count;
    }

    [Approach(Name = "Brian Kernighan's Trick", Time = "O(number of 1 bits)", Space = "O(1)", Idea = """
        `number & (number - 1)` **removes the lowest 1 bit**: `1100 & 1011 = 1000`.
        So the loop runs once per 1 bit, skipping all the zeros.
        (.NET also has this built in: `BitOperations.PopCount`.)
        """)]
    public static int CountWithKernighan(int number)
    {
        int count = 0;
        while (number != 0)
        {
            number &= number - 1;
            count++;
        }
        return count;
    }

    public static Example[] Examples =>
    [
        new([11], 3),
        new([128], 1),
        new([255], 8),
        new([0], 0),
    ];
}
