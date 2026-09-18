namespace CSharpCodingQuestions.Questions.Dsa.Basics;

[Question(Order = 3, Title = "FizzBuzz", Level = Easy, Problem = """
    For every number from 1 to `n`:

    - multiples of both 3 and 5 → `"FizzBuzz"`
    - multiples of 3 → `"Fizz"`
    - multiples of 5 → `"Buzz"`
    - any other number → the number itself
    """)]
public static class FizzBuzz
{
    [Approach(Name = "Check Remainders", Time = "O(n)", Space = "O(n)", Idea = """
        `i % 3 == 0` means "i divides by 3 with nothing left over".
        Check "both 3 and 5" **first**. If you checked 3 first, 15 would wrongly become "Fizz".
        """)]
    public static List<string> FizzBuzzList(int n)
    {
        var result = new List<string>();
        for (int i = 1; i <= n; i++)
        {
            if (i % 3 == 0 && i % 5 == 0)
            {
                result.Add("FizzBuzz");
            }
            else if (i % 3 == 0)
            {
                result.Add("Fizz");
            }
            else if (i % 5 == 0)
            {
                result.Add("Buzz");
            }
            else
            {
                result.Add(i.ToString());
            }
        }
        return result;
    }

    public static Example[] Examples =>
    [
        new([15], new[] { "1", "2", "Fizz", "4", "Buzz", "Fizz", "7", "8", "Fizz", "Buzz", "11", "Fizz", "13", "14", "FizzBuzz" }),
        new([3], new[] { "1", "2", "Fizz" }),
    ];
}
