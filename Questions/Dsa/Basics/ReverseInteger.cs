namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 10, Title = "Reverse the Digits of a Number", Level = Easy, Problem = """
    Reverse the digits of an integer: `123` → `321`, `-456` → `-654`, `1200` → `21`.
    """)]
public static class ReverseInteger
{
    [Approach(Name = "Convert to Text", Time = "O(d)", Space = "O(d)", Idea = """
        Turn the number into a string, reverse the characters, and parse it back.
        Handle the minus sign separately. `d` is the number of digits.
        """)]
    public static int ReverseUsingText(int number)
    {
        bool isNegative = number < 0;
        char[] digits = Math.Abs(number).ToString().ToCharArray();
        Array.Reverse(digits);
        int reversed = int.Parse(new string(digits));
        return isNegative ? -reversed : reversed;
    }

    [Approach(Name = "Math with % and /", Time = "O(d)", Space = "O(1)", Idea = """
        - `number % 10` gives the last digit (`123 % 10 = 3`).
        - `number / 10` removes the last digit (`123 / 10 = 12`).

        Take digits off the end one by one and build the new number with `reversed * 10 + digit`.
        This works for negative numbers too, because the remainder keeps the sign.
        """)]
    public static int ReverseUsingMath(int number)
    {
        int reversed = 0;
        while (number != 0)
        {
            int lastDigit = number % 10;
            reversed = reversed * 10 + lastDigit;
            number /= 10;
        }
        return reversed;
    }

    public static Example[] Examples =>
    [
        new([123], 321),
        new([-456], -654),
        new([1200], 21),
    ];
}
