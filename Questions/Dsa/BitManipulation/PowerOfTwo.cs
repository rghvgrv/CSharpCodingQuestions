namespace CSharpCodingQuestions.Questions.Dsa.BitManipulation;

[Question(Order = 2, Title = "Is It a Power of Two?", Level = Easy, Problem = """
    Return `true` if the number is 1, 2, 4, 8, 16, … (2 raised to some power).
    """)]
public static class PowerOfTwo
{
    [Approach(Name = "Keep Dividing by 2", Time = "O(log n)", Space = "O(1)", Idea = """
        While the number is even, divide it by 2. A power of two ends up at exactly 1.
        """)]
    public static bool IsPowerOfTwoByDividing(int number)
    {
        if (number <= 0)
        {
            return false;
        }
        while (number % 2 == 0)
        {
            number /= 2;
        }
        return number == 1;
    }

    [Approach(Name = "One Bit Trick", Time = "O(1)", Space = "O(1)", Idea = """
        A power of two has **exactly one** 1 bit: `8 = 1000`.
        Subtracting 1 flips that bit and turns on all bits below it: `7 = 0111`.
        So for a power of two, `number & (number - 1)` is 0. For any other positive number, it isn't.
        """)]
    public static bool IsPowerOfTwoWithBits(int number)
    {
        return number > 0 && (number & (number - 1)) == 0;
    }

    public static Example[] Examples =>
    [
        new([64], true),
        new([6], false),
        new([1], true),
        new([0], false),
    ];
}
