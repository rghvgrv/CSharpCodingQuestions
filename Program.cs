using CSharpCodingQuestions.Api;
using CSharpCodingQuestions.Core;

ConsoleCapture.Install();

if (args.Contains("--check"))
{
    return QuestionChecker.CheckAll();
}

if (args.Contains("--export"))
{
    return StaticSiteExporter.Export(outputFolder: "site");
}

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Same URLs as the static export (site/data/...), so one UI works both live and on a static host.
app.MapGet("/data/catalog.json", ApiResponses.Catalog);

app.MapGet("/data/topics/{section}/{topic}.json", (string section, string topic) =>
{
    TopicInfo? found = QuestionCatalog.Topics.FirstOrDefault(item => item.Id.Equals($"{section}/{topic}", StringComparison.OrdinalIgnoreCase));
    return found == null ? Results.NotFound() : Results.Ok(ApiResponses.Topic(found));
});

app.MapGet("/data/questions/{id}.json", (string id) =>
    QuestionCatalog.ById.TryGetValue(id, out QuestionInfo? question)
        ? Results.Ok(ApiResponses.Question(question))
        : Results.NotFound());

app.MapFallbackToFile("index.html");
app.Run();
return 0;
