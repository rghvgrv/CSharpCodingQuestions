using CSharpCodingQuestions.Core;

ConsoleCapture.Install();

if (args.Contains("--check"))
{
    return QuestionChecker.CheckAll();
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/api/catalog", () => new
{
    Sections = QuestionCatalog.Topics
        .GroupBy(topic => topic.SectionTitle)
        .Select(section => new
        {
            Title = section.Key,
            Topics = section.Select(topic => new
            {
                topic.Id,
                topic.Title,
                topic.Summary,
                Questions = QuestionCatalog.Questions
                    .Where(question => question.Topic == topic)
                    .Select(question => new { question.Id, question.Number, question.Title, Level = question.Level.ToString() }),
            }),
        }),
});

app.MapGet("/api/topics/{section}/{topic}", (string section, string topic) =>
{
    TopicInfo? found = QuestionCatalog.Topics.FirstOrDefault(item => item.Id.Equals($"{section}/{topic}", StringComparison.OrdinalIgnoreCase));
    return found == null ? Results.NotFound() : Results.Ok(new { found.Id, found.SectionTitle, found.Title, found.Body });
});

app.MapGet("/api/questions/{id}", (string id) =>
{
    if (!QuestionCatalog.ById.TryGetValue(id, out QuestionInfo? question))
    {
        return Results.NotFound();
    }

    QuestionResult result = QuestionRunner.GetResult(question);
    return Results.Ok(new
    {
        question.Id,
        question.Number,
        question.Title,
        Level = question.Level.ToString(),
        question.Problem,
        Topic = new { question.Topic.Id, question.Topic.Title, question.Topic.SectionTitle },
        Approaches = question.Approaches.Select(approach => new { approach.Name, approach.Time, approach.Space, approach.Idea, approach.Code }),
        Result = new { result.Examples, result.Text, result.Errors },
    });
});

app.MapFallbackToFile("index.html");
app.Run();
return 0;
