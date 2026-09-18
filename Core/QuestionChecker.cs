namespace CSharpCodingQuestions.Core;

/// <summary>`dotnet run -- --check`: runs every question and fails if any answer is wrong, crashes or times out.</summary>
public static class QuestionChecker
{
    public static int CheckAll()
    {
        int failed = 0;
        foreach (QuestionInfo question in QuestionCatalog.Questions)
        {
            var clock = Stopwatch.StartNew();
            QuestionResult result = QuestionRunner.Run(question);
            if (result.Errors.Count == 0)
            {
                Console.Error.WriteLine($"ok   #{question.Number} {question.Id} ({clock.ElapsedMilliseconds} ms)");
                continue;
            }

            failed++;
            Console.Error.WriteLine($"FAIL #{question.Number} {question.Id}");
            foreach (string error in result.Errors)
            {
                Console.Error.WriteLine($"     {error}");
            }
        }

        int total = QuestionCatalog.Questions.Count;
        Console.Error.WriteLine($"{total - failed}/{total} passed");
        return failed == 0 ? 0 : 1;
    }
}
