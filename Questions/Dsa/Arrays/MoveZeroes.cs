namespace CSharpCodingQuestions.Questions.Dsa.Arrays;

[Question(Order = 6, Title = "Move Zeroes to the End", Level = Easy, Problem = """
    Move every `0` to the end of the array and keep the other numbers in their original order.
    `[0, 1, 0, 3, 12]` → `[1, 3, 12, 0, 0]`.
    """)]
public static class MoveZeroes
{
    [Approach(Name = "New Array", Time = "O(n)", Space = "O(n)", Idea = """
        Copy the non-zero numbers into a new array, in order. The slots left over stay `0`, which is the default value of an `int` array.
        """)]
    public static int[] MoveZeroesWithNewArray(int[] numbers)
    {
        int[] result = new int[numbers.Length];
        int position = 0;
        foreach (int number in numbers)
        {
            if (number != 0)
            {
                result[position] = number;
                position++;
            }
        }
        return result;
    }

    [Approach(Name = "Two Pointers In Place", Time = "O(n)", Space = "O(1)", Idea = """
        `write` marks where the next non-zero number should go. For every non-zero number, swap it into `write` and move `write` forward.
        Zeros are pushed back as the non-zero numbers move forward.
        """)]
    public static int[] MoveZeroesInPlace(int[] numbers)
    {
        int write = 0;
        for (int read = 0; read < numbers.Length; read++)
        {
            if (numbers[read] != 0)
            {
                int temp = numbers[write];
                numbers[write] = numbers[read];
                numbers[read] = temp;
                write++;
            }
        }
        return numbers;
    }

    public static Example[] Examples =>
    [
        new([new[] { 0, 1, 0, 3, 12 }], new[] { 1, 3, 12, 0, 0 }),
        new([new[] { 4, 2, 0, 0, 1 }], new[] { 4, 2, 1, 0, 0 }),
        new([new[] { 0 }], new[] { 0 }),
    ];
}
